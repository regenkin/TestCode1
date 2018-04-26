using System;
namespace KfCrm.Model
{
    /// <summary>
    /// CRM_Customer:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class CRM_Customer
    {
        public CRM_Customer()
        { }
        #region Model
        private int _id;
        private string _serialnumber;
        private string _customer;
        private string _address;
        private string _tel;
        private string _fax;
        private string _site;
        private string _industry;
        private int? _provinces_id;
        private string _provinces;
        private int? _city_id;
        private string _city;
        private int? _customertype_id;
        private string _customertype;
        private int? _customerlevel_id;
        private string _customerlevel;
        private int? _customersource_id;
        private string _customersource;
        private string _descripe;
        private string _remarks;
        private int? _department_id;
        private string _department;
        private int? _employee_id;
        private string _employee;
        private string _privatecustomer;
        private DateTime? _lastfollow;
        private int? _create_id;
        private string _create_name;
        private DateTime? _create_date;
        private int? _isdelete;
        private DateTime? _delete_time;
        private int? _industry_id;
        /// <summary>
        /// 
        /// </summary>
        public int id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Serialnumber
        {
            set { _serialnumber = value; }
            get { return _serialnumber; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Customer
        {
            set { _customer = value; }
            get { return _customer; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string address
        {
            set { _address = value; }
            get { return _address; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string tel
        {
            set { _tel = value; }
            get { return _tel; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string fax
        {
            set { _fax = value; }
            get { return _fax; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string site
        {
            set { _site = value; }
            get { return _site; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string industry
        {
            set { _industry = value; }
            get { return _industry; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? Provinces_id
        {
            set { _provinces_id = value; }
            get { return _provinces_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Provinces
        {
            set { _provinces = value; }
            get { return _provinces; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? City_id
        {
            set { _city_id = value; }
            get { return _city_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string City
        {
            set { _city = value; }
            get { return _city; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? CustomerType_id
        {
            set { _customertype_id = value; }
            get { return _customertype_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CustomerType
        {
            set { _customertype = value; }
            get { return _customertype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? CustomerLevel_id
        {
            set { _customerlevel_id = value; }
            get { return _customerlevel_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CustomerLevel
        {
            set { _customerlevel = value; }
            get { return _customerlevel; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? CustomerSource_id
        {
            set { _customersource_id = value; }
            get { return _customersource_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CustomerSource
        {
            set { _customersource = value; }
            get { return _customersource; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DesCripe
        {
            set { _descripe = value; }
            get { return _descripe; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Remarks
        {
            set { _remarks = value; }
            get { return _remarks; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? Department_id
        {
            set { _department_id = value; }
            get { return _department_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Department
        {
            set { _department = value; }
            get { return _department; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? Employee_id
        {
            set { _employee_id = value; }
            get { return _employee_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Employee
        {
            set { _employee = value; }
            get { return _employee; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string privatecustomer
        {
            set { _privatecustomer = value; }
            get { return _privatecustomer; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? lastfollow
        {
            set { _lastfollow = value; }
            get { return _lastfollow; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? Create_id
        {
            set { _create_id = value; }
            get { return _create_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Create_name
        {
            set { _create_name = value; }
            get { return _create_name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? Create_date
        {
            set { _create_date = value; }
            get { return _create_date; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? isDelete
        {
            set { _isdelete = value; }
            get { return _isdelete; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? Delete_time
        {
            set { _delete_time = value; }
            get { return _delete_time; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? industry_id
        {
            set { _industry_id = value; }
            get { return _industry_id; }
        }
        #endregion Model

    }
}

