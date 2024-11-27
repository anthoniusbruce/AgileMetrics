namespace AgileMetricsRules
{
    public class FeatureChildPoint
    {
        public DateTime Date { get; set; }
        public int Count { get; set; }

        public override bool Equals(object? obj)
        {
            var point = obj as FeatureChildPoint;
            if (point == null)
                return false;
            return Date == point.Date && Count == point.Count;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Date, Count);
        }
    }
}