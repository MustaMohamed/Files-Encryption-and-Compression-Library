using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using ImageEDLib.Controlers.ImageOperations.Base;
using ImageEDLib.Models.ImageContainers;

namespace ImageEDLib.Controlers.ImageOperations
{
    public class ImageOperations : IImageHolder
    {
        public object OpenImage(string imagePath)
        {
            Bitmap originalBm = new Bitmap(imagePath);
            int height = originalBm.Height;
            int width = originalBm.Width;

            RGBPixel[,] buffer = new RGBPixel[height, width];

            unsafe
            {
                BitmapData bmd = originalBm.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite,
                    originalBm.PixelFormat);
                int x, y;
                int nWidth = 0;
                bool format32 = false;
                bool format24 = false;
                bool format8 = false;

                if (originalBm.PixelFormat == PixelFormat.Format24bppRgb)
                {
                    format24 = true;
                    nWidth = width * 3;
                }
                else if (originalBm.PixelFormat == PixelFormat.Format32bppArgb ||
                         originalBm.PixelFormat == PixelFormat.Format32bppRgb ||
                         originalBm.PixelFormat == PixelFormat.Format32bppPArgb)
                {
                    format32 = true;
                    nWidth = width * 4;
                }
                else if (originalBm.PixelFormat == PixelFormat.Format8bppIndexed)
                {
                    format8 = true;
                    nWidth = width;
                }

                int nOffset = bmd.Stride - nWidth;
                byte* p = (byte*) bmd.Scan0;
                for (y = 0; y < height; y++)
                {
                    for (x = 0; x < width; x++)
                    {
                        buffer[y, x] = new RGBPixel();
                        if (format8)
                        {
                            buffer[y, x].Red = buffer[y, x].Green = buffer[y, x].Blue = p[0];
                            p++;
                        }
                        else
                        {
                            buffer[y, x].Red = p[2];
                            buffer[y, x].Green = p[1];
                            buffer[y, x].Blue = p[0];
                            if (format24) p += 3;
                            else if (format32) p += 4;
                        }
                    }

                    p += nOffset;
                }

                originalBm.UnlockBits(bmd);
            }

            return buffer;
        }

        public void DisplayImage(object sourceMatrix, PictureBox picBox)
        {
            var imageMatrix = sourceMatrix as RGBPixel[,];
            if (imageMatrix == null)
                throw new ArgumentException("sourceMatrix object must RGBPixel[,]");

            int height = imageMatrix.GetLength(0);
            int width = imageMatrix.GetLength(1);

            Bitmap imageBmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            unsafe
            {
                BitmapData bmd = imageBmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite,
                    imageBmp.PixelFormat);
                int nWidth = 0;
                nWidth = width * 3;
                int nOffset = bmd.Stride - nWidth;
                byte* p = (byte*) bmd.Scan0;
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        p[2] = imageMatrix[i, j].Red;
                        p[1] = imageMatrix[i, j].Green;
                        p[0] = imageMatrix[i, j].Blue;
                        p += 3;
                    }

                    p += nOffset;
                }

                imageBmp.UnlockBits(bmd);
            }

            picBox.Image = imageBmp;
        }

        public int GetWidth(object sourceMatrix)
        {
            var imageMatrix = sourceMatrix as RGBPixel[,];
            if (imageMatrix == null)
                throw new ArgumentException("sourceMatrix object must RGBPixel[,]");
            return imageMatrix.GetLength(1);
        }

        public int GetHeight(object sourceMatrix)
        {
            var imageMatrix = sourceMatrix as RGBPixel[,];
            if (imageMatrix == null)
                throw new ArgumentException("sourceMatrix object must RGBPixel[,]");
            return imageMatrix.GetLength(0);
        }

        public bool SaveImage(object sourceMatrix, string locatioPath)
        {
            var imageMatrix = sourceMatrix as RGBPixel[,];
            if (imageMatrix == null)
                throw new ArgumentException("sourceMatrix object must RGBPixel[,]");
            // TODO
            throw new System.NotImplementedException();
        }
    }
}