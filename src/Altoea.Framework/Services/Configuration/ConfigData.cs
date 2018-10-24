

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq.Expression;
using Altoea.Framework.Models;
using Altoea.Framework.ViewPort;
using Altoea.Framework.ViewPort.Descriptor;
using Altoea.Framework.Helpers;  //Extend, Constant

namespace Altoea.Framework.Services.Configuration
{
    /// <summary>
    /// Configuration Data parameters
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ConfigData<T> : IConfigData where T : class
    {
        public ConfigData()
        {
        }

        public Dictionary<string, BaseDescriptor> ViewPortDescriptors { get; set; }

        public Type TargetType { get; private set; }

        public Dictionary<st ring, PropertyInfo> Properties
        {
            get
            {
                var props = new Dictionary<string, PropertyInfo>();
                TargetType.GetProperties().Each(m => props.Add(m.Name, m));
                return props;
            }
        }

        protected abstract void GetConfigBy();

        protected TagsHelper GetConfig(Expression<Func<T, object>> expression)
        {
            string key = Reflection.LinqExpression.GetPropertyName(expression.Body);
            return ViewConfig(key);
        }
       
        protected TagsHelper ViewConfig(string prop)
        {
            return new TagsHelper(prop, ViewPortDescriptors, TargetType, TargetType.GetProperty(prop));
        }      

    }
}




