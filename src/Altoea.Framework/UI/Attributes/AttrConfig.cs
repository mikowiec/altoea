

using System;
using System.Collections.Generic;
using System.Linq;

namespace Altoea.Framework.UI.Attributes
{
    public class AttrConfig
    {
        public IAttrData AttrData { get; private set; }

        public AttrConfig(IAttrData attrData)
        {
            AttrData = attrData;
        }

        public List<DescriptorBase>  GetContextDescriptor(string prop)
        {
            if(AttrData.ContextDescriptors.ContainsKey(prop))
            {
                return AttrData.ContextDescriptors[prop];
            }
            return null;
        }

        public DescriptorBase GetHtmlTag<T>(System.Linq.Expression.Expression<Func<T, object>> expression)
        {
            string prop = Reflection.LinqExpression.GetPropertyName(expression.Body);
            return GetContextDescriptor(prop);
        }

    }
}



