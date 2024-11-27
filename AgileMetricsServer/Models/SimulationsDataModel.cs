using System.ComponentModel.DataAnnotations;

namespace AgileMetricsServer.Models
{
    public class SimulationsDataModel
    {
        [Required(ErrorMessage = "Ado Organization field is required")]
        public string? AdoOrganization { get; set; }

        [Required(ErrorMessage = "ADO Team field is required. ")]
        public string? AdoTeam { get; set; }

        [Required(ErrorMessage = "From Date field is required. ")]
        public DateTime? StartingDate { get; set; }

        [Required(ErrorMessage = "To Date field is required. ")]
        public DateTime? EndingDate { get; set; } = DateTime.Today.Date;

        [Required(ErrorMessage = "Unit field is required. ")]
        public int? Unit { get; set; }
            
        [Required(ErrorMessage = "Number of simulations field is required. ")]
        public int? Simulations { get; set; }

        [Required(ErrorMessage = "Work Item Type field is required", AllowEmptyStrings = false)]
        public string? WorkItemType { get; set; }
    }
}   

