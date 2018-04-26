using System;
namespace KfCrm.Model
{
	/// <summary>
	/// hr_department:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class hr_department
	{
		public hr_department()
		{}
		#region Model
		private int _id;
		private string _d_name;
		private int? _parentid;
		private string _parentname;
		private string _d_type;
		private string _d_icon;
		private string _d_fuzeren;
		private string _d_tel;
		private string _d_fax;
		private string _d_add;
		private string _d_email;
		private string _d_miaoshu;
		private int? _d_order;
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
		public string d_name
		{
			set{ _d_name=value;}
			get{return _d_name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? parentid
		{
			set{ _parentid=value;}
			get{return _parentid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string parentname
		{
			set{ _parentname=value;}
			get{return _parentname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string d_type
		{
			set{ _d_type=value;}
			get{return _d_type;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string d_icon
		{
			set{ _d_icon=value;}
			get{return _d_icon;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string d_fuzeren
		{
			set{ _d_fuzeren=value;}
			get{return _d_fuzeren;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string d_tel
		{
			set{ _d_tel=value;}
			get{return _d_tel;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string d_fax
		{
			set{ _d_fax=value;}
			get{return _d_fax;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string d_add
		{
			set{ _d_add=value;}
			get{return _d_add;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string d_email
		{
			set{ _d_email=value;}
			get{return _d_email;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string d_miaoshu
		{
			set{ _d_miaoshu=value;}
			get{return _d_miaoshu;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? d_order
		{
			set{ _d_order=value;}
			get{return _d_order;}
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

