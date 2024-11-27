namespace AgileMetricsRules
{
    public class ThroughputJsonRecord
    {
        public required List<ThroughputJsonRec> Value { get; set; }
        public bool NotAuthorized { get; set; } = false;
        public bool BadRequest { get; set; } = false;
    }

    public class ThroughputJsonRec
    {
        public int CompletedDateSK { get; set; }
        public int Throughput { get; set; }
    }
}