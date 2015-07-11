using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace DigitLoader
{
    public class FileLoader
    {
        public static string[] LoadDataStrings(int threshold = 0)
        {
            string dataFile = ConfigurationManager.AppSettings["dataFile"];
            string fileName = AppDomain.CurrentDomain.BaseDirectory + dataFile;

            var output = new List<string>();

            if (File.Exists(fileName))
            {
                using (var sr = new StreamReader(fileName))
                {
                    string line;
                    sr.ReadLine(); // skip the first line

                    if (threshold > 0)
                    {
                        for (int i = 0; i < threshold; i++)
                            if ((line = sr.ReadLine()) != null)
                                output.Add(line);
                    }
                    else
                    {
                        while ((line = sr.ReadLine()) != null)
                            output.Add(line);
                    }

                }
            }
            return output.ToArray();
        }
    }
}
