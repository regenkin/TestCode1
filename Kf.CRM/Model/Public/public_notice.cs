using System;
namespace KfCrm.Model
{
	/// <summary>
	/// public_notice:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class public_notice
	{
		public public_notice()
		{}
		#region Model
		private int _id;
		private string _notice_title;
		private string _notice_content;
		private int? _create_id;
		private string _create_name;
		private int? _dep_id;
		private string _dep_name;
		private DateTime? _notice_time;
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
		public string notice_title
		{
			set{ _notice_title=value;}
			get{return _notice_title;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string notice_content
		{
			set{ _notice_content=value;}
			get{return _notice_content;}
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
		public int? dep_id
		{
			set{ _dep_id=value;}
			get{return _dep_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string dep_name
		{
			set{ _dep_name=value;}
			get{return _dep_name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? notice_time
		{
			set{ _notice_time=value;}
			get{return _notice_time;}
		}
		#endregion Model

	}
}

