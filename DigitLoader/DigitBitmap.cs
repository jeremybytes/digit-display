using System;
using System.Collections.Generic;
using System.Drawing;

namespace DigitLoader
{
    public class DigitBitmap
    {
        public static int[,] GenerateDigitArray(string input)
        {
            var rawData = input.Split(',');
            var integerData = new List<int>();
            foreach (string item in rawData)
                integerData.Add(Int32.Parse(item));

            var output = new int[28, 28];
            for (int i = 0; i < 28; i++)
                for (int j = 0; j < 28; j++)
                {
                    var pixelIndex = (i * 28) + j + 1;
                    output[i, j] = integerData[pixelIndex];
                }

            return output;
        }

        public static Bitmap GetBitmapFromDigitArray(int[,] digitArray)
        {
            var digitBitmap = new Bitmap(28, 28);

            for (int i = 0; i < 28; i++)
                for (int j = 0; j < 28; j++)
                {
                    var colorValue = 255 - digitArray[i, j];
                    digitBitmap.SetPixel(j, i,  
                        Color.FromArgb(colorValue, colorValue, colorValue));
                }

            //digitBitmap.RotateFlip(RotateFlipType.Rotate270FlipY);

            return digitBitmap;
        }

        public static Bitmap GetBitmapFromRawData(string input)
        {
            var rawData = input.Split(',');
            var integerData = new List<int>();
            foreach (string item in rawData)
                integerData.Add(Int32.Parse(item));

            var digitBitmap = new Bitmap(28, 28);

            for (int i = 0; i < 28; i++)
                for (int j = 0; j < 28; j++)
                {
                    var pixelIndex = (i * 28) + j + 1;
                    var colorValue = 255 - integerData[pixelIndex];
                    digitBitmap.SetPixel(j, i,
                        Color.FromArgb(colorValue, colorValue, colorValue));
                }
            return digitBitmap;
        }

    }
}
