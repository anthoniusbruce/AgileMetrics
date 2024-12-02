using System;
using System.ComponentModel.DataAnnotations;

namespace AgileMetricsServer.Models
{
    public class FeatureProgressDataModel
    {
        [Required(ErrorMessage = "Feature IDs field is required. ")]
        public string? FeatureIds { get; set; }

        [Required(ErrorMessage = "Start Date field is required. ")]
        public DateTime? StartDate { get; set; }

        [Required(ErrorMessage = "Reporting Frequency field is required", AllowEmptyStrings = false)]
        public string? ReportingFrequency { get; set; }
    }
}

