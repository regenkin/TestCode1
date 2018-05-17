using System;
using System.Reflection;
using log4net;
namespace DTcms.Web.MVC.Areas.admin
{
    public static class LogHelper
    {
        public static readonly log4net.ILog loginfo = log4net.LogManager.GetLogger("loginfo");
        public static readonly log4net.ILog logerror = log4net.LogManager.GetLogger("logerror");
        public static readonly log4net.ILog logDebug = log4net.LogManager.GetLogger("logdebug");

        /// <summary>
        /// 运行信息
        /// </summary>
        /// <param name="info"></param>
        public static string InfoLog(object info)
        {
            string s = GetTimeStamp();
            if (loginfo.IsInfoEnabled)
            {
                loginfo.Info("(" + s + ")" + info);
            }
            return s;
        }
        /// <summary>
        /// 运行信息
        /// </summary>
        /// <param name="info"></param>
        /// <param name="se"></param>
        public static string InfoLog(object info, Exception se)
        {
            string s = GetTimeStamp();
            if (loginfo.IsInfoEnabled)
            {
                loginfo.Info("(" + s + ")" + info, se);
            }
            return s;
        }
        /// <summary>
        /// 错误信息
        /// </summary>
        /// <param name="info"></param>
        public static string ErrorLog(object info)
        {
            string s = GetTimeStamp();
            if (logerror.IsErrorEnabled)
            {
                logerror.Error("(" + s + ")" + info);
            }
            return s;
        }
        /// <summary>
        /// 错误信息
        /// </summary>
        /// <param name="info"></param>
        /// <param name="se"></param>
        public static string ErrorLog(string info, Exception se)
        {
            string s = GetTimeStamp();
            if (logerror.IsErrorEnabled)
            {
                logerror.Error("(" + s + ")" + info, se);
            }
            return s;
        }
        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="obj"></param>
        public static string DebugLog(object obj)
        {
            string s = GetTimeStamp();
            if (logDebug.IsDebugEnabled)
            {
                logDebug.Debug("(" + s + ")" + obj);
            }
            return s;
        }
        /// <summary>  
        /// 获取当前时间戳  
        /// </summary>  
        /// <param name="bflag">为真时获取10位时间戳,为假时获取13位时间戳.bool bflag = true</param>  
        /// <returns></returns>  
        public static string GetTimeStamp(bool bflag = false)
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            string ret = string.Empty;
            if (bflag)
                ret = Convert.ToInt64(ts.TotalSeconds).ToString();
            else
                ret = Convert.ToInt64(ts.TotalMilliseconds).ToString();
            return ret;
        }
    }
}