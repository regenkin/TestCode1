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
    ///商品价格
    /// </summary>
    public partial class article_goods
    {
        private readonly Model.sysconfig sysConfig = new BLL.sysconfig().loadConfig();//获得系统配置信息
        private readonly DAL.article_goods dal;

        public article_goods()
        {
            dal = new DAL.article_goods(sysConfig.sysdatabaseprefix);
        }

        #region 基本方法================================
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int channel_id, int article_id, int id)
        {
            return dal.Exists(channel_id, article_id, id);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Model.article_goods GetModel(int id)
        {
            return dal.GetModel(id);
        }
        #endregion

        #region 扩展方法================================
        /// <summary>
        /// 根据规格列表查询商品实体
        /// </summary>
        public Model.article_goods GetModel(int channel_id, int article_id, string spec_ids)
        {
            return dal.GetModel(channel_id, article_id, spec_ids);
        }
        #endregion
    }
}