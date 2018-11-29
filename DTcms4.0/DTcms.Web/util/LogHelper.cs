using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DTcms.Web
{
    public class LogHelper
    {
        static LogHelper()
        {
            //log4net.Config.DOMConfigurator.Configure();
            log4net.Config.DOMConfigurator.Configure(new System.IO.FileInfo(System.AppDomain.CurrentDomain.BaseDirectory + "\\xmlconfig\\Log4net.config"));
        }
        public static readonly log4net.ILog loginfo = log4net.LogManager.GetLogger("loginfo");
        public static readonly log4net.ILog logerror = log4net.LogManager.GetLogger("logerror");
        public static readonly log4net.ILog logDebug = log4net.LogManager.GetLogger("logdebug");
    }
}