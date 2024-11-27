namespace AgileMetricsRules
{
    public class FeatureProgressResults
	{
        public string x { get; private set; }
        public int y { get; private set; }

        public FeatureProgressResults(DateTime completed, int count)
		{
            x = Utility.DateToJsonDate(completed);
            y = count;
        }

        public static List<FeatureProgressResults?> GetState(Dictionary<string, List<FeatureChildPoint>> dataPoints, string state)
        {
            if (!dataPoints.ContainsKey(state))
                return new List<FeatureProgressResults?>();

            return dataPoints[state].Select(item => new FeatureProgressResults(item.Date, item.Count)).DefaultIfEmpty().ToList();
        }
    }
}