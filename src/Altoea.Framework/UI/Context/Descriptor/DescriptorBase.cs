

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Options;
using Altoea.Framework.Helpers; //LINQ, Extend
using Altoea.Framework.UI.Validator;
//using Altoea.Framework.Options;
//using Altoea.Framework.MultiLanguage;

namespace Altoea.Framework.UI.Context.Descriptor
{
    public abstract class DescriptorBase
    {
        public DescriptorBase(Type modelType, string prop)
        {
            this.ModelType = modelType;
            this.Name = prop;
            this.OrderIndex = 100;
            this.IsShowForEdit = true;
            this.IsShowForDisplay = true;
            //            
            Styles = new Dictionary<string, string>();
            Props  = new Dictionary<string, string>();
            Classes   = new List<string>();
            Validator = new List<ValidatorBase>();
            //
            SearchOperator = Query.Operator.Equal;
        }

        public Type ModelType { get; private set; }

        public HtmlElem.TagTypes TagType { get; set; }

        public List<string> Classes { get; set; }

        public Dictionary<string, string> Props { get; set; }

        public Dictionary<string, string> Styles { get; set; }

        public string Name { get; private set; }

        public int OrderIndex { get; set; }

        public Type DataType { get; set; }

        public List<ValidatorBase> Validator { get; set; }

        public bool IsReadOnly { get; set; }
        public bool IsRequired { get; set; }
        public bool IsShowForDisplay { get; set; }
        public bool IsShowForEdit { get; set; }
        public bool IsIgnore { get; set; }
        public bool IsHidden { get; set; }
        public bool IsHideSurroundingHtml { get; set; }
        public bool IsShowInGrid { get; set; }

        public QueryHelper.Operators SearchOperator { get; set; }







    }
}




