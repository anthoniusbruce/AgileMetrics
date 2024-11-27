namespace AgileMetricsRules
{
    public class ThroughputPoint
    {
        public DateTime CompletedDate { get; set; }
        public int Throughput { get; set; }

        public override bool Equals(object? obj)
        {
            var point = obj as ThroughputPoint;
            if (point == null)
                return false;
            return CompletedDate == point.CompletedDate && Throughput == point.Throughput;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CompletedDate, Throughput);
        }
    }
}