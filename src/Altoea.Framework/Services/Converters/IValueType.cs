

using System;
using System.Collections.Generic;
using System.Text;

namespace Altoea.Framework.Services.Converters
{
    public interface IValueType
    {
        Type SupportType { get; }
        object Convert(string value);
    }
}


