using System;
namespace AgileMetricsServer.Models
{
	public class CycleTimeApiJson
	{
        public string? workItemType { get; set; }
        public DateTime? startingDate { get; set; }
        public DateTime? endingDate { get; set; }
        public string? team { get; set; }
        public string? token { get; set; }
        public string? org { get; set; }
        public string? tags { get; set; }
    }
}

