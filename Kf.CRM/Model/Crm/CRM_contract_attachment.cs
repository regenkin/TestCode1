using System;
namespace KfCrm.Model
{
    /// <summary>
    /// CRM_contract_attachment:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class CRM_contract_attachment
    {
        public CRM_contract_attachment()
        { }
        #region Model
        private int? _contract_id;
        private string _page_id;
        private string _file_id;
        private string _file_name;
        private string _real_name;
        private int? _file_size;
        private int? _create_id;
        private string _create_name;
        private DateTime? _create_date;
        /// <summary>
        /// 
        /// </summary>
        public int? contract_id
        {
            set { _contract_id = value; }
            get { return _contract_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string page_id
        {
            set { _page_id = value; }
            get { return _page_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string file_id
        {
            set { _file_id = value; }
            get { return _file_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string file_name
        {
            set { _file_name = value; }
            get { return _file_name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string real_name
        {
            set { _real_name = value; }
            get { return _real_name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? file_size
        {
            set { _file_size = value; }
            get { return _file_size; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? create_id
        {
            set { _create_id = value; }
            get { return _create_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string create_name
        {
            set { _create_name = value; }
            get { return _create_name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? create_date
        {
            set { _create_date = value; }
            get { return _create_date; }
        }
        #endregion Model

    }
}

