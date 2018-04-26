/*----------------------------------------------------------------
        // Copyright (C) 2016 Kinfar.
        // ��Ȩ����
        // �����ߣ�Kinfar.
        // Email��kinfar@foxmail.net
        // QQ��3133119519
//----------------------------------------------------------------*/

using Kinfar.Frame.Operate.Base.TempModel;
using System;
using System.Collections.Generic;

namespace Kinfar.Frame.Operate.Base.ConditionChange
{
    public interface ITransformProvider
    {
        bool Match(ConditionItem item, Type type);
        IEnumerable<ConditionItem> Transform(ConditionItem item, Type type);
    }
}