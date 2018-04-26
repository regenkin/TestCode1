using System;
namespace KfCrm.Model
{
	/// <summary>
	/// Param_City:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class Param_City
	{
		public Param_City()
		{}
		#region Model
		private int _id;
		private int? _parentid;
		private string _city;
        private int? _City_order;
		private int? _create_id;
		private DateTime? _create_date;
		private int? _update_id;
		private DateTime? _update_date;
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
		public int? parentid
		{
			set{ _parentid=value;}
			get{return _parentid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string City
		{
			set{ _city=value;}
			get{return _city;}
		}

        /// <summary>
        /// 
        /// </summary>
        public int? City_order
        {
            set { _City_order = value; }
            get { return _City_order; }
        }

		/// <summary>
		/// 
		/// </summary>
		public int? Create_id
		{
			set{ _create_id=value;}
			get{return _create_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? Create_date
		{
			set{ _create_date=value;}
			get{return _create_date;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Update_id
		{
			set{ _update_id=value;}
			get{return _update_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? Update_date
		{
			set{ _update_date=value;}
			get{return _update_date;}
		}
		#endregion Model

	}
}

