using AgileMetricsRules;

namespace AdoAnalysisServerApis
{
    public interface IAdoAnalysisService
    {
        Task<EpicFeatureIdsJsonRecord?> GetEpicFeatureIdsData(int epicId, string token);
        Task<HttpResponseMessage> GetMetadata(string token);
        Task<FeatureChildrenJsonRecord?> GetFeatureProgressData(string featureIds, DateTime startDate, string reportingFrequency, string token);
        Task<ScatterPlotJsonRecord?> GetCycleTimeScatterPlotData(string workItemType, DateTime startDate, DateTime endDate, string tags, string areaPath, string token);
        Task<AgingWipJsonRecord?> GetAgingWorkInProgressData(string workItemType, string tags, string adoTeam, string token);
        Task<ScatterPlotSeriesJsonRecord?> GetCycleTimeSeriesScatterPlotData(string workItemType, int cycleTimeSpan, DateTime evaluationPeriodStart, DateTime evaluationPeriodEnd, string evaluationPeriodFrequency, string tags, string adoTeam, string token);
        Task<ThroughputJsonRecord?> GetThroughputData(string workItemType, DateTime startDate, DateTime endDate, string adoTeam, string token);
        Task<ColumnJsonRecord?> GetColumnData(string token, string team);
        Task<CumulativeFlowJsonRecord?> GetCumulativeFlowData(string token, string team, string timeSpan);
    }
}