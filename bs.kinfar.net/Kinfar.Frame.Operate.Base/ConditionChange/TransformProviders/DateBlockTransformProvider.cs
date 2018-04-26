/*----------------------------------------------------------------
        // Copyright (C) 2016 Kinfar.
        // ��Ȩ����
        // �����ߣ�Kinfar.
        // Email��kinfar@foxmail.net
        // QQ��3133119519
//----------------------------------------------------------------*/

using Kinfar.Frame.Operate.Base.EnumDef;
using Kinfar.Frame.Operate.Base.TempModel;
using System;
using System.Collections.Generic;

namespace Kinfar.Frame.Operate.Base.ConditionChange.TransformProviders
{
    internal class DateBlockTransformProvider : ITransformProvider
    {
        public bool Match(ConditionItem item, Type type)
        {
            if (item == null)
                throw new ArgumentNullException("item");
            return item.Method == QueryMethod.DateBlock;
        }

        public IEnumerable<ConditionItem> Transform(ConditionItem item, Type type)
        {
            if (item == null)
                throw new ArgumentNullException("item");
            return new[]
                       {
                           new ConditionItem(item.Field, QueryMethod.GreaterThanOrEqual, item.Value),
                           new ConditionItem(item.Field, QueryMethod.LessThan, item.Value)
                       };
        }
    }
}