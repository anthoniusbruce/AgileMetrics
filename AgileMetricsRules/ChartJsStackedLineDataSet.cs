namespace AgileMetricsRules
{
	public class ChartJsStackedLineDataSet
	{
        public required ChartPoint[] data { get; set; }
        public required string label { get; set; }
        public required string borderColor { get; set; }
        public required string backgroundColor { get; set; }
        public bool fill { get; set; }
        public bool pointStyle { get; set; }
    }
}

