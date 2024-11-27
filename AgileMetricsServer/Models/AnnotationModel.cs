using AgileMetricsRules;

namespace AgileMetricsServer.Models
{
	public class AnnotationModel
	{
		public string[] queryDetails { get; private set; }
		public string xPosition { get; private set; }
		public decimal yPosition { get; private set; }

        public AnnotationModel(SimulationsDataModel simulationDetails, SimulationResults simulationResults)
        {
            if (simulationResults == null || simulationResults.simulations == null || simulationResults.simulations.Count() == 0 ||
                simulationDetails == null || simulationDetails.WorkItemType == null || simulationDetails.StartingDate == null ||
                simulationDetails.EndingDate == null)
            {
                queryDetails = new string[] {};
                xPosition = string.Empty;
                return;
            }

            xPosition = simulationResults.simulations.Last().x;
            yPosition = simulationResults.simulations.Max(item => item.y);

            var team = string.Format("ADO team: {0}", simulationDetails.AdoTeam);
            var workItemType = simulationDetails.WorkItemType.Replace("+", " ").Replace("%27", "'");
            var period = string.Format("From: {0} To: {1}", simulationDetails.StartingDate.Value.ToString("d"), simulationDetails.EndingDate.Value.ToString("d"));
            var simDets = string.Format("Number of stories: {0}, Number of simulations: {1}", simulationDetails.Unit, simulationDetails.Simulations);
            queryDetails = new string[] { team, workItemType, period, simDets };
        }

        public AnnotationModel(CycleTimeDataModel cycleTimeDetails, IEnumerable<CycleTimeResults> cycleTimeResults)
        {
            if (cycleTimeDetails == null || cycleTimeResults == null || cycleTimeResults.Count() == 0 || cycleTimeDetails.WorkItemType == null ||
                cycleTimeDetails.StartingDate == null || cycleTimeDetails.EndingDate == null)
            {
                queryDetails = new string[] { };
                xPosition = string.Empty;
                return;
            }

            xPosition = cycleTimeResults.OrderBy(item => item.x).Last().x;
            yPosition = cycleTimeResults.Max(item => item.y);

            var team = string.Format("ADO team: {0}", cycleTimeDetails.AdoTeam);
            var workItemType = cycleTimeDetails.WorkItemType.Replace("+", " ").Replace("%27", "'");
            var period = string.Format("From: {0} To: {1}", cycleTimeDetails.StartingDate.Value.ToString("d"), cycleTimeDetails.EndingDate.Value.ToString("d"));
            string tags = string.Empty;
            if (!string.IsNullOrWhiteSpace(cycleTimeDetails.Tags))
                tags = string.Format("Excluding work items with tags: {0}", cycleTimeDetails.Tags);
            queryDetails = new string[] { team, workItemType, period, tags };
        }

        public AnnotationModel(DeliveryEfficiencyDataModel deliveryEfficiencyDetails, DeliveryEfficiencyResults ninetyFifthResults)
        {

            if (deliveryEfficiencyDetails == null || ninetyFifthResults == null || ninetyFifthResults.percentileData == null ||
                ninetyFifthResults.percentileData.Count() == 0 || deliveryEfficiencyDetails.WorkItemType == null ||
                deliveryEfficiencyDetails.EvaluationPeriodStart == null || deliveryEfficiencyDetails.EvaluationPeriodEnd == null)
            {
                queryDetails = new string[] { };
                xPosition = string.Empty;
                return;
            }

            xPosition = ninetyFifthResults.percentileData.Last().x;
            yPosition = ninetyFifthResults.percentileData.Max(item => item.y);

            var team = string.Format("ADO team: {0}", deliveryEfficiencyDetails.AdoTeam);
            var workItemType = deliveryEfficiencyDetails.WorkItemType.Replace("+", " ").Replace("%27", "'");
            var period = string.Format("From: {0} To: {1} {2}, cycle time: {3} days", deliveryEfficiencyDetails.EvaluationPeriodStart.Value.ToString("d"), deliveryEfficiencyDetails.EvaluationPeriodEnd.Value.ToString("d"), deliveryEfficiencyDetails.EvaluationPeriodFrequency, deliveryEfficiencyDetails.CycleTimeSpan);
            string tags = string.Empty;
            if (!string.IsNullOrWhiteSpace(deliveryEfficiencyDetails.Tags))
                tags = string.Format("Excluding work items with tags: {0}", deliveryEfficiencyDetails.Tags);
            queryDetails = new string[] { team, workItemType, period, tags };
        }
    }
}

