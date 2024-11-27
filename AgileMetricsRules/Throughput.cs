using System.Text.Json;

namespace AgileMetricsRules
{
    public class Throughput
    {
        public static List<ThroughputPoint> CalculateThroughputPoints(ThroughputJsonRecord throughputJson, DateTime startDate, DateTime endDate)
        {
            var ret = new List<ThroughputPoint>();

            if (throughputJson.Value == null)
                return ret;

            if (throughputJson.Value.Count == 0)
                return ret;

            foreach (var item in throughputJson.Value)
            {
                var date = Utility.DateSkToDate(item.CompletedDateSK);
                ret.Add(new ThroughputPoint { CompletedDate = date, Throughput = item.Throughput });
            }

            for (var d = startDate; d <= endDate; d = d.AddDays(1))
            {
                if (ret.Find(x => x.CompletedDate == d) == null)
                {
                    ret.Add(new ThroughputPoint { CompletedDate = d, Throughput = 0 });
                }
            }
            return ret;
        }

        public static ThroughputJsonRecord? ParseJsonStream(Stream stream)
        {
            var ret = JsonSerializer.Deserialize<ThroughputJsonRecord>(stream);
            return ret;
        }
    }
}

