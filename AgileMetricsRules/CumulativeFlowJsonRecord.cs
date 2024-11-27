namespace AgileMetricsRules
{
	public class CumulativeFlowJsonRecord
	{
		public required List<CumulativeFlowJsonRec> Value { get; set; }
		public bool NotAuthorized { get; set; }
		public bool BadRequest { get; set; }
    }

	public class CumulativeFlowJsonRec
	{
		public int WorkItemId { get; set; }
		public int DateSK { get; set; }
		public required string ColumnId { get; set; }
		public bool? IsDone { get; set; }
	}
}

