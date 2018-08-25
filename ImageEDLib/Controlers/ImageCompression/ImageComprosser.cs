using System;
using System.Collections.Generic;
using System.Text;
using ImageEDLib.Controlers.ImageCompression.Base;
using ImageEDLib.Models.Huffman;
using ImageEDLib.Models.ImageContainers;

namespace ImageEDLib.Controlers.ImageCompression
{
    public class ImageComprosser : ICompressible
    {
        private long[,] _frequencies;

        private readonly HuffmanTree _redTree = new HuffmanTree();

        private readonly HuffmanTree _greenTree = new HuffmanTree();

        private readonly HuffmanTree _blueTree = new HuffmanTree();

        public object Compress(object source)
        {
            if (ValidateArguments(ref source))
            {
                RGBPixel[,] sourceImage = source as RGBPixel[,];
                _frequencies = new long[3, 260];
                InitFrequency(ref sourceImage);
                return FastCompressionWithPreEncryption(ref sourceImage, ref _frequencies);
            }
            else
                throw new ArgumentException("Compression source object must be RGBPixel[,]");
        }

        public object DeCompress(object source)
        {
            // TODO
            throw new NotImplementedException();
        }

        private object FastCompressionWithPreEncryption(ref RGBPixel[,] sourceImage, ref long[,] mFrequencies)
        {
            _frequencies = mFrequencies;
            InitHuffmanNodes();
            BuildTees();
            return GenerateCompressedObject(ref sourceImage);
        }

        private bool ValidateArguments(ref object source)
        {
            return source is RGBPixel[,];
        }

        private void InitFrequency(ref RGBPixel[,] sourceImage)
        {
            int imageWidth = (new ImageOperations.ImageOperations()).GetWidth(sourceImage),
                imageHeight = (new ImageOperations.ImageOperations()).GetHeight(sourceImage);
            for (int i = 0; i < imageHeight; i++)
            {
                for (int j = 0; j < imageWidth; j++)
                {
                    int redValue = sourceImage[i, j].Red;
                    int greenValue = sourceImage[i, j].Green;
                    int blueValue = sourceImage[i, j].Blue;

                    _frequencies[0, redValue]++;
                    _frequencies[1, greenValue]++;
                    _frequencies[2, blueValue]++;
                }
            }
        }

        private void InitHuffmanNodes()
        {
            for (int i = 0; i < 256; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (_frequencies[j, i] > 0)
                    {
                        if (j == 0) _redTree.AddNode(new HuffmanNode(i, _frequencies[j, i]));
                        else if (j == 1) _greenTree.AddNode(new HuffmanNode(i, _frequencies[j, i]));
                        else _blueTree.AddNode(new HuffmanNode(i, _frequencies[j, i]));
                    }
                }
            }
        }

        private void BuildTees()
        {
            _redTree.BuildTree();
            _greenTree.BuildTree();
            _blueTree.BuildTree();
        }

        private object GenerateCompressedObject(ref RGBPixel[,] sourceImage)
        {
            int imageWidth = (new ImageOperations.ImageOperations()).GetWidth(sourceImage),
                imageHeight = (new ImageOperations.ImageOperations()).GetHeight(sourceImage);
            StringBuilder imageCode = new StringBuilder();
            for (int i = 0; i < imageHeight; i++)
            {
                for (int j = 0; j < imageWidth; j++)
                {
                    imageCode.Append(_redTree.GetNodePath((int) sourceImage[i, j].Red));
                    imageCode.Append(_greenTree.GetNodePath((int) sourceImage[i, j].Green));
                    imageCode.Append(_blueTree.GetNodePath((int) sourceImage[i, j].Blue));
                }
            }

            _redTree.Dispose();
            _greenTree.Dispose();
            _blueTree.Dispose();
            string imageCodeString = imageCode.ToString();
            imageCode.Clear();
            return ConvertStrToByte(ref imageCodeString);
        }

        private byte[] ConvertStrToByte(ref string str)
        {
            var size = str.Length;
            size = (size / 8) + ((size % 8 != 0) ? 1 : 0);
            var list = new List<byte>();
            for (int i = 0; i < size; i++)
            {
                byte a = 0;
                for (int j = 0; j < 8; j++)
                {
                    int idx = i * 8 + j;
                    if (idx < str.Length && str[idx] == '1')
                    {
                        a |= 1;
                    }

                    a <<= 1;
                }

                list.Add(a);
            }

            return list.ToArray();
        }
    }
}