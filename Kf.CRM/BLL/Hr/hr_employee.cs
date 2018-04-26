using System;
using System.Data;
using System.Collections.Generic;
using KfCrm.Common;
using KfCrm.Model;
namespace KfCrm.BLL
{
    /// <summary>
    /// hr_employee
    /// </summary>
    public partial class hr_employee
    {
        private readonly KfCrm.DAL.hr_employee dal = new KfCrm.DAL.hr_employee();
        public hr_employee()
        { }
        #region  Method
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int ID)
        {
            return dal.Exists(ID);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(KfCrm.Model.hr_employee model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public bool Update(KfCrm.Model.hr_employee model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// Ԥɾ��
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
        /// ɾ��һ������
        /// </summary>
        public bool Delete(int ID)
        {

            return dal.Delete(ID);
        }
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public bool DeleteList(string IDlist)
        {
            return dal.DeleteList(IDlist);
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public KfCrm.Model.hr_employee GetModel(int ID)
        {

            return dal.GetModel(ID);
        }

        /// <summary>
        /// �õ�һ������ʵ�壬�ӻ�����
        /// </summary>
        public KfCrm.Model.hr_employee GetModelByCache(int ID)
        {

            string CacheKey = "hr_employeeModel-" + ID;
            object objModel = KfCrm.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = dal.GetModel(ID);
                    if (objModel != null)
                    {
                        int ModelCache = KfCrm.Common.ConfigHelper.GetConfigInt("ModelCache");
                        KfCrm.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (KfCrm.Model.hr_employee)objModel;
        }

        /// <summary>
        /// ��������б�
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return dal.GetList(strWhere);
        }
        /// <summary>
        /// ���ǰ��������
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            return dal.GetList(Top, strWhere, filedOrder);
        }
        /// <summary>
        /// ��������б�
        /// </summary>
        public List<KfCrm.Model.hr_employee> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// ��������б�
        /// </summary>
        public List<KfCrm.Model.hr_employee> DataTableToList(DataTable dt)
        {
            List<KfCrm.Model.hr_employee> modelList = new List<KfCrm.Model.hr_employee>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                KfCrm.Model.hr_employee model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new KfCrm.Model.hr_employee();
                    if (dt.Rows[n]["ID"] != null && dt.Rows[n]["ID"].ToString() != "")
                    {
                        model.ID = int.Parse(dt.Rows[n]["ID"].ToString());
                    }
                    if (dt.Rows[n]["uid"] != null && dt.Rows[n]["uid"].ToString() != "")
                    {
                        model.uid = dt.Rows[n]["uid"].ToString();
                    }
                    if (dt.Rows[n]["pwd"] != null && dt.Rows[n]["pwd"].ToString() != "")
                    {
                        model.pwd = dt.Rows[n]["pwd"].ToString();
                    }
                    if (dt.Rows[n]["name"] != null && dt.Rows[n]["name"].ToString() != "")
                    {
                        model.name = dt.Rows[n]["name"].ToString();
                    }
                    if (dt.Rows[n]["idcard"] != null && dt.Rows[n]["idcard"].ToString() != "")
                    {
                        model.idcard = dt.Rows[n]["idcard"].ToString();
                    }
                    if (dt.Rows[n]["birthday"] != null && dt.Rows[n]["birthday"].ToString() != "")
                    {
                        model.birthday = dt.Rows[n]["birthday"].ToString();
                    }
                    if (dt.Rows[n]["d_id"] != null && dt.Rows[n]["d_id"].ToString() != "")
                    {
                        model.d_id = int.Parse(dt.Rows[n]["d_id"].ToString());
                    }
                    if (dt.Rows[n]["dname"] != null && dt.Rows[n]["dname"].ToString() != "")
                    {
                        model.dname = dt.Rows[n]["dname"].ToString();
                    }
                    if (dt.Rows[n]["postid"] != null && dt.Rows[n]["postid"].ToString() != "")
                    {
                        model.postid = int.Parse(dt.Rows[n]["postid"].ToString());
                    }
                    if (dt.Rows[n]["post"] != null && dt.Rows[n]["post"].ToString() != "")
                    {
                        model.post = dt.Rows[n]["post"].ToString();
                    }
                    if (dt.Rows[n]["email"] != null && dt.Rows[n]["email"].ToString() != "")
                    {
                        model.email = dt.Rows[n]["email"].ToString();
                    }
                    if (dt.Rows[n]["sex"] != null && dt.Rows[n]["sex"].ToString() != "")
                    {
                        model.sex = dt.Rows[n]["sex"].ToString();
                    }
                    if (dt.Rows[n]["tel"] != null && dt.Rows[n]["tel"].ToString() != "")
                    {
                        model.tel = dt.Rows[n]["tel"].ToString();
                    }
                    if (dt.Rows[n]["status"] != null && dt.Rows[n]["status"].ToString() != "")
                    {
                        model.status = dt.Rows[n]["status"].ToString();
                    }
                    if (dt.Rows[n]["zhiwuid"] != null && dt.Rows[n]["zhiwuid"].ToString() != "")
                    {
                        model.zhiwuid = int.Parse(dt.Rows[n]["zhiwuid"].ToString());
                    }
                    if (dt.Rows[n]["zhiwu"] != null && dt.Rows[n]["zhiwu"].ToString() != "")
                    {
                        model.zhiwu = dt.Rows[n]["zhiwu"].ToString();
                    }
                    if (dt.Rows[n]["sort"] != null && dt.Rows[n]["sort"].ToString() != "")
                    {
                        model.sort = int.Parse(dt.Rows[n]["sort"].ToString());
                    }
                    if (dt.Rows[n]["EntryDate"] != null && dt.Rows[n]["EntryDate"].ToString() != "")
                    {
                        model.EntryDate = dt.Rows[n]["EntryDate"].ToString();
                    }
                    if (dt.Rows[n]["address"] != null && dt.Rows[n]["address"].ToString() != "")
                    {
                        model.address = dt.Rows[n]["address"].ToString();
                    }
                    if (dt.Rows[n]["remarks"] != null && dt.Rows[n]["remarks"].ToString() != "")
                    {
                        model.remarks = dt.Rows[n]["remarks"].ToString();
                    }
                    if (dt.Rows[n]["education"] != null && dt.Rows[n]["education"].ToString() != "")
                    {
                        model.education = dt.Rows[n]["education"].ToString();
                    }
                    if (dt.Rows[n]["level"] != null && dt.Rows[n]["level"].ToString() != "")
                    {
                        model.level = dt.Rows[n]["level"].ToString();
                    }
                    if (dt.Rows[n]["professional"] != null && dt.Rows[n]["professional"].ToString() != "")
                    {
                        model.professional = dt.Rows[n]["professional"].ToString();
                    }
                    if (dt.Rows[n]["schools"] != null && dt.Rows[n]["schools"].ToString() != "")
                    {
                        model.schools = dt.Rows[n]["schools"].ToString();
                    }
                    if (dt.Rows[n]["title"] != null && dt.Rows[n]["title"].ToString() != "")
                    {
                        model.title = dt.Rows[n]["title"].ToString();
                    }
                    if (dt.Rows[n]["isDelete"] != null && dt.Rows[n]["isDelete"].ToString() != "")
                    {
                        model.isDelete = int.Parse(dt.Rows[n]["isDelete"].ToString());
                    }
                    if (dt.Rows[n]["Delete_time"] != null && dt.Rows[n]["Delete_time"].ToString() != "")
                    {
                        model.Delete_time = DateTime.Parse(dt.Rows[n]["Delete_time"].ToString());
                    }
                    if (dt.Rows[n]["portal"] != null && dt.Rows[n]["portal"].ToString() != "")
                    {
                        model.portal = dt.Rows[n]["portal"].ToString();
                    }
                    if (dt.Rows[n]["theme"] != null && dt.Rows[n]["theme"].ToString() != "")
                    {
                        model.theme = dt.Rows[n]["theme"].ToString();
                    }
                    if (dt.Rows[n]["canlogin"] != null && dt.Rows[n]["canlogin"].ToString() != "")
                    {
                        model.canlogin = int.Parse(dt.Rows[n]["canlogin"].ToString());
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        /// <summary>
        /// ��������б�
        /// </summary>
        public DataSet GetAllList()
        {
            return GetList("");
        }

        /// <summary>
        /// ��ҳ��ȡ�����б�
        /// </summary>
        public DataSet GetList(int PageSize, int PageIndex, string strWhere, string filedOrder, out string Total)
        {
            return dal.GetList(PageSize, PageIndex, strWhere, filedOrder, out Total);
        }
        /// <summary>
        /// ���¸�λ
        /// </summary>
        public bool UpdatePost(KfCrm.Model.hr_employee model)
        {
            return dal.UpdatePost(model);
        }
        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public DataSet GetPWD(int ID)
        {
            return dal.GetPWD(ID);
        }
        /// <summary>
        /// ��������
        /// </summary>
        public bool changepwd(KfCrm.Model.hr_employee model)
        {
            return dal.changepwd(model);
        }

        /// <summary>
        /// ��ȡȨ��
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public DataSet GetRole(int ID)
        {
            return dal.GetRole(ID);
        }

        /// <summary>
        /// ������Ϣ�޸�
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool PersonalUpdate(Model.hr_employee model)
        {
            return dal.PersonalUpdate(model);
        }

        /// <summary>
        /// ���¿ͻ�����������ͬ���տ��Ʊ ��Ա
        /// </summary>
        public bool UpdateCOCRI(KfCrm.Model.hr_employee model)
        {
            return dal.UpdateCOCRI(model);
        }
        #endregion  Method
    }
}

