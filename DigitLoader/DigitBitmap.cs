using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace DigitLoader
{
    public class DigitBitmap
    {
        public static int[,] GenerateDigitArray(string input)
        {
            var rawData = input.Split(',');
            var integerData = rawData
                .Skip(1)
                .Select(x => Convert.ToInt32(x))
                .ToArray();

            var output = new int[28, 28];
            for (int i = 0; i < 28; i++)
                for (int j = 0; j < 28; j++)
                {
                    var pixelIndex = (i * 28) + j;
                    output[i, j] = integerData[pixelIndex];
                }

            return output;
        }

        public static Bitmap GetBitmapFromRawData(string input)
        {
            var rawData = input.Split(',');
            var integerData = rawData
                .Skip(1)
                .Select(x => Convert.ToInt32(x))
                .ToArray();

            var digitBitmap = new Bitmap(28, 28);

            for (int i = 0; i < 28; i++)
                for (int j = 0; j < 28; j++)
                {
                    var pixelIndex = (i * 28) + j;
                    var colorValue = 255 - integerData[pixelIndex];
                    digitBitmap.SetPixel(j, i,
                        Color.FromArgb(colorValue, colorValue, colorValue));
                }
            return digitBitmap;
        }

    }
}
