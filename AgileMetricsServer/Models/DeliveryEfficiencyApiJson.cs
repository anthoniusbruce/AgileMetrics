using System;
namespace AgileMetricsServer.Models
{
	public class DeliveryEfficiencyApiJson
	{
        public string? workItemType { get; set; }
		public int? cycleTimeSpan { get; set; }
		public DateTime? evaluationPeriodStart { get; set; }
		public DateTime? evaluationPeriodEnd { get; set; }
		public string? evaluationPeriodFrequency { get; set; }
		public string? team { get; set; }
		public string? token { get; set; }
		public string? org { get; set; }
		public string? tags { get; set; }
    }
}

