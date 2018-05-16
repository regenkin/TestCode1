/*
Name: Kinfar
Author: Kinfar
Description:数据中心账套组
*/
using System.Runtime.Serialization;

namespace DTcms.Web.Mvc.Plugin.KfCenter.Models
{
    /// <summary>
    /// 数据中心账套组
    /// </summary>
    [DataContract]
    public class kfActGroup
    {
        private int _id;
        [DataMember]
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _parentGroupKey;
        [DataMember]
        public string ParentGroupKey
        {
            get { return _parentGroupKey; }
            set { _parentGroupKey = value; }
        }
        private string _actGroupNum;
        [DataMember]
        public string ActGroupNum
        {
            get { return _actGroupNum; }
            set { _actGroupNum = value; }
        }
        private string _actGroupName;
        [DataMember]
        public string ActGroupName
        {
            get { return _actGroupName; }
            set { _actGroupName = value; }
        }
    }
}