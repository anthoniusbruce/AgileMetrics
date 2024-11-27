namespace AgileMetricsRules
{
    public class AgingWipJsonRecord
    {
        public required List<AgingWipJsonRec> value { get; set; }
        public bool NotAuthorized { get; set; } = false;
        public bool BadRequest { get; set; } = false;
    }

    public class AgingWipJsonRec
    {
        public int ColumnOrder { get; set; }
        public bool? IsDone { get; set; }
        public int ActivatedDateSK { get; set; }
        public required string ColumnName { get; set; }
        public int WorkItemId { get; set; }
    }
}