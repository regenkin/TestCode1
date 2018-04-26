using System;
namespace KfCrm.Model
{
	/// <summary>
	/// Sys_Button:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class Sys_Button
	{
		public Sys_Button()
		{}
		#region Model
		private int _btn_id;
		private string _btn_name;
		private string _btn_icon;
		private string _btn_handler;
		private int? _menu_id;
		private string _menu_name;
		private string _btn_order;
		/// <summary>
		/// 
		/// </summary>
		public int Btn_id
		{
			set{ _btn_id=value;}
			get{return _btn_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Btn_name
		{
			set{ _btn_name=value;}
			get{return _btn_name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Btn_icon
		{
			set{ _btn_icon=value;}
			get{return _btn_icon;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Btn_handler
		{
			set{ _btn_handler=value;}
			get{return _btn_handler;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Menu_id
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
		public string Btn_order
		{
			set{ _btn_order=value;}
			get{return _btn_order;}
		}
		#endregion Model

	}
}

