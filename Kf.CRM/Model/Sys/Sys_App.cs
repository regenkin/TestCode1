using System;
namespace KfCrm.Model
{
	/// <summary>
	/// Sys_App:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class Sys_App
	{
		public Sys_App()
		{}
		#region Model
		private int _id;
		private string _app_name;
		private int? _app_order;
		private string _app_url;
		private string _app_handler;
		private string _app_type;
		private string _app_icon;
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
		public string App_name
		{
			set{ _app_name=value;}
			get{return _app_name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? App_order
		{
			set{ _app_order=value;}
			get{return _app_order;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string App_url
		{
			set{ _app_url=value;}
			get{return _app_url;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string App_handler
		{
			set{ _app_handler=value;}
			get{return _app_handler;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string App_type
		{
			set{ _app_type=value;}
			get{return _app_type;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string App_icon
		{
			set{ _app_icon=value;}
			get{return _app_icon;}
		}
		#endregion Model

	}
}

