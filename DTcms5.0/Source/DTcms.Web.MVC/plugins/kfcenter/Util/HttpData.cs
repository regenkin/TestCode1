using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTcms.Web.Mvc.Plugin.KfCenter.Util
{
    /// <summary>
    /// 提交数据基本格式
    /// </summary>
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

    /// <summary>
    /// 全局令牌
    /// </summary>
    public class TokenData
    {
        public TokenData() { }

        private string _AccessToken = string.Empty;
        /// <summary>
        /// 全局令牌
        /// </summary>
        public string AccessToken
        {
            get { return _AccessToken; }
            set { _AccessToken = value; }
        }

        private int? _Expire = 0;
        /// <summary>
        /// 令牌失效秒
        /// </summary>
        public int? Expire
        {
            get { return _Expire; }
            set { _Expire = value; }
        }

        private DateTime _ExpireTime = DateTime.MinValue.AddDays(1);
        /// <summary>
        /// 令牌本机失效时间
        /// </summary>
        public DateTime ExpireTime
        {
            get { return _ExpireTime; }
            set { _ExpireTime = value; }
        }
    }
}
