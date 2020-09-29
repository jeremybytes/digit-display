using System;

namespace digit_console_channel
{
    public static class Display
    {
        public static void OutputImages(int[] image1, int[] image2)
        {
            string output = string.Empty;
            for (int i = 0; i < 28; i++)
            {
                for (int j = 0; j < 28; j++)
                {
                    char outputChar;
                    outputChar = GetDisplayCharForPixel(image1[(i * 28) + j]);

                    output += $"{outputChar}{outputChar}";
                }
                if (image2 != null)
                {
                    output += "  |  ";
                    for (int j = 0; j < 28; j++)
                    {
                        char outputChar;
                        outputChar = GetDisplayCharForPixel(image2[(i * 28) + j]);

                        output += $"{outputChar}{outputChar}";
                    }
                }
                output += "\n";
            }
            Console.Write(output);
        }

        private static char GetDisplayCharForPixel(int i)
        {
            switch (i)
            {
                case var low when low > 16 && low < 32:
                    return '.';
                case var mid when mid >= 32 && mid < 64:
                    return ':';
                case var high when high >= 64 && high < 160:
                    return 'o';
                case var reallyHigh when reallyHigh >= 160 && reallyHigh < 224:
                    return 'O';
                case var reallyReallyHigh when reallyReallyHigh >= 224:
                    return '@';
                default:
                    return ' ';
            }
        }
    }
}
