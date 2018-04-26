using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KfCrm.Model
{
    /// <summary>
    /// 第三方登陆
    /// </summary>
   [Serializable]
    public class hr_employee_openid
    {
        public hr_employee_openid()
        { }
        #region Model
        public int ID{set;get;}
        public int hr_employeeID{set;get;}
        public int Type{set;get;}
        public int OpenID{set;get;}
        #endregion
    }
}
