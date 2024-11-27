using System;
using System.ComponentModel.DataAnnotations;

namespace AgileMetricsServer.Models
{
    public class EpicProgressDataModel
    {
        [Required(ErrorMessage = "Ado Organization field is required")]
        public string? AdoOrganization { get; set; }

        [Range(1, Int32.MaxValue)]
        [Required(ErrorMessage = "Epic ID field is required. ")]
        public int? EpicId { get; set; }

        [Required(ErrorMessage = "Start Date field is required. ")]
        public DateTime? StartDate { get; set; }

        [Required(ErrorMessage = "Reporting Frequency field is required", AllowEmptyStrings = false)]
        public string? ReportingFrequency { get; set; }
    }
}

