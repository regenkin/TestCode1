using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using DTcms.Common;

namespace DTcms.BLL
{
    /// <summary>
    ///请求回复内容
    /// </summary>
    public partial class weixin_request_content
    {
        private readonly Model.sysconfig sysConfig = new BLL.sysconfig().loadConfig();//获得系统配置信息
        private readonly DAL.weixin_request_content dal;

        public weixin_request_content()
        {
            dal = new DAL.weixin_request_content(sysConfig.sysdatabaseprefix);
        }

        #region 基本方法================================
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int rule_id)
        {
            return dal.Exists(rule_id);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Model.weixin_request_content GetModel(int rule_id)
        {
            return dal.GetModel(rule_id);
        }
        #endregion

        #region 扩展方法================================
        /// <summary>
        /// 获得规格下的数据列表
        /// </summary>
        public List<Model.weixin_request_content> GetModelList(int Top, int ruleId, string strWhere)
        {
            DataSet ds = dal.GetList(Top, ruleId, strWhere);
            return DataTableToList(ds.Tables[0]);
        }

        /// <summary>
        /// 组合数据列表
        /// </summary>
        public List<Model.weixin_request_content> DataTableToList(DataTable dt)
        {
            List<Model.weixin_request_content> modelList = new List<Model.weixin_request_content>();
            int rowsCount = dt.Rows.Count;
            if (dt.Rows.Count > 0)
            {
                Model.weixin_request_content model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = dal.DataRowToModel(dt.Rows[n]);
                    if (model != null)
                    {
                        modelList.Add(model);
                    }
                }
            }
            return modelList;
        }

        /// <summary>
        /// 返回规则的第一条标题
        /// </summary>
        public string GetTitle(int rule_id)
        {
            return dal.GetTitle(rule_id);
        }

        /// <summary>
        /// 返回规则的第一条内容
        /// </summary>
        public string GetContent(int rule_id)
        {
            return dal.GetContent(rule_id);
        }

        /// <summary>
        /// 返回规则下面的内容数量
        /// </summary>
        public int GetCount(int rule_id)
        {
            return dal.GetCount(rule_id);
        }
        #endregion
    }
}