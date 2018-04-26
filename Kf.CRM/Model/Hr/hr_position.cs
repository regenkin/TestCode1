using System;
namespace KfCrm.Model
{
	/// <summary>
	/// hr_position:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class hr_position
	{
		public hr_position()
		{}
		#region Model
		private int _id;
		private string _position_name;
		private int? _position_order;
		private string _position_level;
		private int? _create_id;
		private DateTime? _create_date;
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
		public string position_name
		{
			set{ _position_name=value;}
			get{return _position_name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? position_order
		{
			set{ _position_order=value;}
			get{return _position_order;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string position_level
		{
			set{ _position_level=value;}
			get{return _position_level;}
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
		public DateTime? create_date
		{
			set{ _create_date=value;}
			get{return _create_date;}
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

