using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using KfCrm.Common;

namespace KfCrm.CRM.Data
{
    public class C_Sys_log
    {
        public C_Sys_log() { }

        public void Add_log(int uid, string uname, string ip, string EventTitle, string EventType, int EventID, string Remind_name,string Original_txt, string Current_txt)
        {
            BLL.Sys_log log = new BLL.Sys_log();
            Model.Sys_log modellog = new Model.Sys_log();

            modellog.EventDate = DateTime.Now;
            modellog.UserID = uid;
            modellog.UserName = PageValidate.InputText(uname, 255);
            modellog.IPStreet = PageValidate.InputText(ip, 255);

            modellog.EventTitle = PageValidate.InputText(EventTitle, 255);

            modellog.EventType = PageValidate.InputText(EventType,255);
            modellog.EventID = EventID.ToString();
            modellog.Original_txt = "¡¾" + PageValidate.InputText(Remind_name, 255) + "¡¿" + PageValidate.InputText(Original_txt, int.MaxValue);
            modellog.Current_txt = "¡¾" + PageValidate.InputText(Remind_name, 255) + "¡¿" + PageValidate.InputText(Current_txt, int.MaxValue);

            log.Add(modellog);
        }
    }
}