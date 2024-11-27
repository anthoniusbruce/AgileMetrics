namespace AgileMetricsRules
{
    public class ColumnJsonRecord
    {
        public required List<ColumnJsonRec> Value { get; set; }
        public bool NotAuthorized { get; set; }
        public bool BadRequest { get; set; }
    }

    public class ColumnJsonRec
    {
        public bool? IsDone { get; set; }
        public required string Done { get; set; }
        public required string ColumnId { get; set; }
        public required string ColumnName { get; set; }
        public int ColumnOrder { get; set; }
    }
}

