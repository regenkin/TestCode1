using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTcms.Web.Mvc.Plugin.KfCenter.Factory
{
    /// <summary>
    /// 用来装载适合所有类的分页结果类
    /// </summary>
    public class PageDataView<T>
    {
        public PageDataView()
        {
            this._Items = new List<T>();
        }
        private int _TotalNum;
        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalNum
        {
            get { return _TotalNum; }
            set { _TotalNum = value; }
        }
        private IList<T> _Items;
        /// <summary>
        /// 记录集合
        /// </summary>
        public IList<T> Items
        {
            get { return _Items; }
            set { _Items = value; }
        }
        /// <summary>
        /// 当前页
        /// </summary>
        public int CurrentPage { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPageCount { get; set; }
    }
}
