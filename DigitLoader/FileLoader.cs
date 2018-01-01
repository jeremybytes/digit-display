using System;
using System.Configuration;
using ObservationLoader;

namespace DigitLoader
{
    public class FileLoader
    {
        public static Observation[] LoadObservations()
        {
            string dataFile = ConfigurationManager.AppSettings["dataFile"];
            int offset = int.Parse(ConfigurationManager.AppSettings["offset"]);
            int recordCount = int.Parse(ConfigurationManager.AppSettings["recordCount"]);
            string fileName = AppDomain.CurrentDomain.BaseDirectory + dataFile;

            return DataReader.ReadObservations(fileName, offset, recordCount);
        }
    }
}
