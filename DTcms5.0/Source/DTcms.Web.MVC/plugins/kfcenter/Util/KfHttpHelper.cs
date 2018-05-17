using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTcms.Web.Mvc.Plugin.KfCenter.Util
{
    public class KfHttpHelper
    {
        /// <summary>
        /// 令牌管理 过期自动获取
        /// </summary>
        public static string TOKEN
        {
            get { return "pufang"; }
        }

        /// <summary>
        /// 提交POST Json数据返回Json数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="postdata"></param>
        /// <returns></returns>
        public static ReturnData PostJson<T>(string url, PostData<T> postdata)
        {
            //赋值令牌
            postdata.token = KfHttpHelper.TOKEN;
            //创建Httphelper对象
            CsharpHttpHelper.HttpHelper http = new CsharpHttpHelper.HttpHelper();
            //创建Httphelper参数对象
            CsharpHttpHelper.HttpItem item = new CsharpHttpHelper.HttpItem()
            {
                URL = url,//URL     必需项    
                Method = "post",//URL     可选项 默认为Get   
                ContentType = "application/json;charset=urf-8",//返回类型    可选项有默认值 application/x-www-form-urlencoded 键值对
                PostDataType = CsharpHttpHelper.Enum.PostDataType.String,//默认为字符串，同时支持Byte和文件方法
                PostEncoding = System.Text.Encoding.UTF8,//默认为Default，
                Postdata = CsharpHttpHelper.HttpHelper.ObjectToJson(postdata),//Post要发送的数据
            };
            //请求的返回值对象
            CsharpHttpHelper.HttpResult result = http.GetHtml(item);
            //获取请请求的Html
            var rData = CsharpHttpHelper.HttpHelper.JsonToObject<ReturnData>(result.Html) as ReturnData;

            return rData;
        }
    }
}
