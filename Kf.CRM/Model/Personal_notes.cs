using System;
namespace KfCrm.Model
{
	/// <summary>
	/// Personal_notes:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class Personal_notes
	{
		public Personal_notes()
		{}
		#region Model
		private int _id;
		private int? _emp_id;
		private string _emp_name;
		private string _note_content;
		private string _note_color;
		private string _xyz;
		private DateTime? _note_time;
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
		public int? emp_id
		{
			set{ _emp_id=value;}
			get{return _emp_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string emp_name
		{
			set{ _emp_name=value;}
			get{return _emp_name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string note_content
		{
			set{ _note_content=value;}
			get{return _note_content;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string note_color
		{
			set{ _note_color=value;}
			get{return _note_color;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string xyz
		{
			set{ _xyz=value;}
			get{return _xyz;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? note_time
		{
			set{ _note_time=value;}
			get{return _note_time;}
		}
		#endregion Model

	}
}

