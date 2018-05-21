using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTcms.Web.Mvc.Plugin.KfCenter.Util
{
    /// <summary>
    /// PageCriteria是一个封装查询条件相关信息的类
    /// </summary>
    public class PageCriteria
    {
        private string _TableName;
        /// <summary>
        /// 表名,多表是请使用 tA a inner join tB b On a.AID = b.AID
        /// </summary>
        public string TableName
        {
            get { return _TableName; }
            set { _TableName = value; }
        }
        private string _Fileds = "*";
        /// <summary>
        /// 读取字段默认'*'
        /// </summary>
        public string Fields
        {
            get { return _Fileds; }
            set { _Fileds = value; }
        }
        private string _PrimaryKey = "ID";
        /// <summary>
        /// 主键，可以带表头 a.AID
        /// </summary>
        public string PrimaryKey
        {
            get { return _PrimaryKey; }
            set { _PrimaryKey = value; }
        }
        private int _PageSize = 10;
        /// <summary>
        /// 页大小 默认10
        /// </summary>
        public int PageSize
        {
            get { return _PageSize; }
            set { _PageSize = value; }
        }
        private int _CurrentPage = 1;
        /// <summary>
        /// 当前页码 默认1
        /// </summary>
        public int CurrentPage
        {
            get { return _CurrentPage; }
            set { _CurrentPage = value; }
        }
        private string _Sort = string.Empty;
        /// <summary>
        /// 排序字段
        /// </summary>
        public string Sort
        {
            get { return _Sort; }
            set { _Sort = value; }
        }
        private string _Condition = string.Empty;
        /// <summary>
        /// Where条件
        /// </summary>
        public string Condition
        {
            get { return _Condition; }
            set { _Condition = value; }
        }
        private int _RecordCount;
        /// <summary>
        /// 返回的 总记录数
        /// </summary>
        public int RecordCount
        {
            get { return _RecordCount; }
            set { _RecordCount = value; }
        }
    }
}
