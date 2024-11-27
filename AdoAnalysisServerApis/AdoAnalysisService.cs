using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using AgileMetricsRules;

namespace AdoAnalysisServerApis
{
    public class AdoAnalysisService : IAdoAnalysisService
    {
        private const string epicFeatureIdsParameters = "WorkItems?%24select=WorkItemId&%24filter=WorkItemType+eq+%27Feature%27+and+ParentWorkItemId+eq+{0:0}+and+State+ne+%27Removed%27";
        private const string CycleTimeScatterPlotParameters = "WorkItems?%24select=ActivatedDate%2C+CompletedDate%2C+WorkItemId&%24filter=%28{0}%29+and+{1}StateCategory+eq+%27Completed%27+and+CompletedDateSK+ge+{2}+and+CompletedDateSK+le+{3}+and+Teams%2Fany%28t%3A+t%2FTeamName+eq+%27{4}%27%29";
        private const string AgingWorkInProgressParameters = "WorkItemBoardSnapshot?%24apply=filter%28+%28{0}%29+and+Team%2FTeamName+eq+%27{1}%27+and+BoardName+eq+%27Stories%27+and+DateSK+eq+%28year%28now%28%29%29+mul+10000%29+add+%28month%28now%28%29%29+mul+100%29+add+day%28now%28%29%29+and+ActivatedDateSK+ne+null+and+{2}State+ne+%27Closed%27+%29+%2Fgroupby%28+%28ColumnOrder%2CIsDone%2CActivatedDateSK%2CColumnName%2CWorkItemId%29+%29";
        private const string UserStoryThroughputParameters = "WorkItems?%24apply=filter%28%28{0}%29+and+StateCategory+eq+%27Completed%27+and+CompletedDateSK+ge+{1}+and+CompletedDateSK+le+{2}+and+Teams%2Fany%28t%3At%2FTeamName+eq+%27{3}%27%29%29+%2Fgroupby%28%28CompletedDateSK%29%2Caggregate%28%24count+as+Throughput%29%29";
        private const string FeatureProgressParameters = "WorkItemSnapshot?%24apply=filter%28%28{0}%29+and+{1}WorkItemType+eq+%27User+Story%27+and+State+ne+%27Removed%27%29+%2Fgroupby%28%28DateSK%2C+WorkItemId%2C+State%29%29";
        private const string ColumnParameters = "BoardLocations?%24apply=filter%28+Team%2FTeamName+eq+%27{0}%27+and+BoardName+eq+%27Stories%27+and+IsCurrent+eq+true+%29+%2Fgroupby+%28%28ColumnOrder%2CColumnId%2CColumnName%2CDone%2CIsDone%29%29";
        private const string CumulativeFlowParameters = "WorkItemBoardSnapshot?%24apply=filter%28+Team%2FTeamName+eq+%27{0}%27+and+BoardName+eq+%27Stories%27+and+%28WorkItemType+eq+%27User+Story%27+or+WorkItemType+eq+%27Bug%27%29+and+DateSK+ge+{1}+and+State+ne+%27New%27+and+%28ClosedDateSK+eq+null+or+ClosedDateSK+ge+{1}%29+%29+%2Fgroupby%28+%28WorkItemId%2CDateSK%2CColumnId%2CIsDone%29+%29";
        private const string WorkItemTypeUserStoryString = "WorkItemType+eq+%27User+Story%27";
        private const string WorkItemTypeBugString = "WorkItemType+eq+%27Bug%27";
        private const string WorkItemTypeUserStoryAndBugString = "WorkItemType+eq+%27User+Story%27+or+WorkItemType+eq+%27Bug%27";
        private const string Metadata = "$metadata";
        private const string FrequencyDayString = "Each day";
        private const string FrequencyWeekString = "Once a week";
        private const string FrequencyMonthString = "Once a month";
        private const string trTaxOrganization = "tr-tax";
        private const string trTaxDefaultPatagoniaOrganization = "tr-tax-default-Patagonia";
        private const string trTaxDefaultEflexwareOrganization = "tr-tax-default-eFlexware";
        private const string OneMonthString = "1 month";
        private const string TwoMonthsString = "2 months";
        private const string ThreeMonthsString = "3 months";

        private readonly IHttpClientFactory httpClientFactory;

        public static string WorkItemTypeUserStory { get { return WorkItemTypeUserStoryString; } }
        public static string WorkItemTypeBug { get { return WorkItemTypeBugString; } }
        public static string WorkItemTypeUserStoryAndBug { get { return WorkItemTypeUserStoryAndBugString; } }
        public static string FrequencyDay { get { return FrequencyDayString; } }
        public static string FrequencyWeek { get { return FrequencyWeekString; } }
        public static string FrequencyMonth { get { return FrequencyMonthString; } }
        public static string TrTaxOrganization { get { return trTaxOrganization; } }
        public static string TrTaxDefaultPatagoniaOrganization { get { return trTaxDefaultPatagoniaOrganization; } }
        public static string TrTaxDefaultEflexwareOrganization { get { return trTaxDefaultEflexwareOrganization; } }
        public static string OneMonth { get { return OneMonthString; } }
        public static string TwoMonths { get { return TwoMonthsString; } }
        public static string ThreeMonths { get { return ThreeMonthsString; } }

        public AdoAnalysisService(IHttpClientFactory factory)
        {
            httpClientFactory = factory;
        }

        private HttpClient CreateClient(string token, string organization)
        {
            var client = httpClientFactory.CreateClient(organization);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", "", token))));
            return client;
        }

        public async Task<HttpResponseMessage> GetMetadata(string token, string organization)
        {
            HttpClient client = CreateClient(token, organization);

            var result = await client.GetAsync(Metadata);

            return result;
        }

        public async Task<CumulativeFlowJsonRecord?> GetCumulativeFlowData(string organization, string token, string team, string timeSpan)
        {
            var teamQuery = FormatOdata(FormatUserEnteredData(team));
            var startDate = BuildTimeSpanStartDate(timeSpan);

            HttpClient client = CreateClient(token, organization);

            CumulativeFlowJsonRecord? result;
            string query = string.Format(CumulativeFlowParameters, teamQuery, startDate);
            try
            {
                result = await client.GetFromJsonAsync<CumulativeFlowJsonRecord>(query);
            }
            catch (HttpRequestException e)
            {
                if (e.StatusCode == HttpStatusCode.Unauthorized)
                {
                    result = new CumulativeFlowJsonRecord { Value = new List<CumulativeFlowJsonRec>() };
                    result.NotAuthorized = true;
                }
                else if (e.StatusCode == HttpStatusCode.BadRequest)
                {
                    result = new CumulativeFlowJsonRecord { Value = new List<CumulativeFlowJsonRec>() };
                    result.BadRequest = true;
                }
                else
                    throw;
            }

            return result;
        }

        public async Task<ColumnJsonRecord?> GetColumnData(string organization, string token, string team)
        {
            var teamQuery = FormatOdata(FormatUserEnteredData(team));

            HttpClient client = CreateClient(token, organization);

            ColumnJsonRecord? result;
            string query = string.Format(ColumnParameters, teamQuery);
            try
            {
                result = await client.GetFromJsonAsync<ColumnJsonRecord>(query);
            }
            catch (HttpRequestException e)
            {
                if (e.StatusCode == HttpStatusCode.Unauthorized)
                {
                    result = new ColumnJsonRecord { Value = new List<ColumnJsonRec>() };
                    result.NotAuthorized = true;
                }
                else if (e.StatusCode == HttpStatusCode.BadRequest)
                {
                    result = new ColumnJsonRecord { Value = new List<ColumnJsonRec>() };
                    result.BadRequest = true;
                }
                else
                    throw;
            }

            return result;
        }

        public async Task<EpicFeatureIdsJsonRecord?> GetEpicFeatureIdsData(int epicId, string token, string organization)
        {
            var epicQuery = string.Format(epicFeatureIdsParameters, epicId);

            HttpClient client = CreateClient(token, organization);

            EpicFeatureIdsJsonRecord? result;
            try
            {
                var res = client.GetStringAsync(epicQuery);
                result = await client.GetFromJsonAsync<EpicFeatureIdsJsonRecord?>(epicQuery);
            }
            catch (HttpRequestException e)
            {
                if (e.StatusCode == HttpStatusCode.Unauthorized)
                {
                    result = new EpicFeatureIdsJsonRecord { Value = new List<EpicFeatureIdsJsonRec>() };
                    result.NotAuthorized = true;
                }
                else if (e.StatusCode == HttpStatusCode.BadRequest)
                {
                    result = new EpicFeatureIdsJsonRecord { Value = new List<EpicFeatureIdsJsonRec>() };
                    result.BadRequest = true;
                }
                else
                    throw;
            }

            return result;
        }

        public async Task<FeatureChildrenJsonRecord?> GetFeatureProgressData(string featureIds, DateTime startDate, string reportingFrequency, string token, string organization)
        {
            var queryDates = BuildDatesQuerySubstring(startDate, reportingFrequency);
            var featureIdsQuery = BuildFeatureIdsSubstring(featureIds);

            if (string.IsNullOrWhiteSpace(queryDates))
                return new FeatureChildrenJsonRecord { Value = new List<FeatureChildrenJsonRec>()};

            HttpClient client = CreateClient(token, organization);

            FeatureChildrenJsonRecord? result;
            string query = string.Format(FeatureProgressParameters, queryDates, featureIdsQuery);
            try
            {
                result = await client.GetFromJsonAsync<FeatureChildrenJsonRecord>(query);
            }
            catch (HttpRequestException e)
            {
                if (e.StatusCode == HttpStatusCode.Unauthorized)
                {
                    result = new FeatureChildrenJsonRecord { Value = new List<FeatureChildrenJsonRec>() };
                    result.NotAuthorized = true;
                }
                else if (e.StatusCode == HttpStatusCode.BadRequest)
                {
                    result = new FeatureChildrenJsonRecord { Value = new List<FeatureChildrenJsonRec>() };
                    result.BadRequest = true;
                }
                else
                    throw;
            }

            return result;
        }

        public async Task<ScatterPlotJsonRecord?> GetCycleTimeScatterPlotData(string workItemType, DateTime startDate, DateTime endDate, string tags, string adoTeam, string token, string organization)
        {
            var tuple = FormatFromToDate(startDate, endDate);
            var fromDate = tuple.Item1;
            var toDate = tuple.Item2;
            var team = FormatOdata(FormatUserEnteredData(adoTeam));
            var type = FormatOdata(workItemType);
            var tagsQuery = BuildTagsSubstring(tags);
            HttpClient client = CreateClient(token, organization);


            ScatterPlotJsonRecord? result;
            string query = string.Format(CycleTimeScatterPlotParameters, type, tagsQuery, fromDate, toDate, team);
            try
            {
                result = await client.GetFromJsonAsync<ScatterPlotJsonRecord>(query);
            }
            catch (HttpRequestException e)
            {
                if (e.StatusCode == HttpStatusCode.Unauthorized)
                {
                    result = new ScatterPlotJsonRecord { Value = new List<ScatterPlotJsonRec>() };
                    result.NotAuthorized = true;
                }
                else if (e.StatusCode == HttpStatusCode.BadRequest)
                {
                    result = new ScatterPlotJsonRecord { Value = new List<ScatterPlotJsonRec>() };
                    result.BadRequest = true;
                }
                else
                    throw;
            }

            return result;
        }

        public async Task<AgingWipJsonRecord?> GetAgingWorkInProgressData(string workItemType, string tags, string adoTeam, string token, string organization)
        {
            var type = FormatOdata(workItemType);
            var tagsQuery = BuildTagsSubstring(tags);
            var team = FormatOdata(adoTeam);
            HttpClient client = CreateClient(token, organization);

            AgingWipJsonRecord? result;
            string query = string.Format(AgingWorkInProgressParameters, type, team, tagsQuery);
            try
            {
                result = await client.GetFromJsonAsync<AgingWipJsonRecord>(query);
            }
            catch (HttpRequestException e)
            {
                if (e.StatusCode == HttpStatusCode.Unauthorized)
                {
                    result = new AgingWipJsonRecord { value = new List<AgingWipJsonRec>() };
                    result.NotAuthorized = true;
                }
                else if (e.StatusCode == HttpStatusCode.BadRequest)
                {
                    result = new AgingWipJsonRecord { value = new List<AgingWipJsonRec>() };
                    result.BadRequest = true;
                }
                else
                    throw;
            }

            return result;
        }

        public async Task<ScatterPlotSeriesJsonRecord?> GetCycleTimeSeriesScatterPlotData(string workItemType, int cycleTimeSpan, DateTime evaluationPeriodStart, DateTime evaluationPeriodEnd, string evaluationPeriodFrequency, string tags, string adoTeam, string token, string organization)
        {
            ScatterPlotSeriesJsonRecord result;

            result = new ScatterPlotSeriesJsonRecord { Value = new Dictionary<DateTime, ScatterPlotJsonRecord>() };
            result.NotAuthorized = false;
            result.Value = new Dictionary<DateTime, ScatterPlotJsonRecord>();

            for (DateTime date = evaluationPeriodEnd; date >= evaluationPeriodStart; date = GetNextEarlierDate(date, evaluationPeriodFrequency))
            {
                var intermediateResult = await GetCycleTimeScatterPlotData(workItemType, date.AddDays(-cycleTimeSpan), date, tags, adoTeam, token, organization);

                if (intermediateResult == null)
                    return null;
                if (intermediateResult.NotAuthorized)
                {
                    result.NotAuthorized = true;
                    return result;
                }
                result.Value[date] = intermediateResult;
            }

            return result;
        }

        public async Task<ThroughputJsonRecord?> GetThroughputData(string workItemType, DateTime startDate, DateTime endDate, string adoTeam, string token, string organization)
        {
            var tuple = FormatFromToDate(startDate, endDate);
            var fromDate = tuple.Item1;
            var toDate = tuple.Item2;
            var team = FormatOdata(FormatUserEnteredData(adoTeam));
            var type = FormatOdata(workItemType);
            var client = CreateClient(token, organization);

            ThroughputJsonRecord? result;
            string query = string.Format(UserStoryThroughputParameters, type, fromDate, toDate, team);
            try
            {
                result = await client.GetFromJsonAsync<ThroughputJsonRecord>(query);
            }
            catch (HttpRequestException e)
            {
                if (e.StatusCode == HttpStatusCode.Unauthorized)
                {
                    result = new ThroughputJsonRecord { Value = new List<ThroughputJsonRec>() };
                    result.NotAuthorized = true;
                }
                else if (e.StatusCode == HttpStatusCode.BadRequest)
                {
                    result = new ThroughputJsonRecord { Value = new List<ThroughputJsonRec>() };
                    result.BadRequest = true;
                }
                else
                    throw;
            }

            return result;
        }

        private static string FormatUserEnteredData(string userData)
        {
            var ret = userData.Replace("'", "");
            ret = ret.Replace("%27", "");
            return ret;
        }

        private static string FormatOdata(string odataOriginal)
        {
            var odata = odataOriginal.Replace("/", "%2F");
            odata = odata.Replace("(", "%28");
            odata = odata.Replace(")", "%29");
            odata = odata.Replace("'", "%27");
            odata = odata.Replace(" ", "+");

            return odata;
        }

        private static Tuple<int,int> FormatFromToDate(DateTime d1, DateTime d2)
        {
            DateTime from;
            DateTime to;
            if (d1 < d2)
            {
                from = d1;
                to = d2;
            }
            else
            {
                from = d2;
                to = d1;
            }

            return new Tuple<int, int>(Utility.DateToDateSk(from), Utility.DateToDateSk(to));
        }

        private DateTime GetNextEarlierDate(DateTime date, string frequency)
        {
            DateTime ret;
            if (frequency == FrequencyDay)
                ret = date.AddDays(-1);
            else if (frequency == FrequencyWeek)
                ret = date.AddDays(-7);
            else
                ret = date.AddMonths(-1);
            return ret;
        }

        private string BuildDatesQuerySubstring(DateTime startDate, string reportingFrequency)
        {
            DateTime endDate = DateTime.Today;
            var dateFormat = "DateSK+eq+{0}";

            var ret = string.Empty;
            for (DateTime date = endDate; date >= startDate; date = GetNextEarlierDate(date, reportingFrequency))
            {
                var or = "+or+";
                if (string.IsNullOrEmpty(ret))
                    or = string.Empty;

                var dateSk = Utility.DateToDateSk(date);

                ret = ret + or + string.Format(dateFormat, dateSk);
            }

            return ret;
        }

        private string BuildTagsSubstring(string tags)
        {
            string outerQueryFormat = "%28TagNames+eq+null+or+%28{0}%29%29+and+";
            string tagQueryFormat = "not+contains%28TagNames%2C+%27{0}%27%29";
            var ret = string.Empty;

            string[] split = tags.Split(';');

            foreach (var item in split)
            {
                string val = item.Trim();
                val = FormatOdata(FormatUserEnteredData(val));
                if (!string.IsNullOrWhiteSpace(val))
                {
                    var and = "+and+";
                    if (string.IsNullOrWhiteSpace(ret))
                        and = string.Empty;

                    ret = ret + and + string.Format(tagQueryFormat, val);
                }
            }

            if (!string.IsNullOrWhiteSpace(ret))
                ret = string.Format(outerQueryFormat, ret);

            return ret;
        }


        private string BuildFeatureIdsSubstring(string featureIdsString)
        {
            string outerQueryFormat = "%28{0}%29+and+";
            string idQueryFormat = "ParentWorkItemId+eq+{0}";
            string ret = string.Empty;

            string[] split = featureIdsString.Split(';');

            foreach (var item in split)
            {
                string val = item.Trim();
                if (!string.IsNullOrWhiteSpace(val))
                {
                    var or = "+or+";
                    if (string.IsNullOrWhiteSpace(ret))
                        or = string.Empty;

                    ret = ret + or + string.Format(idQueryFormat, FormatUserEnteredData(val));
                }
            }

            if (!string.IsNullOrWhiteSpace(ret))
                ret = string.Format(outerQueryFormat, ret);

            return ret;
        }

        private int BuildTimeSpanStartDate(string timeSpan)
        {
            int ret;

            DateTime today = DateTime.Today;
            DateTime startDate = DateTime.MinValue;
            if (timeSpan == OneMonth)
                startDate = today.AddMonths(-1);
            else if (timeSpan == TwoMonths)
                startDate = today.AddMonths(-2);
            else if (timeSpan == ThreeMonths)
                startDate = today.AddMonths(-3);

            ret = Utility.DateToDateSk(startDate);

            return ret;
        }


        static public string GetWorkItemTypeQuerySubstring(string? workItemType)
        {
            string ret = string.Empty;

            if (string.IsNullOrWhiteSpace(workItemType))
                return ret;

            ret = workItemType.Trim();
            if (ret == "User Story")
                ret = WorkItemTypeUserStory;
            else if (ret == "Bug")
                ret = WorkItemTypeBug;
            else if (ret == "User Story and Bug")
                ret = WorkItemTypeUserStoryAndBug;
            else
                ret = string.Empty;

            return ret;
        }

    }
}

