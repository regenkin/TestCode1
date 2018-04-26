using System;
namespace KfCrm.Model
{
	/// <summary>
	/// CRM_invoice:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class CRM_invoice
	{
		public CRM_invoice()
		{}
		#region Model
		private int _id;
		private int? _customer_id;
		private string _customer_name;
		private string _invoice_num;
		private int? _invoice_type_id;
		private string _invoice_type;
		private decimal? _invoice_amount;
		private string _invoice_content;
		private DateTime? _invoice_date;
		private int? _c_depid;
		private string _c_depname;
		private int? _c_empid;
		private string _c_empname;
		private int? _create_id;
		private string _create_name;
		private DateTime? _create_date;
		private int? _order_id;
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
		public string invoice_num
		{
			set{ _invoice_num=value;}
			get{return _invoice_num;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? invoice_type_id
		{
			set{ _invoice_type_id=value;}
			get{return _invoice_type_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string invoice_type
		{
			set{ _invoice_type=value;}
			get{return _invoice_type;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? invoice_amount
		{
			set{ _invoice_amount=value;}
			get{return _invoice_amount;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string invoice_content
		{
			set{ _invoice_content=value;}
			get{return _invoice_content;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? invoice_date
		{
			set{ _invoice_date=value;}
			get{return _invoice_date;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? C_depid
		{
			set{ _c_depid=value;}
			get{return _c_depid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string C_depname
		{
			set{ _c_depname=value;}
			get{return _c_depname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? C_empid
		{
			set{ _c_empid=value;}
			get{return _c_empid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string C_empname
		{
			set{ _c_empname=value;}
			get{return _c_empname;}
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
		public string create_name
		{
			set{ _create_name=value;}
			get{return _create_name;}
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
		public int? order_id
		{
			set{ _order_id=value;}
			get{return _order_id;}
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

