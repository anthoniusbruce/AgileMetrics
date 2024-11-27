namespace AgileMetricsRules
{
	public class DateValuePair
	{
        public string x { get; private set; }
        public int y { get; private set; }

        public DateValuePair(DateTime date, int cycleTime)
        {
            x = Utility.DateToJsonDate(date);
            y = cycleTime;
        }
    }

    public class DeliveryEfficiencyResults
    {
        public string description { get; private set; }
        public DateValuePair[] percentileData { get; private set; }

        public DeliveryEfficiencyResults(string desc, DateValuePair[] data)
        {
            description = desc;
            percentileData = data;
        }
    }
}

