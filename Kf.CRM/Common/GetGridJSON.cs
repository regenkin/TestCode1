using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Microsoft.JScript;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace KfCrm.Common
{
    public class GetGridJSON
    {
        public GetGridJSON() 
        {
        
        }

        #region
        public static string DataTableToJSON(DataTable dt)
        {
            try
            {
                string rowsjson = JsonConvert.SerializeObject(dt, new DataTableConverter());
                string json = @"{""Rows"":" + rowsjson + @",""Total"":""" + dt.Rows.Count.ToString() + @"""}";
                return json;
            }
            catch (Exception err)
            {
                return "{Rows:[],Total:0}";
            }
        }

        public static string DataTableToJSON1(DataTable dt,string Total)
        {
            try
            {
                string rowsjson = JsonConvert.SerializeObject(dt, new DataTableConverter());
                string json = @"{""Rows"":" + rowsjson + @",""Total"":""" + Total + @"""}";
                return json;
            }
            catch (Exception err)
            {
                return "{Rows:[],Total:0}";
            }
        }
        /// <summary>
        /// ÆÕÍ¨Êý×éjson
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DataTableToJSON2(DataTable dt)
        {
            try
            {
                string rowsjson = JsonConvert.SerializeObject(dt, new DataTableConverter());               
                return rowsjson;
            }
            catch (Exception err)
            {
                return "[]";
            }
        }
        #endregion
    }
}
