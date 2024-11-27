using System;
namespace AgileMetricsServer.Models
{
	public class FeatureProgressApiJson
	{
        public string? featureIds { get; set; }
        public DateTime? startDate { get; set; }
        public string? reportingFrequency { get; set; }
        public string? token { get; set; }
        public string? org { get; set; }
    }
}

