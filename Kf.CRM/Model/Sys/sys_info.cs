using System;
namespace KfCrm.Model
{
	/// <summary>
	/// sys_info:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class sys_info
	{
		public sys_info()
		{}
		#region Model
		private int _id;
		private string _sys_key;
		private string _sys_value;
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
		public string sys_key
		{
			set{ _sys_key=value;}
			get{return _sys_key;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string sys_value
		{
			set{ _sys_value=value;}
			get{return _sys_value;}
		}
		#endregion Model

	}
}

