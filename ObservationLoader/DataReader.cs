using System.Globalization;
using System.IO;
using System.Linq;

namespace ObservationLoader
{
    public class DataReader
    {
        public static Observation[] ReadObservations(string dataPath, int offset = 0, int? count = null)
        {
            var data = File.ReadAllLines(dataPath).Skip(1 + offset);
            data = count.HasValue ? data.Take(count.Value) : data;
            return data.Select(ObservationFactory).ToArray();
        }

        private static Observation ObservationFactory(string data)
        {
            var commaSeparated = data.Split(',');
           
            return commaSeparated.Length == 785 
                ? new Observation(commaSeparated[0], IntStringsAsInts(commaSeparated, 1)) 
                : new Observation("T", IntStringsAsInts(commaSeparated, 0));
        }

        private static int[] IntStringsAsInts(string[] intStrings, int offset)
        {
            var result = new int[intStrings.Length - offset];
            for (var i = offset; i < result.Length; i++)
            {
                result[i] = int.Parse(intStrings[i], NumberStyles.Integer);
            }

            return result;
        }
    }
}