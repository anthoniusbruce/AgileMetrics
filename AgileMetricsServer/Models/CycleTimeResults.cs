using AgileMetricsRules;

namespace AgileMetricsServer.Models
{
	public class CycleTimeResults
	{
        public string x { get; private set; }
        public int y { get; private set; }
        public string workItemId { get; private set; }
        public int thirtieth { get; private set; }
        public int fiftieth { get; private set; }
        public int seventieth { get; private set; }
        public int eightyFifth { get; private set; }
        public int ninetyFifth { get; private set; }

        public CycleTimeResults(string workItem, DateTime completed, int cycleTime, int thirty, int fifty, int seventy, int eightyFive, int ninetyFive)
		{
            x = Utility.DateToJsonDate(completed);
            y = cycleTime;
            workItemId = workItem;
            thirtieth = thirty;
            fiftieth = fifty;
            seventieth = seventy;
            eightyFifth = eightyFive;
            ninetyFifth = ninetyFive;
        }
    }
}

