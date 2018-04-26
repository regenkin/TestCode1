﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Senparc.Weixin.MP.Entities;

namespace Senparc.Weixin.MP.AdvancedAPIs
{
    public class BaseInterfaceAnalysisResult
    {
        /// <summary>
        /// 数据的日期，需在begin_date和end_date之间
        /// </summary>
        public string ref_date { get; set; }
        /// <summary>
        /// 通过服务器配置地址获得消息后，被动回复用户消息的次数      
        /// </summary>
        public int callback_count { get; set; }
        /// <summary>
        /// 上述动作的失败次数
        /// </summary>
        public int fail_count { get; set; }
        /// <summary>
        /// 总耗时，除以callback_count即为平均耗时
        /// </summary>
        public int total_time_cost { get; set; }
        /// <summary>
        /// 最大耗时
        /// </summary>
        public int max_time_cost { get; set; }
    }

    /// <summary>
    /// 获取接口分析数据返回结果
    /// </summary>
    public class InterfaceSummaryResultJson : WxJsonResult
    {
        public List<BaseInterfaceAnalysisResult> list { get; set; } 
    }

    /// <summary>
    /// 获取接口分析分时数据返回结果
    /// </summary>
    public class InterfaceSummaryHourResultJson : WxJsonResult
    {
        public List<InterfaceSummaryHour> list { get; set; } 
    }

    public class InterfaceSummaryHour : BaseInterfaceAnalysisResult
    {
        /// <summary>
        /// 数据的小时
        /// </summary>
        public int ref_hour { get; set; }
    }
}
