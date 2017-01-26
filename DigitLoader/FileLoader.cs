using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace DigitLoader
{
    public class FileLoader
    {
        public static string[] LoadDataStrings()
        {
            string dataFile = ConfigurationManager.AppSettings["dataFile"];
            int offset = int.Parse(ConfigurationManager.AppSettings["offset"]);
            int recordCount = int.Parse(ConfigurationManager.AppSettings["recordCount"]);
            string fileName = AppDomain.CurrentDomain.BaseDirectory + dataFile;

            var data = File.ReadLines(fileName)
                .Skip(1 + offset)
                .Take(recordCount)
                .ToArray();

            return data;
        }
    }
}
