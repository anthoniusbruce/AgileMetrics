using AgileMetricsRules;

namespace AdoAnalysisServerApis
{
    public interface IAdoAnalysisService
    {
        Task<EpicFeatureIdsJsonRecord?> GetEpicFeatureIdsData(int epicId, string token, string organization);
        Task<HttpResponseMessage> GetMetadata(string token, string organization);
        Task<FeatureChildrenJsonRecord?> GetFeatureProgressData(string featureIds, DateTime startDate, string reportingFrequency, string token, string organization);
        Task<ScatterPlotJsonRecord?> GetCycleTimeScatterPlotData(string workItemType, DateTime startDate, DateTime endDate, string tags, string areaPath, string token, string organization);
        Task<AgingWipJsonRecord?> GetAgingWorkInProgressData(string workItemType, string tags, string adoTeam, string token, string organization);
        Task<ScatterPlotSeriesJsonRecord?> GetCycleTimeSeriesScatterPlotData(string workItemType, int cycleTimeSpan, DateTime evaluationPeriodStart, DateTime evaluationPeriodEnd, string evaluationPeriodFrequency, string tags, string adoTeam, string token, string organization);
        Task<ThroughputJsonRecord?> GetThroughputData(string workItemType, DateTime startDate, DateTime endDate, string adoTeam, string token, string organization);
        Task<ColumnJsonRecord?> GetColumnData(string organization, string token, string team);
        Task<CumulativeFlowJsonRecord?> GetCumulativeFlowData(string organization, string token, string team, string timeSpan);
    }
}