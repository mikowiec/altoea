

using System;
using System.Collections.Generic;
using System.Reflection;
using Altoea.Framework.Models;
using Altoea.Framework.ViewPort.Descriptor;

namespace Altoea.Framework.Services.Configuration
{
    public interface IConfigData
    {
        Dictionary<string, BaseDescriptor> ViewPortDescriptors { get; }
        Dictionary<string, PropertyInfo> Properties { get; }
        
        Type TargetType { get; }
    }
}


