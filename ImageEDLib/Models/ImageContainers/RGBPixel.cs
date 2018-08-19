﻿
namespace ImageEDLib.Models.ImageContainers
{
    public class RGBPixel
    {
        public RGBPixel(byte red, byte green, byte blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }

        public RGBPixel()
        {
        }

        public byte Red { get; set; }
        public byte Green { get; set; }
        public byte Blue { get; set; }
    }
}
