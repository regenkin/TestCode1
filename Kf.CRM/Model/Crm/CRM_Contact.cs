using System;
namespace KfCrm.Model
{
	/// <summary>
	/// CRM_Contact:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class CRM_Contact
	{
		public CRM_Contact()
		{}
		#region Model
		private int _id;
		private string _c_name;
		private string _c_sex;
		private string _c_department;
		private string _c_position;
		private string _c_birthday;
		private string _c_tel;
		private string _c_fax;
		private string _c_email;
		private string _c_mob;
		private string _c_qq;
		private string _c_add;
		private string _c_hobby;
		private string _c_remarks;
		private int? _c_customerid;
		private string _c_customername;
		private int? _c_createid;
		private DateTime? _c_createdate;
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
		public string C_name
		{
			set{ _c_name=value;}
			get{return _c_name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string C_sex
		{
			set{ _c_sex=value;}
			get{return _c_sex;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string C_department
		{
			set{ _c_department=value;}
			get{return _c_department;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string C_position
		{
			set{ _c_position=value;}
			get{return _c_position;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string C_birthday
		{
			set{ _c_birthday=value;}
			get{return _c_birthday;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string C_tel
		{
			set{ _c_tel=value;}
			get{return _c_tel;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string C_fax
		{
			set{ _c_fax=value;}
			get{return _c_fax;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string C_email
		{
			set{ _c_email=value;}
			get{return _c_email;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string C_mob
		{
			set{ _c_mob=value;}
			get{return _c_mob;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string C_QQ
		{
			set{ _c_qq=value;}
			get{return _c_qq;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string C_add
		{
			set{ _c_add=value;}
			get{return _c_add;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string C_hobby
		{
			set{ _c_hobby=value;}
			get{return _c_hobby;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string C_remarks
		{
			set{ _c_remarks=value;}
			get{return _c_remarks;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? C_customerid
		{
			set{ _c_customerid=value;}
			get{return _c_customerid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string C_customername
		{
			set{ _c_customername=value;}
			get{return _c_customername;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? C_createId
		{
			set{ _c_createid=value;}
			get{return _c_createid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? C_createDate
		{
			set{ _c_createdate=value;}
			get{return _c_createdate;}
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

