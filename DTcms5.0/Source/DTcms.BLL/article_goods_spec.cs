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
    ///商品对应规格
    /// </summary>
    public partial class article_goods_spec
    {
        private readonly Model.sysconfig sysConfig = new BLL.sysconfig().loadConfig(); //获得系统配置信息
        private readonly DAL.article_goods_spec dal;

        public article_goods_spec()
        {
            dal = new DAL.article_goods_spec(sysConfig.sysdatabaseprefix);
        }

        #region 基本方法================================
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Model.article_goods_spec> GetList(int channel_id, int article_id, string strWhere)
        {
            return dal.GetList(channel_id, article_id, strWhere);
        }
        #endregion
    }
}