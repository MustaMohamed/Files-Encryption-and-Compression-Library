using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageEDLib.Controlers.ImageCompression.Base
{
    interface ICompressible
    {
        object Compress(object source);
        object DeCompress(object source);
    }
}
