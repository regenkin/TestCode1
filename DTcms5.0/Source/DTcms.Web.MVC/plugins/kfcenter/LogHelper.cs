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
        public static void InfoLog(object info)
        {
            if (loginfo.IsInfoEnabled)
            {
                loginfo.Info(info);
            }
        }
        /// <summary>
        /// 运行信息
        /// </summary>
        /// <param name="info"></param>
        /// <param name="se"></param>
        public static void InfoLog(object info, Exception se)
        {
            if (loginfo.IsInfoEnabled)
            {
                loginfo.Info(info, se);
            }
        }
        /// <summary>
        /// 错误信息
        /// </summary>
        /// <param name="info"></param>
        public static void ErrorLog(object info)
        {
            if (logerror.IsErrorEnabled)
            {
                logerror.Error(info);
            }
        }
        /// <summary>
        /// 错误信息
        /// </summary>
        /// <param name="info"></param>
        /// <param name="se"></param>
        public static void ErrorLog(string info, Exception se)
        {
            if (logerror.IsErrorEnabled)
            {
                logerror.Error(info, se);
            }
        }
        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="obj"></param>
        public static void DebugLog(object obj)
        {
            if (logDebug.IsDebugEnabled)
            {
                logDebug.Debug(obj);
            }
        }
    }
}