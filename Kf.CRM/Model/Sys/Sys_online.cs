using System;
namespace KfCrm.Model
{
	/// <summary>
	/// Sys_online:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class Sys_online
	{
		public Sys_online()
		{}
		#region Model
		private int? _userid;
		private string _username;
		private DateTime? _lastlogtime;
		/// <summary>
		/// 
		/// </summary>
		public int? UserID
		{
			set{ _userid=value;}
			get{return _userid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string UserName
		{
			set{ _username=value;}
			get{return _username;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? LastLogTime
		{
			set{ _lastlogtime=value;}
			get{return _lastlogtime;}
		}
		#endregion Model

	}
}

