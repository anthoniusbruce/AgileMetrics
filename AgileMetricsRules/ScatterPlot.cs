using System.Text.Json;

namespace AgileMetricsRules
{
    public class ScatterPlot
    {
        public static ScatterPlotJsonRecord? ParseJsonStream(Stream stream)
        {
            var ret = JsonSerializer.Deserialize<ScatterPlotJsonRecord>(stream);
            return ret;
        }

        public static List<ScatterPlotPoint> CalculateCycleTimeScatterPlotPoints(ScatterPlotJsonRecord scatterPlotJson)
        {
            var ret = new List<ScatterPlotPoint>();

            if (scatterPlotJson.Value == null)
                return ret;

            foreach (var item in scatterPlotJson.Value)
            {
                DateTime activatedDate;
                DateTime completedDate;
                var activatedSuccess = DateTime.TryParse(item.ActivatedDate, out activatedDate);
                var completedSuccess = DateTime.TryParse(item.CompletedDate, out completedDate);
                string workItemId = item.WorkItemId.ToString();

                int cycleTime = 0;
                if (activatedSuccess && completedSuccess)
                    cycleTime = ((int)(completedDate.Date - activatedDate.Date).TotalDays);

                ret.Add(new ScatterPlotPoint { CompletedDate = completedDate.Date, CycleTime = cycleTime, WorkItemId = workItemId });
            }

            return ret;
        }

        public static Percentiles CalculateCycleTimePercentiles(List<ScatterPlotPoint> points)
        {
            var pointCount = points.Count();

            if (pointCount == 0)
                return new Percentiles();

            int thirtiethIndex = Math.Max((int)Math.Round(pointCount * 0.3M) - 1, 0);
            int fiftiethIndex = Math.Max((int)Math.Round(pointCount * 0.5M) - 1, 0);
            int seventiethIndex = Math.Max((int)Math.Round(pointCount * 0.7M) - 1, 0);
            int eightyFifthIndex = Math.Max((int)Math.Round(pointCount * 0.85M) - 1, 0);
            int ninetyFifthIndex = Math.Max((int)Math.Round(pointCount * 0.95M) - 1, 0);
            var orderedList = points.OrderBy(x => x.CycleTime).ToList();
            var percentiles = new Percentiles
            {
                Thirtieth = orderedList[thirtiethIndex].CycleTime,
                Fiftieth = orderedList[fiftiethIndex].CycleTime,
                Seventieth = orderedList[seventiethIndex].CycleTime,
                EightyFifth = orderedList[eightyFifthIndex].CycleTime,
                NinetyFifth = orderedList[ninetyFifthIndex].CycleTime
            };

            return percentiles;
        }

        public static Dictionary<DateTime, Percentiles> CalculateCycleTimePercentilesSeries(Dictionary<DateTime, ScatterPlotJsonRecord> scatterPlotJsonSeries)
        {
            var ret = new Dictionary<DateTime, Percentiles>();
            foreach (var item in scatterPlotJsonSeries.Keys)
            {
                var points = CalculateCycleTimeScatterPlotPoints(scatterPlotJsonSeries[item]);
                ret[item] = CalculateCycleTimePercentiles(points);
            }
            return ret;
        }
    }
}

