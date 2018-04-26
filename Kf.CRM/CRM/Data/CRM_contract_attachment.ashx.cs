using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data;
using KfCrm.Common;
using System.IO;
using Word = Microsoft.Office.Interop.Word;
using System.Web.Security;
using System.Security.Cryptography;

namespace KfCrm.CRM.Data
{
    /// <summary>
    /// CRM_contract_attachment 的摘要说明
    /// </summary>
    public class CRM_contract_attachment : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            HttpRequest request = context.Request;

            BLL.CRM_contract_attachment cca = new BLL.CRM_contract_attachment();
            Model.CRM_contract_attachment model = new Model.CRM_contract_attachment();

            var cookie = context.Request.Cookies[FormsAuthentication.FormsCookieName];
            var ticket = FormsAuthentication.Decrypt(cookie.Value);
            string CoockiesID = ticket.UserData;

            BLL.hr_employee emp = new BLL.hr_employee();
            int emp_id = int.Parse(CoockiesID);
            DataSet dsemp = emp.GetList("id=" + emp_id);
            string empname = dsemp.Tables[0].Rows[0]["name"].ToString();
            string uid = dsemp.Tables[0].Rows[0]["uid"].ToString();

            if (request["Action"] == "insert_attachment")
            {
                string fileName = PageValidate.InputText( request["filename"],250);    //文件路径
                fileName = fileName.Substring(fileName.LastIndexOf('\\') + 1);
                string sExt = fileName.Substring(fileName.LastIndexOf(".")).ToLower();

                DateTime now = DateTime.Now;
                string nowfileName = now.ToString("yyyyMMddHHmmss") + GetRandom(6) + sExt;

                HttpPostedFile uploadFile = request.Files[0];
                uploadFile.SaveAs(context.Server.MapPath(@"~/file/contract/" + nowfileName));

                context.Response.Write(nowfileName);

                string contract_id = PageValidate.InputText(request["contract_id"], 50);
                if (PageValidate.IsNumber(contract_id))
                    model.contract_id = int.Parse(contract_id);

                model.file_id = Guid.NewGuid().ToString();
                model.file_name = fileName;
                model.real_name = nowfileName;
                model.page_id = PageValidate.InputText(request["page_id"], 255);
                model.file_size = uploadFile.ContentLength;

                model.create_id = emp_id;
                model.create_name = PageValidate.InputText(empname, 255);
                model.create_date = DateTime.Now;

                cca.Add(model);
            }
            if (request["Action"] == "flush_attachment")
            {
                string page_id = PageValidate.InputText(request["page_id"], 255);
                DataSet attachment = cca.GetList("page_id='" + page_id + "'");
                cca.Delete("page_id='" + page_id + "'");

                for (int i = 0; i < attachment.Tables[0].Rows.Count; i++)
                {
                    try
                    {
                        string filename = attachment.Tables[0].Rows[i]["real_name"].ToString();
                        string sExt = filename.Substring(filename.LastIndexOf('.'));
                        string html = filename.Substring(0, filename.Length - sExt.Length) + ".html";
                        string files = filename.Substring(0, filename.Length - sExt.Length) + ".files";
                        File.Delete(HttpContext.Current.Server.MapPath("../file/contract/" + filename));
                        File.Delete(HttpContext.Current.Server.MapPath("../file/contract/" + html));

                        DirectoryInfo dir = new DirectoryInfo(HttpContext.Current.Server.MapPath("../file/contract/" + files));
                        dir.Delete(true);
                    }
                    catch
                    { }
                }
            }
            if (request["Action"] == "del_attachment")
            {
                string file_id = PageValidate.InputText(request["file_id"], 255);
                string page_id = PageValidate.InputText(request["page_id"], 255);

                DataSet attachment = cca.GetList("file_id='" + file_id + "' and page_id='" + page_id + "'");
                cca.Delete("file_id='" + file_id + "' and page_id='" + page_id + "'");

                for (int i = 0; i < attachment.Tables[0].Rows.Count; i++)
                {
                    try
                    {
                        File.Delete(HttpContext.Current.Server.MapPath("../file/contract/" + attachment.Tables[0].Rows[i]["real_name"].ToString()));
                    }
                    catch
                    { }
                }
            }
            if (request["Action"] == "get_attachment")
            {
                string whereStr = "";
                string contract_id = PageValidate.InputText(request["contract_id"],50);
                string page_id = PageValidate.InputText(request["page_id"], 50);

                if (!string.IsNullOrEmpty(contract_id) && contract_id != "null")
                    whereStr = string.Format(" contract_id={0}", contract_id);
                else if (!string.IsNullOrEmpty(page_id) && page_id != "null")
                    whereStr = string.Format(" page_id={0}", page_id);
                else
                    whereStr = " 1=2 ";
                

                DataSet ds = cca.GetList(whereStr);
                string dt = Common.GetGridJSON.DataTableToJSON(ds.Tables[0]);

                context.Response.Write(dt);
            }
            if (request["Action"] == "get_realname")
            {
                string page_id = PageValidate.InputText(request["page_id"], 255);
                string file_name = PageValidate.InputText(request["filename"], 255);

                DataSet ds = cca.GetList(string.Format("page_id='{0}' and file_name='{1}'", page_id, file_name));

                if (ds.Tables[0].Rows.Count > 0)
                    context.Response.Write(ds.Tables[0].Rows[0]["real_name"].ToString());
                else
                    context.Response.Write("sucess:false");
            }
            if (request["Action"] == "get_office")
            {
                string real_name = PageValidate.InputText(request["realname"], 255);
                string page_id = PageValidate.InputText(request["page_id"], 255);
                string file_name = PageValidate.InputText(request["filename"], 255);

                if (string.IsNullOrEmpty(real_name) || real_name == "undefined")
                {
                    DataSet ds = cca.GetList(string.Format("page_id='{0}' and file_name='{1}'", page_id, file_name));

                    if (ds.Tables[0].Rows.Count > 0)
                        real_name = ds.Tables[0].Rows[0]["real_name"].ToString();
                    else
                        real_name = null;
                }

                if (!string.IsNullOrEmpty(real_name))
                {
                    string filename = context.Server.MapPath(@"../file/contract/" + real_name);
                    string readname = WordToHtml(filename);
                    //StreamReader fread = new StreamReader(readname, System.Text.Encoding.GetEncoding("gb2312"));
                    //string ss = fread.ReadToEnd();
                    string sExt = readname.Substring(readname.LastIndexOf("\\")).ToLower();
                    real_name = real_name.Replace(sExt, "");
                    context.Response.Write(sExt);
                    //fread.Close();
                    //fread.Dispose();
                    try
                    {
                        // File.Delete(readname); 
                    }
                    catch { }

                }

            }
        }
        /// <summary> 
        /// word转成html 
        /// </summary> 
        /// <param name="wordFileName"></param> 
        private string WordToHtml(object wordFileName)
        {
            //在此处放置用户代码以初始化页面 
            Word.Application word = new Word.Application();
            Type wordType = word.GetType();
            Word.Documents docs = word.Documents;
            //打开文件 
            Type docsType = docs.GetType();
            Word.Document doc = (Word.Document)docsType.InvokeMember("Open", System.Reflection.BindingFlags.InvokeMethod, null, docs, new Object[] { wordFileName, true, true });
            //转换格式，另存为 
            Type docType = doc.GetType();
            string wordSaveFileName = wordFileName.ToString();
            string sExt = wordSaveFileName.Substring(wordSaveFileName.LastIndexOf('.'));
            string strSaveFileName = wordSaveFileName.Substring(0, wordSaveFileName.Length - sExt.Length) + ".html";
            object saveFileName = (object)strSaveFileName;
            docType.InvokeMember("SaveAs", System.Reflection.BindingFlags.InvokeMethod, null, doc, new object[] { saveFileName, Word.WdSaveFormat.wdFormatFilteredHTML });
            docType.InvokeMember("Close", System.Reflection.BindingFlags.InvokeMethod, null, doc, null);
            //退出 Word 
            wordType.InvokeMember("Quit", System.Reflection.BindingFlags.InvokeMethod, null, word, null);
            return saveFileName.ToString();
        }

        #region GetRandom
        private string GetRandom(int length)
        {
            byte[] random = new Byte[length / 2];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetNonZeroBytes(random);

            StringBuilder sb = new StringBuilder(length);
            int i;
            for (i = 0; i < random.Length; i++)
            {
                sb.Append(String.Format("{0:X2}", random[i]));
            }
            return sb.ToString();
        }
        #endregion

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}