using System;
namespace KfCrm.Model
{
	/// <summary>
	/// Sys_role_emp:ʵ����(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public partial class Sys_role_emp
	{
		public Sys_role_emp()
		{}
		#region Model
		private int? _roleid;
		private int? _empid;
		/// <summary>
		/// 
		/// </summary>
		public int? RoleID
		{
			set{ _roleid=value;}
			get{return _roleid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? empID
		{
			set{ _empid=value;}
			get{return _empid;}
		}
		#endregion Model

	}
}

