using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace KfCrm.CRM.Data
{
    public class GetAuthorityByUid
    {
        public GetAuthorityByUid() { }

        public string GetAuthority(string uid,string RequestType)
        {
            switch (RequestType)
            { 
                case "Apps":
                    return GetApp(uid);
                case "Menus":
                    return GetMenus(uid);
                //case "CostMenus":
                //    return GetCost(uid);
                //case "Estate":
                //    return GetEstate(uid);
            }
            return "";
        }

        public string GetBtnAuthority(string uid, string btnid)
        {
            if (!string.IsNullOrEmpty(uid) && !string.IsNullOrEmpty(btnid))
            {
                int istrue = GetBtn(uid).IndexOf("b" + btnid + ",");
                if (istrue == -1)
                {
                    return "false";
                }
                else
                {
                    return "true";
                }
            }
            else
            {
                return "false";
            }
        }

        public string GetAppAuthority(string uid, string appid)
        {
            if (!string.IsNullOrEmpty(uid) && !string.IsNullOrEmpty(appid))
            {
                int istrue = GetApp(uid).IndexOf("a" + appid + ",");
                if (istrue == -1)
                {
                    return "false";
                }
                else
                {
                    return "true";
                }
            }
            else
            {
                return "false";
            }
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
                DataSet ds = rm.GetList("empID=" + int.Parse( uid));
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
        private string GetApp(string empID)
        {
            if (string.IsNullOrEmpty(empID))
            {
                return "(0)";
            }
            else
            {
                BLL.Sys_authority auth=new BLL.Sys_authority();
                string RoleIDs = GetRoleidByUID(empID);
                
                DataSet ds = auth.GetList("Menu_ids!='' and Role_ID in " + RoleIDs);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string Apps = "(";
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Apps += ds.Tables[0].Rows[i]["App_ids"] + ",";
                    }
                    Apps = Apps.Substring(0, Apps.Length - 1);
                    Apps += ")";
                    return Apps;
                }
                else
                {
                    return "(0)";
                }
            }
        }

        private string GetMenus(string empID)
        {
            if (string.IsNullOrEmpty(empID))
            {
                return "(0)";
            }
            else
            {
                BLL.Sys_authority auth = new BLL.Sys_authority();
                string RoleIDs = GetRoleidByUID(empID);
                
                DataSet ds = auth.GetList("Role_ID in " + RoleIDs);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string Menus = "(";
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Menus += ds.Tables[0].Rows[i]["Menu_ids"];
                    }
                    Menus = Menus.Substring(0, Menus.Length - 1);
                    Menus += ")";
                    return Menus.Replace("m", "");
                }
                else
                {
                    return "(0)";
                }
            }
        }

        private string GetBtn(string empID)
        {
            if (string.IsNullOrEmpty(empID))
            {
                return "(0)";
            }
            else
            {
                BLL.Sys_authority auth = new BLL.Sys_authority();
                string RoleIDs = GetRoleidByUID(empID);

                DataSet ds = auth.GetList("Role_ID in " + RoleIDs);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string Btns = "{";
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Btns += ds.Tables[0].Rows[i]["Button_ids"];
                        
                    }
                    Btns.Replace(",,,,,,", "");
                    //Btns = Btns.Substring(0, Btns.Length - 1);
                    Btns += "}";
                    //Btns = Btns.Replace("b", "");
                    return Btns;
                }
                else
                {
                    return "(0)";
                }
            }
        }

        

    }
}