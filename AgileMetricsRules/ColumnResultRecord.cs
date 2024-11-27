namespace AgileMetricsRules
{
	public class ColumnResultRecord
	{
		public string ColumnName { get; set; }
		public int ColumnOrder { get; set; }
        public string Doing { get; set; }
        public string Done { get;set; }

		public ColumnResultRecord(string columnName, int columnOrder, string doing, string done)
		{
			ColumnName = columnName;
			ColumnOrder = columnOrder;
			Doing = doing;
			Done = done;
		}
	}
}

