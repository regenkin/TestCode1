using System;
namespace KfCrm.Model
{
	/// <summary>
	/// Sys_Menu:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class Sys_Menu
	{
		public Sys_Menu()
		{}
		#region Model
		private int _menu_id;
		private string _menu_name;
		private int? _parentid;
		private string _parentname;
		private int? _app_id;
		private string _menu_url;
		private string _menu_icon;
		private string _menu_handler;
		private int? _menu_order;
		private string _menu_type;
		/// <summary>
		/// 
		/// </summary>
		public int Menu_id
		{
			set{ _menu_id=value;}
			get{return _menu_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Menu_name
		{
			set{ _menu_name=value;}
			get{return _menu_name;}
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
		public int? App_id
		{
			set{ _app_id=value;}
			get{return _app_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Menu_url
		{
			set{ _menu_url=value;}
			get{return _menu_url;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Menu_icon
		{
			set{ _menu_icon=value;}
			get{return _menu_icon;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Menu_handler
		{
			set{ _menu_handler=value;}
			get{return _menu_handler;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Menu_order
		{
			set{ _menu_order=value;}
			get{return _menu_order;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Menu_type
		{
			set{ _menu_type=value;}
			get{return _menu_type;}
		}
		#endregion Model

	}
}

