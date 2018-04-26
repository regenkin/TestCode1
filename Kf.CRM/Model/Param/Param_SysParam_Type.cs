using System;
namespace KfCrm.Model
{
	/// <summary>
	/// Param_SysParam_Type:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class Param_SysParam_Type
	{
		public Param_SysParam_Type()
		{}
		#region Model
		private int _id;
		private string _params_name;
		private int? _params_order;
		private int? _isdelete;
		private DateTime? _delete_time;
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
		public string params_name
		{
			set{ _params_name=value;}
			get{return _params_name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? params_order
		{
			set{ _params_order=value;}
			get{return _params_order;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? isDelete
		{
			set{ _isdelete=value;}
			get{return _isdelete;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? Delete_time
		{
			set{ _delete_time=value;}
			get{return _delete_time;}
		}
		#endregion Model

	}
}

