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
    //  此类用于将List<object>转换为json数据格式
    //  目前仅能处理一个object的基础数据类型而且对[ { }] \等对json有伤害影响特殊符号没有特殊处理
    //  希望有兄弟继续完善



    public class ObjectListToJSON
    {
        #region 反射一个对象所有属性和属性值和将一个对象的反射结果封装成jsons格式
        /**
          * 对象的全部属性和属性值。用于填写json的{}内数据
          * 生成后的格式类似
          * "属性1":"属性值"
          * 将这些属性名和属性值写入字符串列表返回
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
           * 将一个对象的所有属性和属性值按json的格式要求输入为一个封装后的结果。
           *
           * 返回值类似{"属性1":"属性1值","属性2":"属性2值","属性3":"属性3值"}
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
          * 把对象列表转换成json串
          * */
        public string toJSON(List<FileInfo> objlist)
        {//覆写，给懒人一个不写classname的机会
            return toJSON(objlist, string.Empty);
        }
        public string toJSON(List<FileInfo> objlist, string classname)
        {
            string result = "[";
            //if (classname.Equals(string.Empty))//如果没有给定类的名称，那么自做聪明地安一个
            //{
            //    object o = objlist[0];
            //    classname = o.GetType().ToString();
            //}
            //result += "\"" + classname + "\":[";
            bool firstline = true;//处理第一行前面不加","号
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
