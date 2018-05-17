using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTcms.Web.Mvc.Plugin.KfCenter.Util
{
    public class PostDataBase
    {
        public PostDataBase() { }

        private string _token = string.Empty;

        public string token
        {
            get { return _token; }
            set { _token = value; }
        }

        private string _appid = string.Empty;

        public string appid
        {
            get { return _appid; }
            set { _appid = value; }
        }

        private string _appsecret = string.Empty;

        public string Appsecret
        {
            get { return _appsecret; }
            set { _appsecret = value; }
        }
    
    }

    /// <summary>
    /// Post提交标准格式
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PostData<T> : PostDataBase
    {
        public PostData():base(){}

        public T data { get; set; }
    }

    /// <summary>
    /// json返格标准格式
    /// </summary>
    public class ReturnData
    {
        /// <summary>
        /// ReturnStatus 状态：0-成功，非0相应错误码
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public object Data { get; set; }
    }
}
