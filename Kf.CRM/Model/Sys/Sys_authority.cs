using System;
namespace KfCrm.Model
{
	/// <summary>
	/// Sys_authority:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class Sys_authority
	{
		public Sys_authority()
		{}
		#region Model
		private int _role_id;
		private string _app_ids;
		private string _menu_ids;
		private string _button_ids;
		private int? _create_id;
		private DateTime? _create_date;
		/// <summary>
		/// 
		/// </summary>
		public int Role_id
		{
			set{ _role_id=value;}
			get{return _role_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string App_ids
		{
			set{ _app_ids=value;}
			get{return _app_ids;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Menu_ids
		{
			set{ _menu_ids=value;}
			get{return _menu_ids;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Button_ids
		{
			set{ _button_ids=value;}
			get{return _button_ids;}
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

