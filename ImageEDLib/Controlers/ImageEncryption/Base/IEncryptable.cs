
using System;
using ImageEDLib.Models.ImageContainers;

namespace ImageEDLib.Controlers.ImageEncryption.Base
{
    interface IEncryptable
    {
        object Encrypt(object key, object source);
        object Decrypt(object key, object source);
    }
}
