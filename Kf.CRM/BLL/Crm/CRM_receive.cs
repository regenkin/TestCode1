using System;
using System.Data;
using System.Collections.Generic;
using KfCrm.Common;
using KfCrm.Model;
namespace KfCrm.BLL
{
    /// <summary>
    /// CRM_receive
    /// </summary>
    public partial class CRM_receive
    {
        private readonly KfCrm.DAL.CRM_receive dal = new KfCrm.DAL.CRM_receive();
        public CRM_receive()
        { }
        #region  Method
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
        public int Add(KfCrm.Model.CRM_receive model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(KfCrm.Model.CRM_receive model)
        {
            return dal.Update(model);
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
        public KfCrm.Model.CRM_receive GetModel(int id)
        {

            return dal.GetModel(id);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中
        /// </summary>
        public KfCrm.Model.CRM_receive GetModelByCache(int id)
        {

            string CacheKey = "CRM_receiveModel-" + id;
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
            return (KfCrm.Model.CRM_receive)objModel;
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
        public List<KfCrm.Model.CRM_receive> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<KfCrm.Model.CRM_receive> DataTableToList(DataTable dt)
        {
            List<KfCrm.Model.CRM_receive> modelList = new List<KfCrm.Model.CRM_receive>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                KfCrm.Model.CRM_receive model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new KfCrm.Model.CRM_receive();
                    if (dt.Rows[n]["id"] != null && dt.Rows[n]["id"].ToString() != "")
                    {
                        model.id = int.Parse(dt.Rows[n]["id"].ToString());
                    }
                    if (dt.Rows[n]["Customer_id"] != null && dt.Rows[n]["Customer_id"].ToString() != "")
                    {
                        model.Customer_id = int.Parse(dt.Rows[n]["Customer_id"].ToString());
                    }
                    if (dt.Rows[n]["Customer_name"] != null && dt.Rows[n]["Customer_name"].ToString() != "")
                    {
                        model.Customer_name = dt.Rows[n]["Customer_name"].ToString();
                    }
                    if (dt.Rows[n]["Receive_num"] != null && dt.Rows[n]["Receive_num"].ToString() != "")
                    {
                        model.Receive_num = dt.Rows[n]["Receive_num"].ToString();
                    }
                    if (dt.Rows[n]["Pay_type_id"] != null && dt.Rows[n]["Pay_type_id"].ToString() != "")
                    {
                        model.Pay_type_id = int.Parse(dt.Rows[n]["Pay_type_id"].ToString());
                    }
                    if (dt.Rows[n]["Pay_type"] != null && dt.Rows[n]["Pay_type"].ToString() != "")
                    {
                        model.Pay_type = dt.Rows[n]["Pay_type"].ToString();
                    }
                    if (dt.Rows[n]["Receive_amount"] != null && dt.Rows[n]["Receive_amount"].ToString() != "")
                    {
                        model.Receive_amount = decimal.Parse(dt.Rows[n]["Receive_amount"].ToString());
                    }
                    if (dt.Rows[n]["Receive_date"] != null && dt.Rows[n]["Receive_date"].ToString() != "")
                    {
                        model.Receive_date = DateTime.Parse(dt.Rows[n]["Receive_date"].ToString());
                    }
                    if (dt.Rows[n]["C_depid"] != null && dt.Rows[n]["C_depid"].ToString() != "")
                    {
                        model.C_depid = int.Parse(dt.Rows[n]["C_depid"].ToString());
                    }
                    if (dt.Rows[n]["C_depname"] != null && dt.Rows[n]["C_depname"].ToString() != "")
                    {
                        model.C_depname = dt.Rows[n]["C_depname"].ToString();
                    }
                    if (dt.Rows[n]["C_empid"] != null && dt.Rows[n]["C_empid"].ToString() != "")
                    {
                        model.C_empid = int.Parse(dt.Rows[n]["C_empid"].ToString());
                    }
                    if (dt.Rows[n]["C_empname"] != null && dt.Rows[n]["C_empname"].ToString() != "")
                    {
                        model.C_empname = dt.Rows[n]["C_empname"].ToString();
                    }
                    if (dt.Rows[n]["create_id"] != null && dt.Rows[n]["create_id"].ToString() != "")
                    {
                        model.create_id = int.Parse(dt.Rows[n]["create_id"].ToString());
                    }
                    if (dt.Rows[n]["create_name"] != null && dt.Rows[n]["create_name"].ToString() != "")
                    {
                        model.create_name = dt.Rows[n]["create_name"].ToString();
                    }
                    if (dt.Rows[n]["create_date"] != null && dt.Rows[n]["create_date"].ToString() != "")
                    {
                        model.create_date = DateTime.Parse(dt.Rows[n]["create_date"].ToString());
                    }
                    if (dt.Rows[n]["companyid"] != null && dt.Rows[n]["companyid"].ToString() != "")
                    {
                        model.companyid = int.Parse(dt.Rows[n]["companyid"].ToString());
                    }
                    if (dt.Rows[n]["order_id"] != null && dt.Rows[n]["order_id"].ToString() != "")
                    {
                        model.order_id = int.Parse(dt.Rows[n]["order_id"].ToString());
                    }
                    if (dt.Rows[n]["remarks"] != null && dt.Rows[n]["remarks"].ToString() != "")
                    {
                        model.remarks = dt.Rows[n]["remarks"].ToString();
                    }
                    if (dt.Rows[n]["isDelete"] != null && dt.Rows[n]["isDelete"].ToString() != "")
                    {
                        model.isDelete = int.Parse(dt.Rows[n]["isDelete"].ToString());
                    }
                    if (dt.Rows[n]["Delete_time"] != null && dt.Rows[n]["Delete_time"].ToString() != "")
                    {
                        model.Delete_time = DateTime.Parse(dt.Rows[n]["Delete_time"].ToString());
                    }
                    if (dt.Rows[n]["receive_direction_id"] != null && dt.Rows[n]["receive_direction_id"].ToString() != "")
                    {
                        model.receive_direction_id = int.Parse(dt.Rows[n]["receive_direction_id"].ToString());
                    }
                    if (dt.Rows[n]["receive_direction_name"] != null && dt.Rows[n]["receive_direction_name"].ToString() != "")
                    {
                        model.receive_direction_name = dt.Rows[n]["receive_direction_name"].ToString();
                    }
                    if (dt.Rows[n]["receive_type_id"] != null && dt.Rows[n]["receive_type_id"].ToString() != "")
                    {
                        model.receive_type_id = int.Parse(dt.Rows[n]["receive_type_id"].ToString());
                    }
                    if (dt.Rows[n]["receive_type_name"] != null && dt.Rows[n]["receive_type_name"].ToString() != "")
                    {
                        model.receive_type_name = dt.Rows[n]["receive_type_name"].ToString();
                    }
                    if (dt.Rows[n]["receive_real"] != null && dt.Rows[n]["receive_real"].ToString() != "")
                    {
                        model.receive_real = decimal.Parse(dt.Rows[n]["receive_real"].ToString());
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

        #endregion  Method
    }
}

