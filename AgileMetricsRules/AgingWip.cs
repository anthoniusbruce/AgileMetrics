namespace AgileMetricsRules
{
    public class AgingWip
    {
        public static int CalculateAging(AgingWipJsonRec rec, DateTime today)
        {
            var activeDate = Utility.DateSkToDate(rec.ActivatedDateSK);
            var timeSpan = today - activeDate;
            return timeSpan.Days;
        }

        public static decimal CalculateColumnOrder(AgingWipJsonRec agingRec)
        {
            var ret = new Decimal(agingRec.ColumnOrder);
            if (agingRec.IsDone != null && agingRec.IsDone.Value)
            {
                ret += .5m;
            }

            return ret;
        }

        public static string GetColumnName(AgingWipJsonRec agingRec)
        {
            string ret = agingRec.ColumnName;

            if (agingRec.IsDone != null)
            {
                string suffix;
                if (agingRec.IsDone.Value)
                    suffix = " - Done";
                else
                    suffix = " - Doing";
                ret = ret.TrimEnd() + suffix;
            }

            return ret;
        }

        public static AgingWipResults GatherAllChartData(List<AgingWipJsonRec> agingWip)
        {
            var ret = new AgingWipResults();

            foreach (var item in agingWip)
            {
                var columnOrder = CalculateColumnOrder(item);
                if (!ret.ColumnInfo.ContainsKey(columnOrder))
                    ret.ColumnInfo[columnOrder] = GetColumnName(item);

                var result = new AgingWipResult
                {
                    Age = CalculateAging(item, DateTime.Today),
                    ColumnOrder = columnOrder,
                    WorkItemId = item.WorkItemId
                };
                ret.WorkItems.Add(result);
            }

            return ret;
        }
    }
}

