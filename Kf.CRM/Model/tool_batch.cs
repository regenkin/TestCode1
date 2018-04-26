using System;
namespace KfCrm.Model
{
	/// <summary>
	/// tool_batch:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class tool_batch
	{
		public tool_batch()
		{}
		#region Model
		private int _id;
		private string _batch_type;
		private int? _o_dep_id;
		private string _o_dep;
		private int? _o_emp_id;
		private string _o_emp;
		private int? _c_dep_id;
		private string _c_dep;
		private int? _c_emp_id;
		private string _c_emp;
		private int? _b_count;
		private int? _create_id;
		private string _create_name;
		private DateTime? _create_date;
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
		public string batch_type
		{
			set{ _batch_type=value;}
			get{return _batch_type;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? o_dep_id
		{
			set{ _o_dep_id=value;}
			get{return _o_dep_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string o_dep
		{
			set{ _o_dep=value;}
			get{return _o_dep;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? o_emp_id
		{
			set{ _o_emp_id=value;}
			get{return _o_emp_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string o_emp
		{
			set{ _o_emp=value;}
			get{return _o_emp;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? c_dep_id
		{
			set{ _c_dep_id=value;}
			get{return _c_dep_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string c_dep
		{
			set{ _c_dep=value;}
			get{return _c_dep;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? c_emp_id
		{
			set{ _c_emp_id=value;}
			get{return _c_emp_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string c_emp
		{
			set{ _c_emp=value;}
			get{return _c_emp;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? b_count
		{
			set{ _b_count=value;}
			get{return _b_count;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? create_id
		{
			set{ _create_id=value;}
			get{return _create_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string create_name
		{
			set{ _create_name=value;}
			get{return _create_name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? create_date
		{
			set{ _create_date=value;}
			get{return _create_date;}
		}
		#endregion Model

	}
}

