using System;
using System.IO;
using System.Linq;

namespace CSharp
{
    public class DataReader
    {
        private static Observation ObservationFactory(string data)
        {
            string label;
            int[] pixels;
            var commaSeparated = data.Split(',');
            if (commaSeparated.Length == 785)
            {
                label = commaSeparated[0];
                pixels = commaSeparated
                            .Skip(1)
                            .Select(x => Convert.ToInt32(x))
                            .ToArray();
            }
            else
            {
                label = "T";
                pixels = commaSeparated
                            .Select(x => Convert.ToInt32(x))
                            .ToArray();
            }

            return new Observation(label, pixels);
        }

        public static Observation[] ReadObservations(string dataPath)
        {
            var data = File.ReadAllLines(dataPath)
                        .Skip(1)
                        .Select(ObservationFactory)
                        .ToArray();

            return data;
        }
    }
}
