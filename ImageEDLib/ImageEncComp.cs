using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ImageEDLib.Controlers.ImageCompression.Base;
using ImageEDLib.Controlers.ImageEncryption.Base;
using ImageEDLib.Controlers.ImageOperations.Base;
using ImageEDLib.Models.ImageContainers;

namespace ImageEDLib
{
    public static class ImageEncComp
    {
        private static IImageHolder _imageHolder;
        private static IEncryptable _imageEncryptor;
        private static ICompressible _imageComprosser;

        public static bool Initialize(IImageHolder imageHolder, IEncryptable encryptable, ICompressible compressible)
        {
            try
            {
                _imageHolder = imageHolder;
                _imageEncryptor = encryptable;
                _imageComprosser = compressible;
                return true;
            }
            catch (Exception e)
            {
                throw new ArgumentException("Arguments can't match the correct types!");
            }
        }

        public static object OpenImage(string imagePath)
        {
            return _imageHolder.OpenImage(imagePath);
        }

        public static void DisplayImage(object sourceMatrix, PictureBox picBox)
        {
            _imageHolder.DisplayImage(sourceMatrix, picBox);
        }


        public static int GetWidth(object sourceMatrix)
        {
            return _imageHolder.GetWidth(sourceMatrix);
        }

        public static int GetHeight(object sourceMatrix)
        {
            return _imageHolder.GetHeight(sourceMatrix);
        }


        public static bool SaveImage(object sourceMatrix, string locatioPath)
        {
            return _imageHolder.SaveImage(sourceMatrix, locatioPath);
        }

        public static object Encrypt(object key, object source)
        {
            return _imageEncryptor.Encrypt(key, source);
        }

        public static object Decrypt(object key, object source)
        {
            return _imageEncryptor.Decrypt(key, source);
        }

        public static object Compress(object source)
        {
            return _imageComprosser.Compress(source);
        }

        public static object DeCompress(object source)
        {
            return _imageComprosser.DeCompress(source);
        }

        public static object FastCompressionWithPreEncryption(ref RGBPixel[,] sourceImage, ref long[,] mFrequencies)
        {
            // TODO
            throw new NotImplementedException();
        }

        public static object FastEncryptionWithCompression(ref KeyValuePair<string, int> key,
            ref RGBPixel[,] imageSource,
            ref long[,] mFrequencies)
        {
            // TODO
            throw new NotImplementedException();
        }
    }
}