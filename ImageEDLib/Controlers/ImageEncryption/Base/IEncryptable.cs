namespace ImageEDLib.Controlers.ImageEncryption.Base
{
    public interface IEncryptable
    {
        object Encrypt(object key, object source);
        object Decrypt(object key, object source);
    }
}