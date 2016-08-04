using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace DigitLoader
{
    public class FileLoader
    {
        public static string[] LoadDataStrings(int threshold = int.MaxValue,
            int offset = 0)
        {
            string dataFile = ConfigurationManager.AppSettings["dataFile"];
            string fileName = AppDomain.CurrentDomain.BaseDirectory + dataFile;

            var data = File.ReadLines(fileName)
                .Skip(1 + offset)
                .Take(threshold)
                .ToArray();

            return data;
        }
    }
}
