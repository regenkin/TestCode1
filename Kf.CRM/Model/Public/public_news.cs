using System;
namespace KfCrm.Model
{
	/// <summary>
	/// public_news:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class public_news
	{
		public public_news()
		{}
		#region Model
		private int _id;
		private string _news_title;
		private string _news_content;
		private int? _create_id;
		private string _create_name;
		private int? _dep_id;
		private string _dep_name;
		private DateTime? _news_time;
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
		public string news_title
		{
			set{ _news_title=value;}
			get{return _news_title;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string news_content
		{
			set{ _news_content=value;}
			get{return _news_content;}
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
		public DateTime? news_time
		{
			set{ _news_time=value;}
			get{return _news_time;}
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

