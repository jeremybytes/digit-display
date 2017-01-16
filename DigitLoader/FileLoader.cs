using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace DigitLoader
{
    public class FileLoader
    {
        public static string[] LoadDataStrings(int threshold = int.MaxValue)
        {
            string dataFile = ConfigurationManager.AppSettings["dataFile"];
            int offset = Int32.Parse(ConfigurationManager.AppSettings["offset"]);
            string fileName = AppDomain.CurrentDomain.BaseDirectory + dataFile;

            var data = File.ReadLines(fileName)
                .Skip(1 + offset)
                .Take(threshold)
                .ToArray();

            return data;
        }
    }
}
