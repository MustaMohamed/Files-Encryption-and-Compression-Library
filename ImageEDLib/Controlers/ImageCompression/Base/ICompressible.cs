namespace ImageEDLib.Controlers.ImageCompression.Base
{
    public interface ICompressible
    {
        object Compress(object source);
        object DeCompress(object source);
    }
}