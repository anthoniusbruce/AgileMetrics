namespace AgileMetricsRules
{
    public class CumulativeFlow
    {
        private static string[] ColorPalette =
        {
            "SteelBlue",
            "Coral",
            "Gold",
            "LightSeaGreen",
            "Tomato",
            "SlateBlue",
            "Olive",
            "LightPink",
            "LightSteelBlue",
            "RosyBrown",
            "MediumOrchid",
            "MediumSeaGreen",
            "OrangeRed",
            "PaleVioletRed",
            "MediumTurquoise",
            "DarkRed",
            "Wheat",
            "MediumVioletRed",
            "LightSteelBlue"
        };

        public static Dictionary<string, ColumnResultRecord> CreateColumnMap(ColumnJsonRecord columnData)
        {
            var ret = new Dictionary<string, ColumnResultRecord>();

            if (columnData == null || columnData.Value == null)
                return ret;

            foreach (var item in columnData.Value)
            {
                ColumnResultRecord record;
                if (ret.ContainsKey(item.ColumnId))
                    record = ret[item.ColumnId];
                else
                    record = new ColumnResultRecord(item.ColumnName, item.ColumnOrder, string.Empty, string.Empty);

                if (item.IsDone != null)
                {
                    if (item.IsDone.Value)
                        record.Done = item.Done;
                    else
                        record.Doing = item.Done;
                }

                ret[item.ColumnId] = record;
            }

            return ret;
        }

        public static Dictionary<string, List<CumulativeFlowResultRecord>> ArrangeCumulativeFlowData(CumulativeFlowJsonRecord jsonData, Dictionary<string, ColumnResultRecord> columnData)
        {
            var ret = new Dictionary<string, List<CumulativeFlowResultRecord>>();

            if (jsonData == null || jsonData.Value == null)
                return ret;

            foreach (var item in jsonData.Value)
            {
                var column = columnData[item.ColumnId];
                var rec = new CumulativeFlowResultRecord { WorkItemId = item.WorkItemId, Date = Utility.DateSkToDate(item.DateSK) };
                string columnTitle = BuildColumnTitle(item.IsDone, column);

                if (!ret.ContainsKey(columnTitle))
                    ret[columnTitle] = new List<CumulativeFlowResultRecord>();

                ret[columnTitle].Add(rec);
            }

            return ret;
        }

        public static Dictionary<DateTime, Dictionary<string, int>> AggregateCumulativeFlowData(Dictionary<string, List<CumulativeFlowResultRecord>> cumulativeFlowData)
        {
            var ret = new Dictionary<DateTime, Dictionary<string, int>>();

            foreach (var key in cumulativeFlowData.Keys)
            {
                foreach (var item in cumulativeFlowData[key])
                {
                    if (!ret.ContainsKey(item.Date))
                        ret[item.Date] = new Dictionary<string, int>();

                    if (!ret[item.Date].ContainsKey(key))
                        ret[item.Date][key] = 0;

                    ret[item.Date][key] = ret[item.Date][key] + 1;
                }
            }

            return ret;
        }

        public static List<string> ColumnOrder(Dictionary<string, ColumnResultRecord> columnData)
        {
            var ret = new List<string>();

            var orderedList = columnData.Values.OrderByDescending(val => val.ColumnOrder);

            foreach (var item in orderedList)
            {
                if (!string.IsNullOrWhiteSpace(item.Done))
                    ret.Add(BuildColumnTitle(true, item));
                if (!string.IsNullOrWhiteSpace(item.Doing))
                    ret.Add(BuildColumnTitle(false, item));
                ret.Add(BuildColumnTitle(null, item));
            }

            return ret;
        }

        public static ChartJsStackedLineDataSet[] PutItAllTogtherForAChart(Dictionary<DateTime, Dictionary<string, int>> aggregatedCfd, List<string> columnOrder)
        {
            var ret = new List<ChartJsStackedLineDataSet>();

            foreach (var item in columnOrder)
            {
                var index = columnOrder.IndexOf(item);
                index = index % ColorPalette.Length;
                var line = new ChartJsStackedLineDataSet
                {
                    label = item,
                    borderColor = ColorPalette[index],
                    backgroundColor = ColorPalette[index],
                    fill = true,
                    pointStyle = false,
                    data = Array.Empty<ChartPoint>()
                };
                var data = new List<ChartPoint>();
                foreach (var key in aggregatedCfd.Keys)
                {
                    if (!aggregatedCfd[key].ContainsKey(item))
                        continue;

                    var date = Utility.DateToJsonDate(key);
                    var point = new ChartPoint { x = date, y = aggregatedCfd[key][item] };
                    data.Add(point);
                }

                if (data.Count() > 0)
                {
                    line.data = data.OrderBy(item => item.x).ToArray();
                    ret.Add(line);
                }
            }

            return ret.ToArray();
        }

        public static void AddPercentagesToLabels(ChartJsStackedLineDataSet[] cfdData)
        {
            int total = 0;

            total = cfdData.Sum(line => line.label == "Closed" ? 0 : line.data.Sum(i => i.y));

            if (total == 0)
                return;

            foreach (var item in cfdData)
            {
                if (item.label == "Closed")
                    continue;

                var sum = item.data.Sum(i => i.y);
                var percent = (double)sum / total;
                item.label = string.Format("{0} {1:0.0%}", item.label, percent);
            }
        }

        private static string BuildColumnTitle(bool? isDone, ColumnResultRecord column)
        {
            var columnTitle = column.ColumnName;
            if (isDone != null)
            {
                string done;
                if (isDone.Value)
                    done = column.Done;
                else
                    done = column.Doing;

                columnTitle = string.Format("{0} - {1}", columnTitle, done);
            }

            return columnTitle;
        }
    }
}

