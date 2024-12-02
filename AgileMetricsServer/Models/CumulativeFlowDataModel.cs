using System;
using System.ComponentModel.DataAnnotations;

namespace AgileMetricsServer.Models
{
    public class CumulativeFlowDataModel
    {
        [Required(ErrorMessage = "Ado Team field is required. ")]
        public string? AdoTeam { get; set; }

        [Required(ErrorMessage = "How many months? field is required", AllowEmptyStrings = false)]
        public string? TimeSpan { get; set; }
    }
}

