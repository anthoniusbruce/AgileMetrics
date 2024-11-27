using System;
using System.ComponentModel.DataAnnotations;
using AgileMetricsRules;

namespace AgileMetricsServer.Models
{
    public class CycleTimeDataModel
    {
        [Required(ErrorMessage = "Ado Organization field is required")]
        public string? AdoOrganization { get; set; }

        [Required(ErrorMessage = "Ado Team field is required. ")]
        public string? AdoTeam { get; set; }

        [Required(ErrorMessage = "From Date field is required. ")]
        public DateTime? StartingDate { get; set; }

        [Required(ErrorMessage = "To Date field is required. ")]
        public DateTime? EndingDate { get; set; } = DateTime.Today.Date;

        [Required(ErrorMessage = "Work Item Type field is required", AllowEmptyStrings = false)]
        public string? WorkItemType { get; set; }

        public string? Tags { get; set; }
    }
}

