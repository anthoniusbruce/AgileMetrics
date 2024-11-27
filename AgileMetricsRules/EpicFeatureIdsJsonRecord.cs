namespace AgileMetricsRules
{
    public class EpicFeatureIdsJsonRecord
    {
        public required List<EpicFeatureIdsJsonRec> Value { get; set; }
        public bool NotAuthorized { get; set; } = false;
        public bool BadRequest { get; set; } = false;
    }

    public class EpicFeatureIdsJsonRec
    {
        public int WorkItemId { get; set; }
    }
}