using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Text;
using System.Security.Cryptography;
using System.Configuration;
using System.IO;
using System.Web.SessionState;

namespace KfCrm.CRM.Data
{
    /// <summary>
    /// upload 的摘要说明
    /// </summary>
    public class upload : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            HttpRequest request = context.Request;
            if (request["Action"] == "upload")
            {
                string fileName = request["filename"];    //文件路径
                fileName = fileName.Substring(fileName.LastIndexOf('\\') + 1);

                HttpPostedFile uploadFile = request.Files[0];
                uploadFile.SaveAs(context.Server.MapPath(@"~/images/upload/temp/" + fileName));
                //this.File1.PostedFile.SaveAs(page.Server.MapPath(@"~/focusimage/" + fileName));
                context.Response.Write(@"../images/upload/temp/" + fileName);
            }
            if (request["Action"] == "cus_import")
            {
                string fileName = request["filename"];    //文件路径
                fileName = fileName.Substring(fileName.LastIndexOf('\\') + 1);
                string sExt = fileName.Substring(fileName.LastIndexOf(".")).ToLower();

                DateTime now = DateTime.Now;
                string nowfileName = now.ToString("yyyyMMddHHmmss") + GetRandom(6) + sExt;

                HttpPostedFile uploadFile = request.Files[0];
                uploadFile.SaveAs(context.Server.MapPath(@"~/file/customer/" + nowfileName));

                context.Response.Write(nowfileName);
            }
            if (request["Action"] == "upheadimg")
            {
                int x1 = int.Parse(request["x1"]);
                int y1 = int.Parse(request["y1"]);
                int w = int.Parse(request["w"]);
                int h = int.Parse(request["h"]);

                string fileName = request["upload"];
                fileName = fileName.Substring(fileName.LastIndexOf('\\') + 1);
                string sExt = fileName.Substring(fileName.LastIndexOf(".")).ToLower();

                DateTime now = DateTime.Now;
                string nowfileName = now.ToString("yyyyMMddHHmmss") + GetRandom(6) + sExt;

                System.Web.UI.Page page = new Page();

                string oldpath = page.Server.MapPath(@"~/images/upload/temp/" + fileName);
                string currpath = page.Server.MapPath(@"~/images/upload/portrait/" + nowfileName);

                System.Drawing.Image originalImg = System.Drawing.Image.FromFile(oldpath);

                ZoomImage.SaveCutPic(oldpath, currpath, 0, 0, w, h, x1, y1, Convert.ToInt32(300 * originalImg.Width / originalImg.Height), 300);

                originalImg.Dispose();

                System.IO.File.Delete(oldpath);

                context.Response.Write(nowfileName);

            }
            if (request["Action"] == "contract")
            {
                try
                {
                    HttpPostedFile file;
                    for (int i = 0; i < request.Files.Count; ++i)
                    {
                        file = request.Files[i];
                        if (file == null || file.ContentLength == 0 || string.IsNullOrEmpty(file.FileName)) continue;

                        string filename = file.FileName;
                        string sExt = filename.Substring(filename.LastIndexOf(".")).ToLower();
                        DateTime now = DateTime.Now;
                        string nowfileName = now.ToString("yyyyMMddHHmmss") + GetRandom(6) + sExt;

                        file.SaveAs(HttpContext.Current.Server.MapPath("../file/contract/" + nowfileName));

                        context.Response.Write(nowfileName);
                    }
                }
                catch (Exception ex)
                {
                    context.Response.StatusCode = 500;
                    context.Response.Write(ex.Message);
                    context.Response.End();
                }
                finally
                {
                    context.Response.End();
                }
            }
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