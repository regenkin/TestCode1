using System;
namespace KfCrm.Model
{
	/// <summary>
	/// Sys_role:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class Sys_role
	{
		public Sys_role()
		{}
		#region Model
		private int _roleid;
		private string _rolename;
		private string _roledscript;
		private int? _rolesort;
		private int? _createid;
		private DateTime? _createdate;
		private int? _updateid;
		private DateTime? _updatedate;
		/// <summary>
		/// 
		/// </summary>
		public int RoleID
		{
			set{ _roleid=value;}
			get{return _roleid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string RoleName
		{
			set{ _rolename=value;}
			get{return _rolename;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string RoleDscript
		{
			set{ _roledscript=value;}
			get{return _roledscript;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? RoleSort
		{
			set{ _rolesort=value;}
			get{return _rolesort;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? CreateID
		{
			set{ _createid=value;}
			get{return _createid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? CreateDate
		{
			set{ _createdate=value;}
			get{return _createdate;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? UpdateID
		{
			set{ _updateid=value;}
			get{return _updateid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? UpdateDate
		{
			set{ _updatedate=value;}
			get{return _updatedate;}
		}
		#endregion Model

	}
}

