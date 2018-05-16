/*
Name: kinfar
Author: kinfar
Description:数据中心账套信息
*/
using System.Runtime.Serialization;
using System;

namespace DTcms.Web.Mvc.Plugin.KfCenter.Models
{
    /// <summary>
    /// 数据中心账套信息
    /// </summary>
    [Serializable]
    [DataContract]
    public class kfActSet
    {
        private int _id=0;
        [DataMember]
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _actsetNum=string.Empty;
        [DataMember]
        public string ActsetNum
        {
            get { return _actsetNum; }
            set { _actsetNum = value; }
        }
        private string _actsetName = string.Empty;
        [DataMember]
        public string ActsetName
        {
            get { return _actsetName; }
            set { _actsetName = value; }
        }
        private string _actsetDBName = string.Empty;
        [DataMember]
        public string ActsetDBName
        {
            get { return _actsetDBName; }
            set { _actsetDBName = value; }
        }
        private string _actsetType = string.Empty;
        [DataMember]
        public string ActsetType
        {
            get { return _actsetType; }
            set { _actsetType = value; }
        }
        private string _actSetGroupKey = string.Empty;
        [DataMember]
        public string ActSetGroupKey
        {
            get { return _actSetGroupKey; }
            set { _actSetGroupKey = value; }
        }
        private string _loginType = string.Empty;
        [DataMember]
        public string LoginType
        {
            get { return _loginType; }
            set { _loginType = value; }
        }
        private string _loginUserName = string.Empty;
        [DataMember]
        public string LoginUserName
        {
            get { return _loginUserName; }
            set { _loginUserName = value; }
        }
        private string _loginPwd = string.Empty;
        [DataMember]
        public string LoginPwd
        {
            get { return _loginPwd; }
            set { _loginPwd = value; }
        }
        private string _dBServerName = string.Empty;
        [DataMember]
        public string DBServerName
        {
            get { return _dBServerName; }
            set { _dBServerName = value; }
        }
        private System.DateTime _createDate = DateTime.Now;
        [DataMember]
        public System.DateTime CreateDate
        {
            get { return _createDate; }
            set { _createDate = value; }
        }
        private System.DateTime _newBackUpDate = new DateTime(1799-01-02);
        [DataMember]
        public System.DateTime NewBackUpDate
        {
            get { return _newBackUpDate; }
            set { _newBackUpDate = value; }
        }
        private string _dBGUID=Guid.NewGuid().ToString();
        [DataMember]
        public string DBGUID
        {
            get { return _dBGUID; }
            set { _dBGUID = value; }
        }
        private string _dBVersion = "2000";
        [DataMember]
        public string DBVersion
        {
            get { return _dBVersion; }
            set { _dBVersion = value; }
        }
        private int _visible=1;
        [DataMember]
        public int Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }
        private int _uIStyle=1;
        [DataMember]
        public int UIStyle
        {
            get { return _uIStyle; }
            set { _uIStyle = value; }
        }
        private int _limitCount=0;
        [DataMember]
        public int LimitCount
        {
            get { return _limitCount; }
            set { _limitCount = value; }
        }
        private decimal _dBMaxSize=0m;
        [DataMember]
        public decimal DBMaxSize
        {
            get { return _dBMaxSize; }
            set { _dBMaxSize = value; }
        }
        private System.DateTime _endDate=DateTime.Today.AddMonths(1);
        [DataMember]
        public System.DateTime EndDate
        {
            get { return _endDate; }
            set { _endDate = value; }
        }
    }
}