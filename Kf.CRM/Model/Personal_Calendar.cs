using System;
namespace KfCrm.Model
{
	/// <summary>
	/// Personal_Calendar:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class Personal_Calendar
	{
		public Personal_Calendar()
		{}
		#region Model
		private int _id;
		private int? _emp_id;
		private string _emp_name;
		private int? _companyid;
		private string _subject;
		private string _location;
		private int? _masterid;
		private string _description;
		private int? _calendartype;
		private DateTime? _starttime;
		private DateTime? _endtime;
		private bool? _isalldayevent;
		private bool? _hasattachment;
		private string _category;
		private int? _instancetype;
		private string _attendees;
		private string _attendeenames;
		private string _otherattendee;
		private string _upaccount;
		private string _upname;
		private DateTime? _uptime;
		private string _recurringrule;
		/// <summary>
		/// 
		/// </summary>
		public int Id
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
		public int? companyid
		{
			set{ _companyid=value;}
			get{return _companyid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Subject
		{
			set{ _subject=value;}
			get{return _subject;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Location
		{
			set{ _location=value;}
			get{return _location;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? MasterId
		{
			set{ _masterid=value;}
			get{return _masterid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Description
		{
			set{ _description=value;}
			get{return _description;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? CalendarType
		{
			set{ _calendartype=value;}
			get{return _calendartype;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? StartTime
		{
			set{ _starttime=value;}
			get{return _starttime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? EndTime
		{
			set{ _endtime=value;}
			get{return _endtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool? IsAllDayEvent
		{
			set{ _isalldayevent=value;}
			get{return _isalldayevent;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool? HasAttachment
		{
			set{ _hasattachment=value;}
			get{return _hasattachment;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Category
		{
			set{ _category=value;}
			get{return _category;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? InstanceType
		{
			set{ _instancetype=value;}
			get{return _instancetype;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Attendees
		{
			set{ _attendees=value;}
			get{return _attendees;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string AttendeeNames
		{
			set{ _attendeenames=value;}
			get{return _attendeenames;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string OtherAttendee
		{
			set{ _otherattendee=value;}
			get{return _otherattendee;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string UPAccount
		{
			set{ _upaccount=value;}
			get{return _upaccount;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string UPName
		{
			set{ _upname=value;}
			get{return _upname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? UPTime
		{
			set{ _uptime=value;}
			get{return _uptime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string RecurringRule
		{
			set{ _recurringrule=value;}
			get{return _recurringrule;}
		}
		#endregion Model

	}
}

