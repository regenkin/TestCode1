using System;
namespace KfCrm.Model
{
	/// <summary>
	/// sys_info:ʵ����(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
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

