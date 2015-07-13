using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace DigitLoader
{
    public class FileLoader
    {
        public static string[] LoadDataStrings(int threshold = 0)
        {
            string dataFile = ConfigurationManager.AppSettings["dataFile"];
            string fileName = AppDomain.CurrentDomain.BaseDirectory + dataFile;

            var data = File.ReadAllLines(fileName)
                .Skip(1).Take(threshold)
                .ToArray();

            return data;
        }
    }
}
