using System;
namespace AgileMetricsServer.Models
{
	public class HowManyDaysApiJson
	{
        public string? workItemType { get; set; }
        public DateTime? startingDate { get; set; }
        public DateTime? endingDate { get; set; }
        public int? simulations { get; set; }
        public int? storyCount { get; set; }
        public string? team { get; set; }
        public string? token { get; set; }
        public string? org { get; set; }
    }
}
