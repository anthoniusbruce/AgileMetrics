using System;
using System.ComponentModel.DataAnnotations;
using AgileMetricsRules;

namespace AgileMetricsServer.Models
{
    public class DeliveryEfficiencyDataModel
    {
        [Required(ErrorMessage = "Ado Team field is required. ")]
        public string? AdoTeam { get; set; }

        [Required(ErrorMessage = "Work Item Type field is required", AllowEmptyStrings = false)]
        public string? WorkItemType { get; set; }

        [Range(1, Int32.MaxValue)]
        [Required(ErrorMessage = "Cycle Time Span field is required. ")]
        public int? CycleTimeSpan { get; set; }

        [Required(ErrorMessage = "Evaluation Period Start field is required. ")]
        public DateTime? EvaluationPeriodStart { get; set; }

        [Required(ErrorMessage = "Evaluation Period End field is required. ")]
        public DateTime? EvaluationPeriodEnd { get; set; } = DateTime.Today.Date;

        [Required(ErrorMessage = "Evaluation Period Frequency field is required", AllowEmptyStrings = false)]
        public string? EvaluationPeriodFrequency { get; set; }

        public string? Tags { get; set; }
    }
}

