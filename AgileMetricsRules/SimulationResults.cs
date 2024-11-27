namespace AgileMetricsRules
{
    public class SimulationResults
    {
        public required IEnumerable<ChartPoint> simulations { get; set; }
        public required IEnumerable<ChartPoint> thirtieth { get; set; }
        public required IEnumerable<ChartPoint> fiftieth { get; set; }
        public required IEnumerable<ChartPoint> seventieth { get; set; }
        public required IEnumerable<ChartPoint> eightyFifth { get; set; }
        public required IEnumerable<ChartPoint> ninetyFifth { get; set; }

        public string ToCsv(string adoTeam, string workItemType, DateTime startingDate, DateTime endingDate, int numberOfStories, int numberOfSimulations)
        {
            string queryFormat = "Team,Work item type,Starting date,Ending date,Number of stories,Number of simulations\n{0},{1},{2:d},{3:d},{4},{5}\n,,,\n";
            string percentileFormat = "percentile,days,,\n30th,{0},,,\n50th,{1},,,\n70th,{2},,\n85th,{3},,\n95th,{4},,";

            string results = "days,count of occurrences,,\n";
            foreach (var sim in simulations)
            {
                results = string.Format("{0}{1},{2},,\n", results, sim.x, sim.y);
            }

            var defaultValue = new ChartPoint { x = string.Empty };

            var simulationResults =
                string.Format(queryFormat, adoTeam, workItemType, startingDate.Date, endingDate.Date, numberOfStories, numberOfSimulations) +
                results +
                ",,,\n" +
                string.Format(percentileFormat, thirtieth.FirstOrDefault(defaultValue).x, fiftieth.FirstOrDefault(defaultValue).x,
                    seventieth.FirstOrDefault(defaultValue).x, eightyFifth.FirstOrDefault(defaultValue).x, ninetyFifth.FirstOrDefault(defaultValue).x);

            return simulationResults;
        }
    }
}

