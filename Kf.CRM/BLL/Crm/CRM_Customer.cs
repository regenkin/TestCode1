using System;
using System.Data;
using System.Collections.Generic;
using KfCrm.Common;
using KfCrm.Model;
namespace KfCrm.BLL
{
    /// <summary>
    /// CRM_Customer
    /// </summary>
    public partial class CRM_Customer
    {
        private readonly KfCrm.DAL.CRM_Customer dal = new KfCrm.DAL.CRM_Customer();
        public CRM_Customer()
        { }
        #region  Method

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return dal.GetMaxId();
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int id)
        {
            return dal.Exists(id);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(KfCrm.Model.CRM_Customer model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(KfCrm.Model.CRM_Customer model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 批量转客源
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Update_batch(KfCrm.Model.CRM_Customer model, string strWhere)
        {
            return dal.Update_batch(model, strWhere);
        }

        /// <summary>
        /// 预删除
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isDelete"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public bool AdvanceDelete(int id, int isDelete, string time)
        {
            return dal.AdvanceDelete(id, isDelete, time);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int id)
        {

            return dal.Delete(id);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteList(string idlist)
        {
            return dal.DeleteList(idlist);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public KfCrm.Model.CRM_Customer GetModel(int id)
        {

            return dal.GetModel(id);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中
        /// </summary>
        public KfCrm.Model.CRM_Customer GetModelByCache(int id)
        {

            string CacheKey = "CRM_CustomerModel-" + id;
            object objModel = KfCrm.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = dal.GetModel(id);
                    if (objModel != null)
                    {
                        int ModelCache = KfCrm.Common.ConfigHelper.GetConfigInt("ModelCache");
                        KfCrm.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (KfCrm.Model.CRM_Customer)objModel;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return dal.GetList(strWhere);
        }
        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            return dal.GetList(Top, strWhere, filedOrder);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<KfCrm.Model.CRM_Customer> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<KfCrm.Model.CRM_Customer> DataTableToList(DataTable dt)
        {
            List<KfCrm.Model.CRM_Customer> modelList = new List<KfCrm.Model.CRM_Customer>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                KfCrm.Model.CRM_Customer model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new KfCrm.Model.CRM_Customer();
                    if (dt.Rows[n]["id"] != null && dt.Rows[n]["id"].ToString() != "")
                    {
                        model.id = int.Parse(dt.Rows[n]["id"].ToString());
                    }
                    if (dt.Rows[n]["Serialnumber"] != null && dt.Rows[n]["Serialnumber"].ToString() != "")
                    {
                        model.Serialnumber = dt.Rows[n]["Serialnumber"].ToString();
                    }
                    if (dt.Rows[n]["Customer"] != null && dt.Rows[n]["Customer"].ToString() != "")
                    {
                        model.Customer = dt.Rows[n]["Customer"].ToString();
                    }
                    if (dt.Rows[n]["address"] != null && dt.Rows[n]["address"].ToString() != "")
                    {
                        model.address = dt.Rows[n]["address"].ToString();
                    }
                    if (dt.Rows[n]["tel"] != null && dt.Rows[n]["tel"].ToString() != "")
                    {
                        model.tel = dt.Rows[n]["tel"].ToString();
                    }
                    if (dt.Rows[n]["fax"] != null && dt.Rows[n]["fax"].ToString() != "")
                    {
                        model.fax = dt.Rows[n]["fax"].ToString();
                    }
                    if (dt.Rows[n]["site"] != null && dt.Rows[n]["site"].ToString() != "")
                    {
                        model.site = dt.Rows[n]["site"].ToString();
                    }
                    if (dt.Rows[n]["industry"] != null && dt.Rows[n]["industry"].ToString() != "")
                    {
                        model.industry = dt.Rows[n]["industry"].ToString();
                    }
                    if (dt.Rows[n]["Provinces_id"] != null && dt.Rows[n]["Provinces_id"].ToString() != "")
                    {
                        model.Provinces_id = int.Parse(dt.Rows[n]["Provinces_id"].ToString());
                    }
                    if (dt.Rows[n]["Provinces"] != null && dt.Rows[n]["Provinces"].ToString() != "")
                    {
                        model.Provinces = dt.Rows[n]["Provinces"].ToString();
                    }
                    if (dt.Rows[n]["City_id"] != null && dt.Rows[n]["City_id"].ToString() != "")
                    {
                        model.City_id = int.Parse(dt.Rows[n]["City_id"].ToString());
                    }
                    if (dt.Rows[n]["City"] != null && dt.Rows[n]["City"].ToString() != "")
                    {
                        model.City = dt.Rows[n]["City"].ToString();
                    }
                    if (dt.Rows[n]["CustomerType_id"] != null && dt.Rows[n]["CustomerType_id"].ToString() != "")
                    {
                        model.CustomerType_id = int.Parse(dt.Rows[n]["CustomerType_id"].ToString());
                    }
                    if (dt.Rows[n]["CustomerType"] != null && dt.Rows[n]["CustomerType"].ToString() != "")
                    {
                        model.CustomerType = dt.Rows[n]["CustomerType"].ToString();
                    }
                    if (dt.Rows[n]["CustomerLevel_id"] != null && dt.Rows[n]["CustomerLevel_id"].ToString() != "")
                    {
                        model.CustomerLevel_id = int.Parse(dt.Rows[n]["CustomerLevel_id"].ToString());
                    }
                    if (dt.Rows[n]["CustomerLevel"] != null && dt.Rows[n]["CustomerLevel"].ToString() != "")
                    {
                        model.CustomerLevel = dt.Rows[n]["CustomerLevel"].ToString();
                    }
                    if (dt.Rows[n]["CustomerSource_id"] != null && dt.Rows[n]["CustomerSource_id"].ToString() != "")
                    {
                        model.CustomerSource_id = int.Parse(dt.Rows[n]["CustomerSource_id"].ToString());
                    }
                    if (dt.Rows[n]["CustomerSource"] != null && dt.Rows[n]["CustomerSource"].ToString() != "")
                    {
                        model.CustomerSource = dt.Rows[n]["CustomerSource"].ToString();
                    }
                    if (dt.Rows[n]["DesCripe"] != null && dt.Rows[n]["DesCripe"].ToString() != "")
                    {
                        model.DesCripe = dt.Rows[n]["DesCripe"].ToString();
                    }
                    if (dt.Rows[n]["Remarks"] != null && dt.Rows[n]["Remarks"].ToString() != "")
                    {
                        model.Remarks = dt.Rows[n]["Remarks"].ToString();
                    }
                    if (dt.Rows[n]["Department_id"] != null && dt.Rows[n]["Department_id"].ToString() != "")
                    {
                        model.Department_id = int.Parse(dt.Rows[n]["Department_id"].ToString());
                    }
                    if (dt.Rows[n]["Department"] != null && dt.Rows[n]["Department"].ToString() != "")
                    {
                        model.Department = dt.Rows[n]["Department"].ToString();
                    }
                    if (dt.Rows[n]["Employee_id"] != null && dt.Rows[n]["Employee_id"].ToString() != "")
                    {
                        model.Employee_id = int.Parse(dt.Rows[n]["Employee_id"].ToString());
                    }
                    if (dt.Rows[n]["Employee"] != null && dt.Rows[n]["Employee"].ToString() != "")
                    {
                        model.Employee = dt.Rows[n]["Employee"].ToString();
                    }
                    if (dt.Rows[n]["privatecustomer"] != null && dt.Rows[n]["privatecustomer"].ToString() != "")
                    {
                        model.privatecustomer = dt.Rows[n]["privatecustomer"].ToString();
                    }
                    if (dt.Rows[n]["lastfollow"] != null && dt.Rows[n]["lastfollow"].ToString() != "")
                    {
                        model.lastfollow = DateTime.Parse(dt.Rows[n]["lastfollow"].ToString());
                    }
                    if (dt.Rows[n]["Create_id"] != null && dt.Rows[n]["Create_id"].ToString() != "")
                    {
                        model.Create_id = int.Parse(dt.Rows[n]["Create_id"].ToString());
                    }
                    if (dt.Rows[n]["Create_name"] != null && dt.Rows[n]["Create_name"].ToString() != "")
                    {
                        model.Create_name = dt.Rows[n]["Create_name"].ToString();
                    }
                    if (dt.Rows[n]["Create_date"] != null && dt.Rows[n]["Create_date"].ToString() != "")
                    {
                        model.Create_date = DateTime.Parse(dt.Rows[n]["Create_date"].ToString());
                    }
                    if (dt.Rows[n]["isDelete"] != null && dt.Rows[n]["isDelete"].ToString() != "")
                    {
                        model.isDelete = int.Parse(dt.Rows[n]["isDelete"].ToString());
                    }
                    if (dt.Rows[n]["Delete_time"] != null && dt.Rows[n]["Delete_time"].ToString() != "")
                    {
                        model.Delete_time = DateTime.Parse(dt.Rows[n]["Delete_time"].ToString());
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetAllList()
        {
            return GetList("");
        }

        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetList(int PageSize, int PageIndex, string strWhere, string filedOrder, out string Total)
        {
            return dal.GetList(PageSize, PageIndex, strWhere, filedOrder, out Total);
        }

        /// <summary>
        /// 更新最后跟进
        /// </summary>
        public bool UpdateLastFollow(string id)
        {
            return dal.UpdateLastFollow(id);
        }
        public DataSet Reports_year(string items, int year, string where)
        {
            return dal.Reports_year(items, year, where);
        }

        /// <summary>
        /// 同比环比【客户新增】
        /// </summary>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <param name="project_id"></param>
        /// <returns></returns>
        public DataSet Compared(string year1, string month1, string year2, string month2)
        {
            return dal.Compared(year1, month1, year2, month2);
        }

        public DataSet Compared_type(string year1, string month1, string year2, string month2)
        {
            return dal.Compared_type(year1, month1, year2, month2);
        }

        public DataSet Compared_level(string year1, string month1, string year2, string month2)
        {
            return dal.Compared_level(year1, month1, year2, month2);
        }

        public DataSet Compared_source(string year1, string month1, string year2, string month2)
        {
            return dal.Compared_source(year1, month1, year2, month2);
        }

        public DataSet Compared_empcusadd(string year1, string month1, string year2, string month2 , string idlist)//, string idlist)
        {
            return dal.Compared_empcusadd(year1, month1, year2, month2, idlist);//, idlist);
        }

        /// <summary>
        /// 客户新增统计
        /// </summary>
        /// <param name="year"></param>
        /// <param name="idlist"></param>
        /// <returns></returns>
        public DataSet report_empcus(int year, string idlist)
        {
            return dal.report_empcus(year, idlist);
        }

        /// <summary>
        /// ToExcel
        /// </summary>
        public DataSet ToExcel(string strWhere)
        {
            return dal.ToExcel(strWhere);
        }
        /// <summary>
        /// 导入
        /// </summary>
        public bool ToImport(int emp_id,string emp_name,DateTime create_time)
        {
            return dal.ToImport(emp_id, emp_name, create_time);
        }

        /// <summary>
        /// 统计漏斗
        /// </summary>
        public DataSet Funnel(string strWhere, string year)
        {
            return dal.Funnel(strWhere, year);
        }
        #endregion  Method
    }
}

