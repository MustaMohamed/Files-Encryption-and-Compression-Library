using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageEDLib.Controlers.ImageEncryption.Base;
using ImageEDLib.Models.ImageContainers;

namespace ImageEDLib.Controlers.ImageEncryption
{
    public class ImageEncryptor : IEncryptable
    {
        private bool CheckArgumentValidation(object key, object source)
        {
            return key is KeyValuePair<string, int> && source is RGBPixel[,];
        }

        public object Encrypt(object key, object source)
        {
            if (CheckArgumentValidation(key, source))
            {
                // TODO: Add Encryption Algorithm
                return new object();
            }
            throw new ArgumentException("Encryption key and source objects must be KeyValuePair and RGBPixel[,]");
        }

        public object Decrypt(object key, object source)
        {
            return Encrypt(key, source);
        }
    }
}