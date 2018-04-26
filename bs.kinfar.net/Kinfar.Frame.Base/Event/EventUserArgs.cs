/*----------------------------------------------------------------
        // Copyright (C) 2016 Kinfar.
        // 版权所有
        // 开发者：Kinfar.
        // Email：kinfar@foxmail.net
        // QQ：3133119519
//----------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kinfar.Frame.Base
{
    /// <summary>
    /// 用户登录事件参数
    /// </summary>
    public class EventUserArgs : EventArgs
    {
        private UserInfo _currUser;

        public UserInfo CurrUser
        {
            get { return _currUser; }
        }

        public EventUserArgs(UserInfo currUser)
        {
            this._currUser = currUser;
        }
    }
}
