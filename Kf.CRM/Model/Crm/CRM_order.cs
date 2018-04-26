using System;
namespace KfCrm.Model
{
	/// <summary>
	/// CRM_order:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class CRM_order
	{
		public CRM_order()
		{}
		#region Model
		private int _id;
		private string _serialnumber;
		private int? _customer_id;
		private string _customer_name;
		private DateTime? _order_date;
		private int? _pay_type_id;
		private string _pay_type;
		private string _order_details;
		private int? _order_status_id;
		private string _order_status;
		private decimal? _order_amount;
		private int? _create_id;
		private DateTime? _create_date;
		private int? _c_dep_id;
		private string _c_dep_name;
		private int? _c_emp_id;
		private string _c_emp_name;
		private int? _f_dep_id;
		private string _f_dep_name;
		private int? _f_emp_id;
		private string _f_emp_name;
		private decimal? _receive_money;
		private decimal? _arrears_money;
		private decimal? _invoice_money;
		private decimal? _arrears_invoice;
		private int? _isdelete;
		private DateTime? _delete_time;
		/// <summary>
		/// 
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Serialnumber
		{
			set{ _serialnumber=value;}
			get{return _serialnumber;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Customer_id
		{
			set{ _customer_id=value;}
			get{return _customer_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Customer_name
		{
			set{ _customer_name=value;}
			get{return _customer_name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? Order_date
		{
			set{ _order_date=value;}
			get{return _order_date;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? pay_type_id
		{
			set{ _pay_type_id=value;}
			get{return _pay_type_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string pay_type
		{
			set{ _pay_type=value;}
			get{return _pay_type;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Order_details
		{
			set{ _order_details=value;}
			get{return _order_details;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Order_status_id
		{
			set{ _order_status_id=value;}
			get{return _order_status_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Order_status
		{
			set{ _order_status=value;}
			get{return _order_status;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? Order_amount
		{
			set{ _order_amount=value;}
			get{return _order_amount;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? create_id
		{
			set{ _create_id=value;}
			get{return _create_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? create_date
		{
			set{ _create_date=value;}
			get{return _create_date;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? C_dep_id
		{
			set{ _c_dep_id=value;}
			get{return _c_dep_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string C_dep_name
		{
			set{ _c_dep_name=value;}
			get{return _c_dep_name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? C_emp_id
		{
			set{ _c_emp_id=value;}
			get{return _c_emp_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string C_emp_name
		{
			set{ _c_emp_name=value;}
			get{return _c_emp_name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? F_dep_id
		{
			set{ _f_dep_id=value;}
			get{return _f_dep_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string F_dep_name
		{
			set{ _f_dep_name=value;}
			get{return _f_dep_name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? F_emp_id
		{
			set{ _f_emp_id=value;}
			get{return _f_emp_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string F_emp_name
		{
			set{ _f_emp_name=value;}
			get{return _f_emp_name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? receive_money
		{
			set{ _receive_money=value;}
			get{return _receive_money;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? arrears_money
		{
			set{ _arrears_money=value;}
			get{return _arrears_money;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? invoice_money
		{
			set{ _invoice_money=value;}
			get{return _invoice_money;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? arrears_invoice
		{
			set{ _arrears_invoice=value;}
			get{return _arrears_invoice;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? isDelete
		{
			set{ _isdelete=value;}
			get{return _isdelete;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? Delete_time
		{
			set{ _delete_time=value;}
			get{return _delete_time;}
		}
		#endregion Model

	}
}

