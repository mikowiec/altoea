

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq.Expression;
using Altoea.Framework.Models;
using Altoea.Framework.UI.Context;
using Altoea.Framework.UI.Context.Descriptor;
using Altoea.Framework.Helpers;  //Extend, Constant

namespace Altoea.Framework.UI.Attributes
{
    /// <summary>
    /// Attribute Data parameters
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AttrData<T> : IAttrData where T : class
    {
        public AttrData()
        {
            ContextDescriptors = new Dictionary<string, DescriptorBase>();

            TargetType = typeof(T);
            foreach (var item in TargetType.GetProperties())
            {
                TypeCode code = Type.GetTypeCode(item.PropertyType.GetTypeInfo().IsGenericType ? item.PropertyType.GetGenericArguments()[0] : item.PropertyType);
                switch (code)
                {
                    case TypeCode.Boolean:
                        AttrConfig(item.Name).AsCheckBox();
                        break;
                    case TypeCode.Char:
                        AttrConfig(item.Name).AsTextBox().MaxLength(1).RegEx(RegEx.Letters).Search(LINQ.Query.Operators.Contains);
                        break;
                    case TypeCode.DateTime:
                        AttrConfig(item.Name).AsTextBox().FormatAsDate().Search(LINQ.Query.Operators.Range);
                        break;
                    case TypeCode.UInt16:
                    case TypeCode.UInt32:
                    case TypeCode.UInt64:
                        AttrConfig(item.Name).AsTextBox().RegEx(RegEx.PositiveIntegersAndZero).Search(LINQ.Query.Operators.Range);
                        break;
                    case TypeCode.SByte:
                    case TypeCode.Int16:
                    case TypeCode.Int32:
                    case TypeCode.Int64:
                        AttrConfig(item.Name).AsTextBox().RegEx(RegEx.Integer).Search(LINQ.Query.Operators.Range);
                        break;
                    case TypeCode.Object:
                        AttrConfig(item.Name).AsHidden().Ignore();
                        break;
                    case TypeCode.Single:
                    case TypeCode.Double:
                    case TypeCode.Decimal:
                        AttrConfig(item.Name).AsTextBox().RegEx(RegEx.Float).Search(LINQ.Query.Operators.Range);
                        break;
                    case TypeCode.String:
                        AttrConfig(item.Name).AsTextBox().MaxLength(200).Search(LINQ.Query.Operators.Contains);
                        break;

                    case TypeCode.Byte:
                    case TypeCode.Empty:
                    default:
                        AttrConfig(item.Name).AsTextBox();
                        break;
                }

            }
            if (typeof(EditorEntity).IsAssignableFrom(TargetType))
            {
                AttrConfig("CreateBy").AsHidden();
                AttrConfig("CreatebyName").AsTextBox().Hide().ShowInGrid();
                AttrConfig("CreateDate").AsTextBox().Hide().FormatAsDateTime().ShowInGrid().Search(LINQ.Query.Operators.Range);

                AttrConfig("LastUpdateBy").AsHidden();
                AttrConfig("LastUpdateByName").AsTextBox().Hide().ShowInGrid();
                AttrConfig("LastUpdateDate").AsTextBox().Hide().FormatAsDateTime().ShowInGrid().Search(LINQ.Query.Operators.Range);
                AttrConfig("ActionType").AsHidden().AddClass("ActionType");
                AttrConfig("Title").AsTextBox().Order(1).ShowInGrid().Search(LINQ.Query.Operators.Contains).MaxLength(200);
                AttrConfig("Description").AsTextArea().Order(101).MaxLength(500);
                AttrConfig("Status").AsDropDownList().DataSource(DicKeys.RecordStatus, SourceType.Dictionary).ShowInGrid();

            }
            if (typeof(IImage).IsAssignableFrom(TargetType))
            {
                AttrConfig("ImageUrl").AsTextBox();
                AttrConfig("ImageThumbUrl").AsTextBox();
            }

            AttrConfig();

        }

        public Dictionary<string, DescriptorBase> ContextDescriptors { get; set; }

        public Type TargetType { get; private set; }

        public Dictionary<string, PropertyInfo> Properties
        {
            get
            {
                var props = new Dictionary<string, PropertyInfo>();
                TargetType.GetProperties().Each(m => props.Add(m.Name, m));
                return props;
            }
        }

        protected abstract void AttrConfig();

        protected TagsHelper AttrConfig(Expression<Func<T, object>> expression)
        {
            string key = Reflection.LinqExpression.GetPropertyName(expression.Body);
            return AttrConfigBy(key);
        }
       
        protected TagsHelper AttrConfig(string prop)
        {
            return new TagsHelper(prop, ContextDescriptors, TargetType, TargetType.GetProperty(prop));
        }      

    }
}




