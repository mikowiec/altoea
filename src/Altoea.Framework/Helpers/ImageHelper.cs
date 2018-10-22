

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Altoea.Framework.Helpers
{
    public class ImageHelper
    {
        public static bool IsImage(string extension)
        {
            switch(extension.ToLower())
            {
                case ".gif":
                case ".jpg":
                case ".png":
                case ".jpeg":
                case ".bmp": 
                    return true;
                default: return false;
            }
        }
    }
}





