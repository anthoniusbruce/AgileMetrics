namespace AgileMetricsRules
{
    public class FeatureChildrenJsonRecord
    {
        public required List<FeatureChildrenJsonRec> Value { get; set; }
        public bool NotAuthorized { get; set; } = false;
        public bool BadRequest { get; set; } = false;
    }

    public class FeatureChildrenJsonRec
    {
        public int DateSK { get; set; }
        public int WorkItemId { get; set; }
        public required string State { get; set; }
    }
}   