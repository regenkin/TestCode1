/*----------------------------------------------------------------
        // Copyright (C) 2016 Kinfar.
        // 版权所有
        // 开发者：Kinfar.
        // Email：kinfar@foxmail.net
        // QQ：3133119519
//----------------------------------------------------------------*/

using System;

namespace Kinfar.Frame.Operate.Base.ConditionChange
{
    public static class UnixTimeUtil
    {
        private static DateTime _baseTime = new DateTime(1970, 1, 1);

        /// <summary>
        /// 将.NET的DateTime转换为unix time
        /// </summary>
        /// <param name="dateTime">待转换的时间</param>
        /// <returns>转换后的unix time</returns>
        public static long FromDateTime(DateTime dateTime)
        {
            return (dateTime.Ticks - _baseTime.Ticks)/10000000 - 8*60*60;
            //return (dateTime.Ticks - new DateTime(1970, 1, 1, 0, 0, 0).Ticks) / 10000000;
        }
    }
}
