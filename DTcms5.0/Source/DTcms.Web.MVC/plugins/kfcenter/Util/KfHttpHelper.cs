using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTcms.Web.Mvc.Plugin.KfCenter.Util
{
    public class KfHttpHelper
    {
        private static TokenData _token = null;
        /// <summary>
        /// 令牌管理 过期自动获取
        /// </summary>
        public static TokenData TOKEN
        {
            get
            {
                if (_token == null || (_token != null && _token.ExpireTime.CompareTo (DateTime.Now) < 0))
                {
                    ReturnData rData = PostJson<string>(WebApiUrl.API_OAth_Account_GetGlobalToken, new PostData<string> { appid = "kinfar.net", Appsecret = "www.kinfar.net" }, false);
                    if (rData.Status == 1)
                    {
                        _token = CsharpHttpHelper.HttpHelper.JsonToObject<TokenData>(rData.Data.ToString()) as TokenData;
                        if (_token != null)
                            _token.ExpireTime = DateTime.Now.AddSeconds((double)(_token.Expire - 30));
                    }
                }
                return _token;
            }
        }

        /// <summary>
        /// 提交POST Json数据返回Json数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="postdata"></param>
        /// <param name="checktoken"></param>
        /// <returns></returns>
        public static ReturnData PostJson<T>(string url, PostData<T> postdata,bool checktoken=true)
        {
            //赋值令牌
            if (checktoken) postdata.token = KfHttpHelper.TOKEN.AccessToken;
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
            //令牌失效
            if (rData.Status == 401) _token = null;
            return rData;
        }

        /// <summary>
        /// 提交Get Json数据返回Json数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="postdata"></param>
        /// <param name="checktoken"></param>
        /// <returns></returns>
        public static ReturnData GetJson<T>(string url, PostData<T> postdata, bool checktoken = true)
        {
            //赋值令牌
            if (checktoken) postdata.token = KfHttpHelper.TOKEN.AccessToken;
            //创建Httphelper对象
            CsharpHttpHelper.HttpHelper http = new CsharpHttpHelper.HttpHelper();
            //创建Httphelper参数对象
            CsharpHttpHelper.HttpItem item = new CsharpHttpHelper.HttpItem()
            {
                URL = url,//URL     必需项    
                Method = "get",//URL     可选项 默认为Get   
                ContentType = "application/json;charset=urf-8",//返回类型    可选项有默认值 application/x-www-form-urlencoded 键值对
                PostDataType = CsharpHttpHelper.Enum.PostDataType.String,//默认为字符串，同时支持Byte和文件方法
                PostEncoding = System.Text.Encoding.UTF8,//默认为Default，
                Postdata = CsharpHttpHelper.HttpHelper.ObjectToJson(postdata),//Post要发送的数据
            };
            //请求的返回值对象
            CsharpHttpHelper.HttpResult result = http.GetHtml(item);
            //获取请请求的Html
            var rData = CsharpHttpHelper.HttpHelper.JsonToObject<ReturnData>(result.Html) as ReturnData;
            //令牌失效
            if (rData.Status == 401) _token = null;
            return rData;
        }
    }
}
