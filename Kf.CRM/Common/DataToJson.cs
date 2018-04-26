using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Microsoft.JScript;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Reflection;
using System.IO;

namespace KfCrm.Common
{
    public class DataToJson
    {
        public DataToJson() { }

        #region
        public static string DataToJSON(DataSet ds)
        {
            StringBuilder JsonString = new StringBuilder();
            DataTable dt = ds.Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                string rowsjson = JsonConvert.SerializeObject(dt, new DataTableConverter());
                return rowsjson.Replace("[","").Replace("]","");
            }
            else
            {
                return null;
            }
        }
        public static string DataToJSON_nomal(DataSet ds)
        {
            StringBuilder JsonString = new StringBuilder();
            DataTable dt = ds.Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                //JsonString.Append("[");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    JsonString.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (j < dt.Columns.Count - 1)
                        {
                            //JsonString.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + "\"" + dt.Rows[i][j].ToString() + "\",");
                            JsonString.Append(dt.Columns[j].ColumnName.ToString() + ":" + "\"" + (dt.Rows[i][j].ToString()) + "\",");
                        }
                        else if (j == dt.Columns.Count - 1)
                        {
                            //JsonString.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + "\"" + dt.Rows[i][j].ToString() + "\"");
                            JsonString.Append(dt.Columns[j].ColumnName.ToString() + ":" + "\"" + (dt.Rows[i][j].ToString()) + "\"");
                        }
                    }
                    if (i == dt.Rows.Count - 1)
                    {
                        JsonString.Append("}");
                    }
                    else
                    {
                        JsonString.Append("},");
                    }
                }
                //JsonString.Append("]");
                return JsonString.ToString();
            }
            else
            {
                return null;
            }
        }
        public static string GetJson(DataSet ds)
        {
            try
            {
                return JsonConvert.SerializeObject(ds.Tables[0], new DataTableConverter());
            }
            catch (Exception err)
            {
                return "{[]}";
            }
        }

        #endregion

    }

    //
    // ObjectListToJSON
    // Copyright (c) 2008 pcode. All rights reserved.
    //
    //  Author(s):
    //
    //      pcode,[email]jy@cjlu.edu.cn[/email]
    //  �������ڽ�List<object>ת��Ϊjson���ݸ�ʽ
    //  Ŀǰ���ܴ���һ��object�Ļ����������Ͷ��Ҷ�[ { }] \�ȶ�json���˺�Ӱ���������û�����⴦��
    //  ϣ�����ֵܼ�������



    public class ObjectListToJSON
    {
        #region ����һ�������������Ժ�����ֵ�ͽ�һ������ķ�������װ��jsons��ʽ
        /**
          * �����ȫ�����Ժ�����ֵ��������дjson��{}������
          * ���ɺ�ĸ�ʽ����
          * "����1":"����ֵ"
          * ����Щ������������ֵд���ַ����б���
          * */
        private List<string> GetObjectProperty(FileInfo o)
        {
            List<string> propertyslist = new List<string>();
            PropertyInfo[] propertys = o.GetType().GetProperties();
            foreach (PropertyInfo p in propertys)
            {
                propertyslist.Add("\"" + p.Name.ToString() + "\":\"" + p.GetValue(o, null) + "\"");
            }
            return propertyslist;
        }
        /**
           * ��һ��������������Ժ�����ֵ��json�ĸ�ʽҪ������Ϊһ����װ��Ľ����
           *
           * ����ֵ����{"����1":"����1ֵ","����2":"����2ֵ","����3":"����3ֵ"}
           * 
           * */
        private string OneObjectToJSON(FileInfo o)
        {
            string result = "{";
            List<string> ls_propertys = new List<string>();
            ls_propertys = GetObjectProperty(o);
            foreach (string str_property in ls_propertys)
            {
                if (result.Equals("{"))
                {
                    result = result + str_property;
                }
                else
                {
                    result = result + "," + str_property + "";
                }
            }
            return result + "}";
        }
        #endregion
        /**
          * �Ѷ����б�ת����json��
          * */
        public string toJSON(List<FileInfo> objlist)
        {//��д��������һ����дclassname�Ļ���
            return toJSON(objlist, string.Empty);
        }
        public string toJSON(List<FileInfo> objlist, string classname)
        {
            string result = "[";
            //if (classname.Equals(string.Empty))//���û�и���������ƣ���ô���������ذ�һ��
            //{
            //    object o = objlist[0];
            //    classname = o.GetType().ToString();
            //}
            //result += "\"" + classname + "\":[";
            bool firstline = true;//�����һ��ǰ�治��","��
            foreach (FileInfo oo in objlist)
            {
                if (!firstline)
                {
                    result = result + "," + OneObjectToJSON(oo);
                }
                else
                {
                    result = result + OneObjectToJSON(oo) + "";
                    firstline = false;
                }
            }
            return result + "]";
        }

    }

}
