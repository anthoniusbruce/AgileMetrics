namespace AgileMetricsRules
{
    public class ScatterPlotSeriesJsonRecord
    {
        public required Dictionary<DateTime, ScatterPlotJsonRecord> Value { get; set; }
        public bool NotAuthorized { get; set; } = false;
    }
}