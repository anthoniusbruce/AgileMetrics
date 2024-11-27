using System;
using AgileMetricsRules;

namespace Tests
{
    public class SimulationsTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void SimulateHowManyItemsForGivenDaysAndIndexerBasedOnThroughput()
        {
            var points = new List<ThroughputPoint>
            {
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 20), Throughput = 1},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 21), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 22), Throughput = 3},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 23), Throughput = 4},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 24), Throughput = 5},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 25), Throughput = 6},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 26), Throughput = 7},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 27), Throughput = 8},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 28), Throughput = 9},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 29), Throughput = 10},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 30), Throughput = 1},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 31), Throughput = 0},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 09, 01), Throughput = 0},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 09, 02), Throughput = 0},
            };
            int expectedNumberOfStories = 55;
            var days = 10;
            int i = 0;
            Func<int> indexer = delegate { return i++; };

            var numberOfStories = Simulations.HowManyItemCompleteInGivenNumberOfDays(points, days, indexer);

            Assert.That(numberOfStories, Is.EqualTo(expectedNumberOfStories));
        }

        [Test]
        public void SimulateHowManyItemsForGivenDaysAndIndexerBasedOnThroughputEmptyList()
        {
            var points = new List<ThroughputPoint>();
            int expectedNumberOfStories = 0;
            var days = 10;
            int i = 0;
            Func<int> indexer = delegate { return i++; };

            var numberOfStories = Simulations.HowManyItemCompleteInGivenNumberOfDays(points, days, indexer);

            Assert.That(numberOfStories, Is.EqualTo(expectedNumberOfStories));
        }

        [Test]
        public void SimulateHowManyItemsForGivenDaysBasedOnThroughput()
        {
            var points = new List<ThroughputPoint>
            {
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 20), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 21), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 22), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 23), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 24), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 25), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 26), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 27), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 28), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 29), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 30), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 31), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 09, 01), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 09, 02), Throughput = 2},
            };
            int expectedNumberOfStories = 10;
            var days = 5;

            var numberOfStories = Simulations.HowManyItemCompleteInGivenNumberOfDays(points, days);

            Assert.That(numberOfStories, Is.EqualTo(expectedNumberOfStories));
        }

        [Test]
        public void FullSimulationHowManyItemsForGivenDaysBasedOnThroughput()
        {
            var points = new List<ThroughputPoint>
            {
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 20), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 21), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 22), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 23), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 24), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 25), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 26), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 27), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 28), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 29), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 30), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 31), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 09, 01), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 09, 02), Throughput = 2},
            };
            var expectedSimulation = new Dictionary<int,int> { { 10, 9 }};
            var numberOfSimulations = 9;
            var days = 5;

            var simulations = Simulations.SimulationOfHowManyItemCompleteInGivenNumberOfDays(numberOfSimulations, points, days);

            Assert.That(simulations, Is.EquivalentTo(expectedSimulation));
        }

        [Test]
        public async Task FullAsycSimulationHowManyItemsForGivenDaysBasedOnThroughput()
        {
            var points = new List<ThroughputPoint>
            {
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 20), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 21), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 22), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 23), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 24), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 25), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 26), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 27), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 28), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 29), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 30), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 31), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 09, 01), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 09, 02), Throughput = 2},
            };
            var expectedSimulation = new Dictionary<int, int> { { 10, 9 } };
            var numberOfSimulations = 9;
            var days = 5;

            var simulations = await Simulations.SimulationOfHowManyItemCompleteInGivenNumberOfDaysAsync(numberOfSimulations, points, days);

            Assert.That(simulations, Is.EquivalentTo(expectedSimulation));
        }

        [Test]
        public void SimulateHowManyDaysForGivenNumberOfItemAndIndexerBasedOnThroughput()
        {
            var points = new List<ThroughputPoint>
            {
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 20), Throughput = 1},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 21), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 22), Throughput = 3},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 23), Throughput = 4},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 24), Throughput = 5},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 25), Throughput = 6},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 26), Throughput = 7},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 27), Throughput = 8},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 28), Throughput = 9},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 29), Throughput = 10},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 30), Throughput = 1},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 31), Throughput = 0},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 09, 01), Throughput = 0},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 09, 02), Throughput = 0},
            };
            int numberOfStories = 55;
            int expectedDays = 10;
            int i = 0;
            Func<int> indexer = delegate { return i++; };

            var days = Simulations.HowManyDaysForGivenNumberOfItems(points, numberOfStories, indexer);

            Assert.That(days, Is.EqualTo(expectedDays));
        }

        [Test]
        public void SimulateHowManyDaysForGivenNumberOfItemsBasedOnThroughput()
        {
            var points = new List<ThroughputPoint>
            {
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 20), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 21), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 22), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 23), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 24), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 25), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 26), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 27), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 28), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 29), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 30), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 31), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 09, 01), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 09, 02), Throughput = 2},
            };
            int numberOfStories = 10;
            var expectedDays = 5;

            var actualDays = Simulations.HowManyDaysForGivenNumberOfItems(points, numberOfStories);

            Assert.That(actualDays, Is.EqualTo(expectedDays));
        }

        [Test]
        public void SimulateHowManyDaysForGivenNumberOfItemsBasedOnThroughputAllZeroItems()
        {
            var points = new List<ThroughputPoint>
            {
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 20), Throughput = 0},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 21), Throughput = 0},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 22), Throughput = 0},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 23), Throughput = 0},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 24), Throughput = 0},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 25), Throughput = 0},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 26), Throughput = 0},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 27), Throughput = 0},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 28), Throughput = 0},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 29), Throughput = 0},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 30), Throughput = 0},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 31), Throughput = 0},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 09, 01), Throughput = 0},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 09, 02), Throughput = 0},
            };
            int numberOfStories = 10;
            var expectedDays = 0;

            var actualDays = Simulations.HowManyDaysForGivenNumberOfItems(points, numberOfStories);

            Assert.That(actualDays, Is.EqualTo(expectedDays));
        }

        [Test]
        public void FullSimulationOfHowManyDaysForGivenNumberOfItemsBasedOnThroughput()
        {
            var points = new List<ThroughputPoint>
            {
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 20), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 21), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 22), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 23), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 24), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 25), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 26), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 27), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 28), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 29), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 30), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 31), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 09, 01), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 09, 02), Throughput = 2},
            };
            int numberOfStories = 10;
            int numberOfSimulations = 10;
            var expectedSimulation = new Dictionary<int, int> { {5, 10 }};

            var simulation = Simulations.SimulationOfHowManyDaysForGivenNumberOfItems(numberOfSimulations, points, numberOfStories);

            Assert.That(simulation, Is.EquivalentTo(expectedSimulation));
        }

        [Test]
        public async Task FullAsyncSimulationOfHowManyDaysForGivenNumberOfItemsBasedOnThroughput()
        {
            var points = new List<ThroughputPoint>
            {
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 20), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 21), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 22), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 23), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 24), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 25), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 26), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 27), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 28), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 29), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 30), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 31), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 09, 01), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 09, 02), Throughput = 2},
            };
            int numberOfStories = 10;
            int numberOfSimulations = 10;
            var expectedSimulation = new Dictionary<int, int> { { 5, 10 } };

            var simulation = await Simulations.SimulationOfHowManyDaysForGivenNumberOfItemsAsync(numberOfSimulations, points, numberOfStories);

            Assert.That(simulation, Is.EquivalentTo(expectedSimulation));
        }

        [Test]
        public void PercentilesForSimulationOfHowManyStoriesGivenNumberOfDays()
        {
            var simulations = new SortedDictionary<int, int> { {1, 3 }, { 2, 1 }, { 3, 1 }, { 4, 1 }, { 5, 1 }, { 6, 1 }, { 7, 1 }, { 8, 1 }, { 9, 1 }, { 0, 3 } };
            var expectedPercentiles = new Percentiles
            {
                Thirtieth = 5,
                Fiftieth = 3,
                Seventieth = 1,
                EightyFifth = 0,
                NinetyFifth = 0
            };

            var actual = Simulations.CalculatePercentilesForHowManyStories(simulations);

            Assert.That(actual.Thirtieth, Is.EqualTo(expectedPercentiles.Thirtieth));
            Assert.That(actual.Fiftieth, Is.EqualTo(expectedPercentiles.Fiftieth));
            Assert.That(actual.Seventieth, Is.EqualTo(expectedPercentiles.Seventieth));
            Assert.That(actual.EightyFifth, Is.EqualTo(expectedPercentiles.EightyFifth));
            Assert.That(actual.NinetyFifth, Is.EqualTo(expectedPercentiles.NinetyFifth));
        }

        [Test]
        public void PercentilesForSimulationOfHowManyStoriesGivenNumberOfDaysSameNumberTwice()
        {
            var simulations = new SortedDictionary<int, int> { { 1, 5 }, { 4, 1 }, { 5, 1 }, { 6, 1 }, { 7, 1 }, { 8, 1 }, { 9, 1 }, { 0, 3 } };
            var expectedPercentiles = new Percentiles
            {
                Thirtieth = 5,
                Fiftieth = 1,
                Seventieth = 1,
                EightyFifth = 0,
                NinetyFifth = 0
            };

            var actual = Simulations.CalculatePercentilesForHowManyStories(simulations);

            Assert.That(actual.Thirtieth, Is.EqualTo(expectedPercentiles.Thirtieth));
            Assert.That(actual.Fiftieth, Is.EqualTo(expectedPercentiles.Fiftieth));
            Assert.That(actual.Seventieth, Is.EqualTo(expectedPercentiles.Seventieth));
            Assert.That(actual.EightyFifth, Is.EqualTo(expectedPercentiles.EightyFifth));
            Assert.That(actual.NinetyFifth, Is.EqualTo(expectedPercentiles.NinetyFifth));
        }

        [Test]
        public void PercentilesForSimulationOfHowManyStoriesGivenNumberOfDaysEmptyList()
        {
            var sims = new SortedDictionary<int, int>();
            var expectedPercentiles = new Percentiles();

            var actualPercentiles = Simulations.CalculatePercentilesForHowManyStories(sims);

            Assert.That(actualPercentiles.Thirtieth, Is.EqualTo(expectedPercentiles.Thirtieth));
            Assert.That(actualPercentiles.Fiftieth, Is.EqualTo(expectedPercentiles.Fiftieth));
            Assert.That(actualPercentiles.Seventieth, Is.EqualTo(expectedPercentiles.Seventieth));
            Assert.That(actualPercentiles.EightyFifth, Is.EqualTo(expectedPercentiles.EightyFifth));
            Assert.That(actualPercentiles.NinetyFifth, Is.EqualTo(expectedPercentiles.NinetyFifth));
        }

        [Test]
        public void PercentilesForSimulationOfHowManyStoriesGivenNumberOfDaysListOfOne()
        {
            var sims = new SortedDictionary<int, int> { { 106, 1 } };
            var expectedPercentiles = new Percentiles { Thirtieth = 106, Fiftieth = 106, Seventieth = 106, EightyFifth = 106, NinetyFifth = 106 };

            var actualPercentiles = Simulations.CalculatePercentilesForHowManyStories(sims);

            Assert.That(actualPercentiles.Thirtieth, Is.EqualTo(expectedPercentiles.Thirtieth));
            Assert.That(actualPercentiles.Fiftieth, Is.EqualTo(expectedPercentiles.Fiftieth));
            Assert.That(actualPercentiles.Seventieth, Is.EqualTo(expectedPercentiles.Seventieth));
            Assert.That(actualPercentiles.EightyFifth, Is.EqualTo(expectedPercentiles.EightyFifth));
            Assert.That(actualPercentiles.NinetyFifth, Is.EqualTo(expectedPercentiles.NinetyFifth));
        }

        [Test]
        public void PercentilesForSimulationOfHowManyDaysGivenNumberOfStories()
        {
            var simulations = new SortedDictionary<int, int> { { 1, 3 }, { 2, 1 }, { 3, 1 }, { 4, 1 }, { 5, 1 }, { 6, 1 }, { 7, 1 }, { 8, 1 }, { 9, 1 }, { 0, 3 } };
            var expectedPercentiles = new Percentiles
            {
                Thirtieth = 1,
                Fiftieth = 2,
                Seventieth = 5,
                EightyFifth = 7,
                NinetyFifth = 9
            };

            var actual = Simulations.CalculatePercentilesForHowManyDays(simulations);

            Assert.That(actual.Thirtieth, Is.EqualTo(expectedPercentiles.Thirtieth));
            Assert.That(actual.Fiftieth, Is.EqualTo(expectedPercentiles.Fiftieth));
            Assert.That(actual.Seventieth, Is.EqualTo(expectedPercentiles.Seventieth));
            Assert.That(actual.EightyFifth, Is.EqualTo(expectedPercentiles.EightyFifth));
            Assert.That(actual.NinetyFifth, Is.EqualTo(expectedPercentiles.NinetyFifth));
        }

        [Test]
        public void PercentilesForSimulationOfHowManyDaysGivenNumberOfStoriesSameNumberTwice()
        {
            var simulations = new SortedDictionary<int, int> { { 1, 5 }, { 4, 1 }, { 5, 1 }, { 6, 1 }, { 7, 1 }, { 8, 1 }, { 9, 1 }, { 0, 3 } };
            var expectedPercentiles = new Percentiles
            {
                Thirtieth = 1,
                Fiftieth = 1,
                Seventieth = 5,
                EightyFifth = 7,
                NinetyFifth = 9
            };

            var actual = Simulations.CalculatePercentilesForHowManyDays(simulations);

            Assert.That(actual.Thirtieth, Is.EqualTo(expectedPercentiles.Thirtieth));
            Assert.That(actual.Fiftieth, Is.EqualTo(expectedPercentiles.Fiftieth));
            Assert.That(actual.Seventieth, Is.EqualTo(expectedPercentiles.Seventieth));
            Assert.That(actual.EightyFifth, Is.EqualTo(expectedPercentiles.EightyFifth));
            Assert.That(actual.NinetyFifth, Is.EqualTo(expectedPercentiles.NinetyFifth));
        }

        [Test]
        public void PercentilesForSimulationOfHowManyDaysGivenNumberOfStoriesEmptyList()
        {
            var sims = new SortedDictionary<int, int>();
            var expectedPercentiles = new Percentiles();

            var actualPercentiles = Simulations.CalculatePercentilesForHowManyDays(sims);

            Assert.That(actualPercentiles.Thirtieth, Is.EqualTo(expectedPercentiles.Thirtieth));
            Assert.That(actualPercentiles.Fiftieth, Is.EqualTo(expectedPercentiles.Fiftieth));
            Assert.That(actualPercentiles.Seventieth, Is.EqualTo(expectedPercentiles.Seventieth));
            Assert.That(actualPercentiles.EightyFifth, Is.EqualTo(expectedPercentiles.EightyFifth));
            Assert.That(actualPercentiles.NinetyFifth, Is.EqualTo(expectedPercentiles.NinetyFifth));
        }

        [Test]
        public void PercentilesForSimulationOfHowManyDaysGivenNumberOfStoriesListOfOne()
        {
            var sims = new SortedDictionary<int, int> { { 106, 1 } };
            var expectedPercentiles = new Percentiles { Thirtieth = 106, Fiftieth = 106, Seventieth = 106, EightyFifth = 106, NinetyFifth = 106 };

            var actualPercentiles = Simulations.CalculatePercentilesForHowManyDays(sims);

            Assert.That(actualPercentiles.Thirtieth, Is.EqualTo(expectedPercentiles.Thirtieth));
            Assert.That(actualPercentiles.Fiftieth, Is.EqualTo(expectedPercentiles.Fiftieth));
            Assert.That(actualPercentiles.Seventieth, Is.EqualTo(expectedPercentiles.Seventieth));
            Assert.That(actualPercentiles.EightyFifth, Is.EqualTo(expectedPercentiles.EightyFifth));
            Assert.That(actualPercentiles.NinetyFifth, Is.EqualTo(expectedPercentiles.NinetyFifth));
        }

        [Test]
        public void FullSimulateHowManyDaysReturnsInSortedOrder()
        {
            var points = new List<ThroughputPoint>
            {
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 20), Throughput = 1},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 21), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 22), Throughput = 3},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 23), Throughput = 4},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 24), Throughput = 5},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 25), Throughput = 6},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 26), Throughput = 7},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 27), Throughput = 8},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 28), Throughput = 9},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 29), Throughput = 10},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 30), Throughput = 1},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 31), Throughput = 0},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 09, 01), Throughput = 0},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 09, 02), Throughput = 0},
            };
            int numberOfStories = 100;
            int numberOfSimulations = 100;

            var simulation = Simulations.SimulationOfHowManyDaysForGivenNumberOfItems(numberOfSimulations, points, numberOfStories);
            var simulationArray = simulation.ToArray();
            var expected = simulation.OrderBy(x => x.Key);
            var expectedArray = expected.ToArray();

            Assert.That(simulation, Is.EquivalentTo(expected));
            Assert.That(simulationArray, Is.EqualTo(expectedArray));
        }

        [Test]
        public void FullSimulateHowManyStoriesReturnsInSortedOrder()
        {
            var points = new List<ThroughputPoint>
            {
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 20), Throughput = 1},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 21), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 22), Throughput = 3},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 23), Throughput = 4},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 24), Throughput = 5},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 25), Throughput = 6},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 26), Throughput = 7},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 27), Throughput = 8},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 28), Throughput = 9},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 29), Throughput = 10},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 30), Throughput = 1},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 31), Throughput = 0},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 09, 01), Throughput = 0},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 09, 02), Throughput = 0},
            };
            int numberOfDays = 100;
            int numberOfSimulations = 100;

            var simulation = Simulations.SimulationOfHowManyItemCompleteInGivenNumberOfDays(numberOfSimulations, points, numberOfDays);
            var simulationArray = simulation.ToArray();
            var expected = simulation.OrderBy(x => x.Key);
            var expectedArray = expected.ToArray();

            Assert.That(simulation, Is.EquivalentTo(expected));
            Assert.That(simulationArray, Is.EqualTo(expectedArray));
        }
    }
}