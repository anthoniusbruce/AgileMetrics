namespace AgileMetricsRules
{
	public class AgingWipResults
	{
		public List<AgingWipResult> WorkItems { get; } = new List<AgingWipResult>();
		public Dictionary<decimal, string> ColumnInfo { get; } = new Dictionary<decimal, string>();
	}

	public class AgingWipResult
	{
		public int WorkItemId { get; set; }
        public decimal ColumnOrder { get; set; }
        public int Age { get; set; }

        public override bool Equals(object? obj)
        {
            var other = obj as AgingWipResult;
            if (other == null)
                return false;
            return WorkItemId == other.WorkItemId && ColumnOrder == other.ColumnOrder && Age == other.Age;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Age, ColumnOrder, WorkItemId);
        }
    }

}

