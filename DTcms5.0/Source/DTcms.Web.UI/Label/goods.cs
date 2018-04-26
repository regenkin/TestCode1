using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DTcms.Common;

namespace DTcms.Web.UI
{
    public partial class BasePage : System.Web.UI.Page
    {
        /// <summary>
        /// 获得该商品的规格列表
        /// </summary>
        /// <param name="channel_id">频道ID</param>
        /// <param name="article_id">信息ID</param>
        /// <param name="strwhere">条件</param>
        /// <returns>List<T></returns>
        protected List<Model.article_goods_spec> get_article_goods_spec(int channel_id, int article_id, string strwhere)
        {
            return new BLL.article_goods_spec().GetList(channel_id, article_id, strwhere);
        }

        /// <summary>
        /// 根据栏目类别规格列表
        /// </summary>
        /// <param name="category_id">分类ID</param>
        /// <returns>DataTable</returns>
        protected DataTable get_article_category_spec(int category_id)
        {
            return new BLL.article_spec().GetCategorySpecList(category_id).Tables[0];
        }

        /// <summary>
        /// 根据父ID查询规格列表
        /// </summary>
        /// <param name="parent_id">父ID</param>
        /// <returns>DataTable</returns>
        protected DataTable get_article_spec_child(int parent_id)
        {
            return new BLL.article_spec().GetList(0, "parent_id=" + parent_id, "sort_id asc").Tables[0];
        }

        /// <summary>
        /// 返回动态规格URL参数
        /// </summary>
        /// <param name="dic">规格字典</param>
        /// <param name="item">要组合的参数</param>
        /// <returns>String</returns>
        protected string get_article_spec_param(Dictionary<string,string> dicSpecIds, string item)
        {
            bool isContains = false;
            string[] itemArr = item.Split('=');
            Dictionary<string, string> dic = new Dictionary<string, string>(dicSpecIds);
            if (itemArr.Length == 2)
            {
                if (dic.ContainsKey(itemArr[0]))
                {
                    isContains = true;
                    dic[itemArr[0]] = itemArr[1];
                }
            }
            string linkParam = string.Empty;
            foreach (KeyValuePair<string, string> kv in dic)
            {
                linkParam += "&" + kv.Key + "=" + kv.Value;
            }
            if (itemArr.Length == 2 && !isContains)
            {
                linkParam += "&" + itemArr[0] + "=" + itemArr[1];
            }
            return linkParam;
        }
    }
}