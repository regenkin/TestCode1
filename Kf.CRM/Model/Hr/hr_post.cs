using System;
namespace KfCrm.Model
{
	/// <summary>
	/// hr_post:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class hr_post
	{
		public hr_post()
		{}
		#region Model
		private int _post_id;
		private string _post_name;
		private int? _position_id;
		private string _position_name;
		private int? _position_order;
		private int? _dep_id;
		private string _depname;
		private int? _emp_id;
		private string _emp_name;
		private int? _default_post;
		private string _note;
		private string _post_descript;
		private int? _isdelete;
		private DateTime? _delete_time;
		/// <summary>
		/// 
		/// </summary>
		public int post_id
		{
			set{ _post_id=value;}
			get{return _post_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string post_name
		{
			set{ _post_name=value;}
			get{return _post_name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? position_id
		{
			set{ _position_id=value;}
			get{return _position_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string position_name
		{
			set{ _position_name=value;}
			get{return _position_name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? position_order
		{
			set{ _position_order=value;}
			get{return _position_order;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? dep_id
		{
			set{ _dep_id=value;}
			get{return _dep_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string depname
		{
			set{ _depname=value;}
			get{return _depname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? emp_id
		{
			set{ _emp_id=value;}
			get{return _emp_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string emp_name
		{
			set{ _emp_name=value;}
			get{return _emp_name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? default_post
		{
			set{ _default_post=value;}
			get{return _default_post;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string note
		{
			set{ _note=value;}
			get{return _note;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string post_descript
		{
			set{ _post_descript=value;}
			get{return _post_descript;}
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

