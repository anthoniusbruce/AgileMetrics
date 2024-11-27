namespace AgileMetricsRules
{
    public class ScatterPlotPoint
    {
        public DateTime CompletedDate { get; set; }
        public int CycleTime { get; set; }
        public required string WorkItemId { get; set; }

        public override bool Equals(object? obj)
        {
            var point = obj as ScatterPlotPoint;
            if (point == null)
                return false;
            return CompletedDate == point.CompletedDate && CycleTime == point.CycleTime && WorkItemId == point.WorkItemId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CompletedDate, CycleTime, WorkItemId);
        }
    }
}