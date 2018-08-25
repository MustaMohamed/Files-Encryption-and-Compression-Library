using System;
using System.Collections.Generic;
using System.Linq;
using ImageEDLib.Controlers.ImageEncryption.Base;
using ImageEDLib.Models.ImageContainers;

namespace ImageEDLib.Controlers.ImageEncryption
{
    public class ImageEncryptor : IEncryptable
    {
        public object Encrypt(object key, object source)
        {
            if (ValidateArguments(key, ref source))
            {
                RGBPixel[,] imageSource = source as RGBPixel[,];
                KeyValuePair<string, int> encryptionKey = (KeyValuePair<string, int>) key;
                long[,] a = new long[1, 1];
                return FastEncryptionWithCompression(ref encryptionKey, ref imageSource, ref a);
            }
            else
                throw new ArgumentException("Encryption key and source objects must be KeyValuePair and RGBPixel[,]");
        }

        public object Decrypt(object key, object source)
        {
            return Encrypt(key, source);
        }

        public object FastEncryptionWithCompression(ref KeyValuePair<string, int> key, ref RGBPixel[,] imageSource,
            ref long[,] mFrequencies)
        {
            int imageHeight = imageSource.GetLength(0),
                imageWidth = imageSource.GetLength(1);

            Int64 shiftKey = ConvertToInt(key.Key);
            int tapPosition = key.Value;
            // contains the length of {0 and 1} in the key to know the last bit index
            int lastPosition = key.Key.Length;

            RGBPixel[,] targetImage = new RGBPixel[imageHeight, imageWidth];
            for (int i = 0; i < imageHeight; i++)
            {
                for (int j = 0; j < imageWidth; j++)
                {
                    if (imageSource != null)
                    {
                        byte redValue = imageSource[i, j].Red;
                        byte greenValue = imageSource[i, j].Green;
                        byte blueValue = imageSource[i, j].Blue;

                        // get new shifted key
                        shiftKey = FastShiftKey(shiftKey, lastPosition, tapPosition);
                        // XOR the new value of key with the color values
                        redValue ^= (byte) (shiftKey & 0xff);

                        shiftKey = FastShiftKey(shiftKey, lastPosition, tapPosition);
                        greenValue ^= (byte) (shiftKey & 0xff);

                        shiftKey = FastShiftKey(shiftKey, lastPosition, tapPosition);
                        blueValue ^= (byte) (shiftKey & 0xff);

                        // set the new pixel colors
                        targetImage[i, j] = new RGBPixel(redValue, greenValue, blueValue);
                        if (mFrequencies.GetLength(0) > 1)
                        {
                            mFrequencies[0, redValue]++;
                            mFrequencies[1, greenValue]++;
                            mFrequencies[2, blueValue]++;
                        }
                    }
                }
            }

            return targetImage;
        }

        private static bool ValidateArguments(object key, ref object source)
        {
            return key is KeyValuePair<string, int> && source is RGBPixel[,];
        }

        private static Int64 FastShiftKey(Int64 shiftKey, int lastPosition, int tap)
        {
            for (int i = 0; i < 8; i++)
            {
                Int64 msBit = (Int64) ((shiftKey >> lastPosition) & 1);
                Int64 tapBit = (Int64) ((shiftKey >> tap) & 1);
                shiftKey <<= 1;
                shiftKey |= (msBit ^ tapBit);
            }

            return shiftKey;
        }

        private static Int64 ConvertToInt(String strByte)
        {
            Int64 number = 0;
            int sz = strByte.Length;
            for (int i = 0; i < sz; i++)
            {
                if (strByte.ElementAt(i) == '1')
                    number |= (1 << (sz - i - 1));
            }

            return number;
        }
    }
}