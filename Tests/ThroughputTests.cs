using System;
using AgileMetricsRules;
using static AgileMetricsRules.ThroughputJsonRecord;

namespace Tests
{
    public class ThroughputTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ParseJsonForUserStoryThroughput()
        {
            var userStoryData = new ThroughputJsonRecord { Value = new List<ThroughputJsonRec>() };

            using (FileStream fs = File.Open("CannedData/usTpOut.json", FileMode.Open))
            {
                userStoryData = Throughput.ParseJsonStream(fs);
            }

            Assert.NotNull(userStoryData);
            Assert.NotNull(userStoryData.Value);
            Assert.That(userStoryData.Value.Count, Is.EqualTo(13));
        }

        [Test]
        public void ParseJsonForFeatureThroughput()
        {
            var userStoryData = new ThroughputJsonRecord { Value = new List<ThroughputJsonRec>() };

            using (FileStream fs = File.Open("CannedData/fTpOut.json", FileMode.Open))
            {
                userStoryData = Throughput.ParseJsonStream(fs);
            }

            Assert.NotNull(userStoryData);
            Assert.NotNull(userStoryData.Value);
            Assert.That(userStoryData.Value.Count, Is.EqualTo(22));
        }

        [Test]
        public void CalculateThroughput()
        {
            var startDate = new DateTime(2022, 08, 20);
            var endDate = new DateTime(2022, 09, 11);
            var throughputJson = new ThroughputJsonRecord
            {
                Value = new List<ThroughputJsonRec>
                {
                    new ThroughputJsonRec { CompletedDateSK = 20220822, Throughput = 10},
                    new ThroughputJsonRec { CompletedDateSK = 20220823, Throughput = 2},
                    new ThroughputJsonRec { CompletedDateSK = 20220826, Throughput = 2},
                    new ThroughputJsonRec { CompletedDateSK = 20220830, Throughput = 1},
                    new ThroughputJsonRec { CompletedDateSK = 20220905, Throughput = 8},
                    new ThroughputJsonRec { CompletedDateSK = 20220906, Throughput = 2},
                    new ThroughputJsonRec { CompletedDateSK = 20220909, Throughput = 1}
                }
            };
            var expectedPoints = new List<ThroughputPoint>
            {
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 20), Throughput = 0},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 21), Throughput = 0},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 22), Throughput = 10},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 23), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 24), Throughput = 0},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 25), Throughput = 0},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 26), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 27), Throughput = 0},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 28), Throughput = 0},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 29), Throughput = 0},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 30), Throughput = 1},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 08, 31), Throughput = 0},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 09, 01), Throughput = 0},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 09, 02), Throughput = 0},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 09, 03), Throughput = 0},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 09, 04), Throughput = 0},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 09, 05), Throughput = 8},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 09, 06), Throughput = 2},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 09, 07), Throughput = 0},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 09, 08), Throughput = 0},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 09, 09), Throughput = 1},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 09, 10), Throughput = 0},
                new ThroughputPoint {CompletedDate = new DateTime(2022, 09, 11), Throughput = 0},
            };

            var actualPoints = Throughput.CalculateThroughputPoints(throughputJson, startDate, endDate);

            Assert.That(actualPoints, Is.EquivalentTo(expectedPoints));
        }


        [Test]
        public void CalculateEmptyThroughput()
        {
            var startDate = new DateTime(2022, 08, 20);
            var endDate = new DateTime(2022, 09, 11);
            var throughputJson = new ThroughputJsonRecord { Value = new List<ThroughputJsonRec>() };
            var expectedPoints = new List<ThroughputPoint>();

            var actualPoints = Throughput.CalculateThroughputPoints(throughputJson, startDate, endDate);

            Assert.That(actualPoints, Is.EquivalentTo(expectedPoints));
        }
    }
}

