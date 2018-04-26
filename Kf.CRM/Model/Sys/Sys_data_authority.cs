using System;
namespace KfCrm.Model
{
	/// <summary>
	/// Sys_data_authority:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class Sys_data_authority
	{
		public Sys_data_authority()
		{}
		#region Model
		private int? _role_id;
		private int? _option_id;
		private string _sys_option;
		private int? _sys_view;
		private int? _sys_add;
		private int? _sys_edit;
		private int? _sys_del;
		private int? _create_id;
		private DateTime? _create_date;
		/// <summary>
		/// 
		/// </summary>
		public int? Role_id
		{
			set{ _role_id=value;}
			get{return _role_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? option_id
		{
			set{ _option_id=value;}
			get{return _option_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Sys_option
		{
			set{ _sys_option=value;}
			get{return _sys_option;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Sys_view
		{
			set{ _sys_view=value;}
			get{return _sys_view;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Sys_add
		{
			set{ _sys_add=value;}
			get{return _sys_add;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Sys_edit
		{
			set{ _sys_edit=value;}
			get{return _sys_edit;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Sys_del
		{
			set{ _sys_del=value;}
			get{return _sys_del;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Create_id
		{
			set{ _create_id=value;}
			get{return _create_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? Create_date
		{
			set{ _create_date=value;}
			get{return _create_date;}
		}
		#endregion Model

	}
}

