using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using LiveCharts;

namespace Sorting_Algorithm_Chart.Extensions
{
    public static class ExtensionMethods
    {
        public static T DeepClone<T>(this T a)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, a);
                stream.Position = 0;
                return (T)formatter.Deserialize(stream);
            }
        }

        public static ChartValues<int> GetRange(ChartValues<int> list, int start, int end)
        {
            if (start > end)
            {
                throw new ArgumentOutOfRangeException($"Start list index {start} is greater than end list index {end}");
            }
            ChartValues<int> shortenedlist = new ChartValues<int>();

            for (int i = start; i < end + 1; i++)
            {
                shortenedlist.Add(list[i]);
            }

            return shortenedlist;
        }
    }
}
