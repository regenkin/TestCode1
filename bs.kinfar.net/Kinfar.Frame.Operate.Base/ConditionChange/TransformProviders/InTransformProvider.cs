/*----------------------------------------------------------------
        // Copyright (C) 2016 Kinfar.
        // 版权所有
        // 开发者：Kinfar.
        // Email：kinfar@foxmail.net
        // QQ：3133119519
//----------------------------------------------------------------*/

using Kinfar.Frame.Operate.Base.EnumDef;
using Kinfar.Frame.Operate.Base.TempModel;
using System;
using System.Collections.Generic;

namespace Kinfar.Frame.Operate.Base.ConditionChange.TransformProviders
{
    internal class InTransformProvider : ITransformProvider
    {
        public bool Match(ConditionItem item, Type type)
        {
            if (item == null)
                throw new ArgumentNullException("item");
            return item.Method == QueryMethod.In;
        }

        public IEnumerable<ConditionItem> Transform(ConditionItem item, Type type)
        {
            if (item == null)
                throw new ArgumentNullException("item");
            var arr = (item.Value as Array);
            if (arr == null)
            {
                var arrStr = string.Empty;
                if (item.Value.GetType().ToString() == "System.Collections.Generic.List`1[System.Guid]")
                {
                    foreach (Guid id in item.Value as List<Guid>)
                    {
                        if (arrStr == string.Empty)
                        {
                            arrStr += id.ToString();
                        }
                        else
                        {
                            arrStr += "," + id.ToString();
                        }
                    }
                }
                else
                {
                    arrStr = item.Value.ToString();
                }
                if (!string.IsNullOrEmpty(arrStr))
                {
                    arr = arrStr.Split(',');
                }
            }
            return new[] { new ConditionItem(item.Field, QueryMethod.StdIn, arr) };
        }
    }
}