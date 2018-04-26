using System;
namespace KfCrm.Model
{
    /// <summary>
    /// CRM_receive:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class CRM_receive
    {
        public CRM_receive()
        { }
        #region Model
        private int _id;
        private int? _customer_id;
        private string _customer_name;
        private string _receive_num;
        private int? _pay_type_id;
        private string _pay_type;
        private decimal? _receive_amount;
        private DateTime? _receive_date;
        private int? _c_depid;
        private string _c_depname;
        private int? _c_empid;
        private string _c_empname;
        private int? _create_id;
        private string _create_name;
        private DateTime? _create_date;
        private int? _companyid;
        private int? _order_id;
        private string _remarks;
        private int? _isdelete;
        private DateTime? _delete_time;
        private int? _receive_direction_id;
        private string _receive_direction_name;
        private int? _receive_type_id;
        private string _receive_type_name;
        private decimal? _receive_real;
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
        public int? Customer_id
        {
            set { _customer_id = value; }
            get { return _customer_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Customer_name
        {
            set { _customer_name = value; }
            get { return _customer_name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Receive_num
        {
            set { _receive_num = value; }
            get { return _receive_num; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? Pay_type_id
        {
            set { _pay_type_id = value; }
            get { return _pay_type_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Pay_type
        {
            set { _pay_type = value; }
            get { return _pay_type; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? Receive_amount
        {
            set { _receive_amount = value; }
            get { return _receive_amount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? Receive_date
        {
            set { _receive_date = value; }
            get { return _receive_date; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? C_depid
        {
            set { _c_depid = value; }
            get { return _c_depid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string C_depname
        {
            set { _c_depname = value; }
            get { return _c_depname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? C_empid
        {
            set { _c_empid = value; }
            get { return _c_empid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string C_empname
        {
            set { _c_empname = value; }
            get { return _c_empname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? create_id
        {
            set { _create_id = value; }
            get { return _create_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string create_name
        {
            set { _create_name = value; }
            get { return _create_name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? create_date
        {
            set { _create_date = value; }
            get { return _create_date; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? companyid
        {
            set { _companyid = value; }
            get { return _companyid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? order_id
        {
            set { _order_id = value; }
            get { return _order_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string remarks
        {
            set { _remarks = value; }
            get { return _remarks; }
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
        public int? receive_direction_id
        {
            set { _receive_direction_id = value; }
            get { return _receive_direction_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string receive_direction_name
        {
            set { _receive_direction_name = value; }
            get { return _receive_direction_name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? receive_type_id
        {
            set { _receive_type_id = value; }
            get { return _receive_type_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string receive_type_name
        {
            set { _receive_type_name = value; }
            get { return _receive_type_name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? receive_real
        {
            set { _receive_real = value; }
            get { return _receive_real; }
        }
        #endregion Model

    }
}

