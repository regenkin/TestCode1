using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;

namespace KfCrm.CRM.Data
{
    public class GetDataAuth
    {
        public GetDataAuth() { }

        public string GetDataAuthByid(string optionid, string option, string empid)
        {
            string RoleIDs = GetRoleidByUID(empid);
            BLL.Sys_data_authority sda = new BLL.Sys_data_authority();
            DataSet ds = sda.GetList(" option_id=" + optionid + " and Role_id in " + RoleIDs);

            int temp = 0;
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (int.Parse(ds.Tables[0].Rows[i][option].ToString()) > temp)
                        temp = int.Parse(ds.Tables[0].Rows[i][option].ToString());
                }
                //return temp.ToString();
            }

            BLL.hr_employee emp = new BLL.hr_employee();
            DataSet ds1 = emp.GetList("id=" + empid);

            if (ds1.Tables[0].Rows[0]["uid"].ToString() == "admin")
                return "all";
            switch (temp)
            {
                case 0: return "none";
                case 1: return "my:" + empid;
                case 2: return "dep:" + ds1.Tables[0].Rows[0]["d_id"].ToString();
                case 3: return "depall:" + ds1.Tables[0].Rows[0]["d_id"].ToString();
                case 4: return "all";
            }
            return "";
        }

        private string GetRoleidByUID(string uid)
        {
            if (string.IsNullOrEmpty(uid))
            {
                return "(0)";
            }
            else
            {
                BLL.Sys_role_emp rm = new BLL.Sys_role_emp();
                DataSet ds = rm.GetList("empID=" + int.Parse(uid));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string RoleIDs = "(";
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        RoleIDs += ds.Tables[0].Rows[i]["RoleID"].ToString() + ",";
                    }
                    RoleIDs = RoleIDs.Substring(0, RoleIDs.Length - 1);
                    RoleIDs += ")";
                    return RoleIDs;
                }
                else
                {
                    return "(0)";
                }
            }
        }
    }
}