using System;
namespace AgileMetricsServer.Models
{
    public class HowManyStoriesApiJson
    {
        public string? workItemType { get; set; }
        public DateTime? startingDate { get; set; }
        public DateTime? endingDate { get; set; }
        public int? simulations { get; set; }
        public int? dayCount { get; set; }
        public string? team { get; set; }
        public string? token { get; set; }
    }
}

