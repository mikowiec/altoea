

using System;
using System.Collections.Generic;
using System.Reflection;
using Altoea.Framework.Models;
using Altoea.Framework.UI.Context.Descriptor;

namespace Altoea.Framework.UI.Attributes
{
    public interface IAttrData
    {
        Dictionary<string, DescriptorBase> ContextDescriptors { get; }
        Dictionary<string, PropertyInfo> Properties { get; }
        
        Type TargetType { get; }
    }
}


