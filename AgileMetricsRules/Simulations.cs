namespace AgileMetricsRules
{
    public class Simulations
    {
        public Simulations()
        {
        }

        public static Percentiles CalculatePercentilesForHowManyDays(SortedDictionary<int, int> simulations)
        {
            return CalculatePercentiles(simulations, false);
        }

        public static Percentiles CalculatePercentilesForHowManyStories(SortedDictionary<int, int> simulations)
        {
            return CalculatePercentiles(simulations, true);
        }

        private static Percentiles CalculatePercentiles(SortedDictionary<int, int> simulations, bool sortDescending)
        {
            var simCount = simulations.Sum(x => x.Value);
            if (simCount == 0)
                return new Percentiles();

            int thirtieth = (int)Math.Ceiling(simCount * 0.3M);
            int fiftieth = (int)Math.Ceiling(simCount * 0.5M);
            int seventieth = (int)Math.Ceiling(simCount * 0.7M);
            int eightyFifth = (int)Math.Ceiling(simCount * 0.85M);
            int ninetyFifth = (int)Math.Ceiling(simCount * 0.95M);
            var orderedList = sortDescending ? simulations.OrderByDescending(n => n.Key) : simulations.OrderBy(n => n.Key);

            var percentiles = new Percentiles();
            foreach (var item in orderedList)
            {
                thirtieth -= item.Value;
                fiftieth -= item.Value;
                seventieth -= item.Value;
                eightyFifth -= item.Value;
                ninetyFifth -= item.Value;

                if (thirtieth == 0 || (thirtieth < 0 && thirtieth + item.Value > 0))
                {
                    percentiles.Thirtieth = item.Key;
                }

                if (fiftieth == 0 || (fiftieth < 0 && fiftieth + item.Value > 0))
                {
                    percentiles.Fiftieth = item.Key;
                }

                if (seventieth == 0 || (seventieth < 0 && seventieth + item.Value > 0))
                {
                    percentiles.Seventieth = item.Key;
                }

                if (eightyFifth == 0 || (eightyFifth < 0 && eightyFifth + item.Value > 0))
                {
                    percentiles.EightyFifth = item.Key;
                }

                if (ninetyFifth == 0 || (ninetyFifth < 0 && ninetyFifth + item.Value > 0))
                {
                    percentiles.NinetyFifth = item.Key;
                }
            }

            return percentiles;
        }

        public static int HowManyDaysForGivenNumberOfItems(List<ThroughputPoint> points, int numberOfStories, Func<int> indexer)
        {
            var totalDays = 0;
            var totalStories = 0;

            if (points.Sum(x => x.Throughput) == 0)
                return totalDays;

            while (totalStories < numberOfStories)
            {
                totalStories += points[indexer()].Throughput;
                totalDays++;
            }

            return totalDays;
        }

        public static int HowManyDaysForGivenNumberOfItems(List<ThroughputPoint> points, int numberOfStories)
        {
            Func<int> randomIndexer = delegate { var rand = new Random(); return rand.Next(points.Count); };
            return HowManyDaysForGivenNumberOfItems(points, numberOfStories, randomIndexer);
        }

        public static int HowManyItemCompleteInGivenNumberOfDays(List<ThroughputPoint> points, int days, Func<int> indexer)
        {
            var totalStories = 0;

            if (points.Count() == 0)
                return totalStories;

            for (int i = 0; i < days; i++)
            {
                var index = indexer();
                totalStories += points[index].Throughput;
            }

            return totalStories;
        }

        public static int HowManyItemCompleteInGivenNumberOfDays(List<ThroughputPoint> points, int days)
        {
            Func<int> randomIndexer = delegate { var rand = new Random(); return rand.Next(points.Count); };
            return HowManyItemCompleteInGivenNumberOfDays(points, days, randomIndexer);
        }

        public static SortedDictionary<int, int> SimulationOfHowManyDaysForGivenNumberOfItems(int numberOfSimulations, List<ThroughputPoint> points, int numberOfStories)
        {
            var simulations = new SortedDictionary<int, int>();
            for (int i = 0; i < numberOfSimulations; i++)
            {
                var simRes = HowManyDaysForGivenNumberOfItems(points, numberOfStories);
                if (simulations.ContainsKey(simRes))
                    simulations[simRes] = simulations[simRes] + 1;
                else
                    simulations[simRes] = 1;
            }

            return simulations;
        }

        public static async Task<SortedDictionary<int, int>> SimulationOfHowManyDaysForGivenNumberOfItemsAsync(int numberOfSimulations, List<ThroughputPoint> points, int numberOfStories)
        {
            return await Task<SortedDictionary<int, int>>.Run(() => SimulationOfHowManyDaysForGivenNumberOfItems(numberOfSimulations, points, numberOfStories));
        }

        public static SortedDictionary<int, int> SimulationOfHowManyItemCompleteInGivenNumberOfDays(int numberOfSimulations, List<ThroughputPoint> points, int days)
        {
            var simulations = new SortedDictionary<int, int>();
            for (int i = 0; i < numberOfSimulations; i++)
            {
                var simRes = HowManyItemCompleteInGivenNumberOfDays(points, days);
                if (simulations.ContainsKey(simRes))
                    simulations[simRes] = simulations[simRes] + 1;
                else
                    simulations[simRes] = 1;
            }

            return simulations;
        }

        public static async Task<SortedDictionary<int, int>> SimulationOfHowManyItemCompleteInGivenNumberOfDaysAsync(int numberOfSimulations, List<ThroughputPoint> points, int days)
        {
            return await Task<SortedDictionary<int, int>>.Run(() => SimulationOfHowManyItemCompleteInGivenNumberOfDays(numberOfSimulations, points, days));
        }
    }
}

