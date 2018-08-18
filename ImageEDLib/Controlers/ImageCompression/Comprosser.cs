using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageEDLib.Controlers.ImageCompression.Base;
using ImageEDLib.Models.ImageContainers;

namespace ImageEDLib.Controlers.ImageCompression
{
    class Comprosser : ICompressible
    {
        private bool CheckArgumentValidation(object source)
        {
            return source is RGBPixel[,];
        }

        public object Compress(object source)
        {
            if (CheckArgumentValidation(source))
                throw new NotImplementedException();
            throw new ArgumentException("Compression source object must be RGBPixel[,]");
        }

        public object DeCompress(object source)
        {
            throw new NotImplementedException();
        }
    }
}