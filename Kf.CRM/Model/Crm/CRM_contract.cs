using System;
namespace KfCrm.Model
{
	/// <summary>
	/// CRM_contract:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class CRM_contract
	{
		public CRM_contract()
		{}
		#region Model
		private int _id;
		private string _contract_name;
		private string _serialnumber;
		private int? _customer_id;
		private string _customer_name;
		private int? _c_depid;
		private string _c_depname;
		private int? _c_empid;
		private string _c_empname;
		private decimal? _contract_amount;
		private int? _pay_cycle;
		private string _start_date;
		private string _end_date;
		private string _sign_date;
		private string _customer_contractor;
		private int? _our_contractor_depid;
		private string _our_contractor_depname;
		private int? _our_contractor_id;
		private string _our_contractor_name;
		private int? _creater_id;
		private string _creater_name;
		private DateTime? _create_time;
		private string _main_content;
		private string _remarks;
		private string _file_serialnumber;
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
		public string Contract_name
		{
			set{ _contract_name=value;}
			get{return _contract_name;}
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
		public decimal? Contract_amount
		{
			set{ _contract_amount=value;}
			get{return _contract_amount;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Pay_cycle
		{
			set{ _pay_cycle=value;}
			get{return _pay_cycle;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Start_date
		{
			set{ _start_date=value;}
			get{return _start_date;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string End_date
		{
			set{ _end_date=value;}
			get{return _end_date;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Sign_date
		{
			set{ _sign_date=value;}
			get{return _sign_date;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Customer_Contractor
		{
			set{ _customer_contractor=value;}
			get{return _customer_contractor;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Our_Contractor_depid
		{
			set{ _our_contractor_depid=value;}
			get{return _our_contractor_depid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Our_Contractor_depname
		{
			set{ _our_contractor_depname=value;}
			get{return _our_contractor_depname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Our_Contractor_id
		{
			set{ _our_contractor_id=value;}
			get{return _our_contractor_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Our_Contractor_name
		{
			set{ _our_contractor_name=value;}
			get{return _our_contractor_name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Creater_id
		{
			set{ _creater_id=value;}
			get{return _creater_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Creater_name
		{
			set{ _creater_name=value;}
			get{return _creater_name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? Create_time
		{
			set{ _create_time=value;}
			get{return _create_time;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Main_Content
		{
			set{ _main_content=value;}
			get{return _main_content;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Remarks
		{
			set{ _remarks=value;}
			get{return _remarks;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string File_serialnumber
		{
			set{ _file_serialnumber=value;}
			get{return _file_serialnumber;}
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

