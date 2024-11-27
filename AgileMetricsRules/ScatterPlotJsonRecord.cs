namespace AgileMetricsRules
{
    public class ScatterPlotJsonRecord
    {
        public required List<ScatterPlotJsonRec> Value { get; set; }
        public bool NotAuthorized { get; set; } = false;
        public bool BadRequest { get; set; } = false;
    }

    public class ScatterPlotJsonRec
    {
        public int WorkItemId { get; set; }
        public required string CompletedDate { get; set; }
        public required string ActivatedDate { get; set; }
    }
}   