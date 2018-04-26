using System;
namespace KfCrm.Model
{
	/// <summary>
	/// CRM_product_category:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class CRM_product_category
	{
		public CRM_product_category()
		{}
		#region Model
		private int _id;
		private string _product_category;
		private int? _parentid;
		private string _product_icon;
		private int? _isdelete;
		private int? _delete_id;
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
		public string product_category
		{
			set{ _product_category=value;}
			get{return _product_category;}
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
		public string product_icon
		{
			set{ _product_icon=value;}
			get{return _product_icon;}
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
		public int? Delete_id
		{
			set{ _delete_id=value;}
			get{return _delete_id;}
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

