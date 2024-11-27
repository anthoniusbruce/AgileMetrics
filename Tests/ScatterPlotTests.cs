using System.Linq;
using AgileMetricsRules;

namespace Tests
{
    public class ScatterPlotTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ParseJsonForUserStoryScatterPlot()
        {
            var userStoryData = new ScatterPlotJsonRecord { Value = new List<ScatterPlotJsonRec>() };

            using (FileStream fs = File.Open("CannedData/usCtOut.json", FileMode.Open))
            {
                userStoryData = ScatterPlot.ParseJsonStream(fs);
            }

            Assert.NotNull(userStoryData);
            Assert.NotNull(userStoryData.Value);
            Assert.That(userStoryData.Value.Count, Is.EqualTo(38));
        }

        [Test]
        public void ParseJsonForFeatureScatterPlot()
        {
            var userStoryData = new ScatterPlotJsonRecord { Value = new List<ScatterPlotJsonRec>() };

            using (FileStream fs = File.Open("CannedData/fCtOut.json", FileMode.Open))
            {
                userStoryData = ScatterPlot.ParseJsonStream(fs);
            }

            Assert.NotNull(userStoryData);
            Assert.NotNull(userStoryData.Value);
            Assert.That(userStoryData.Value.Count, Is.EqualTo(32));
        }

        [Test]
        public void CalculateCycleTimesForScatterPlot()
        {
            var scatterPlotJson = new ScatterPlotJsonRecord
            {
                Value = new List<ScatterPlotJsonRec>
                {
                    new ScatterPlotJsonRec { WorkItemId = 1, ActivatedDate = "2022-05-09T17:26:35.007Z", CompletedDate = "2022-08-23T17:23:36.323Z"},
                    new ScatterPlotJsonRec { WorkItemId = 2, ActivatedDate = "2022-07-11T17:31:45.06Z", CompletedDate = "2022-08-23T17:23:36.323Z"},
                    new ScatterPlotJsonRec { WorkItemId = 3, ActivatedDate = "2022-06-03T16:08:13.837Z", CompletedDate = "2022-08-23T17:23:36.323Z"},
                    new ScatterPlotJsonRec { WorkItemId = 4, ActivatedDate = "2022-06-03T14:01:58.107Z", CompletedDate = "2022-08-23T17:23:36.323Z"},
                    new ScatterPlotJsonRec { WorkItemId = 5, ActivatedDate = "2022-07-08T15:45:14.457Z", CompletedDate = "2022-08-23T17:23:36.323Z"},
                    new ScatterPlotJsonRec { WorkItemId = 6, ActivatedDate = "2022-08-16T19:58:36.213Z", CompletedDate = "2022-08-23T17:23:36.323Z"},
                    new ScatterPlotJsonRec { WorkItemId = 7, ActivatedDate = "2022-08-16T19:58:34.337Z", CompletedDate = "2022-08-23T17:23:36.323Z"}
                }
            };
            var expectedPoints = new List<ScatterPlotPoint>
            {
                new ScatterPlotPoint {WorkItemId = "1", CompletedDate = new DateTime(2022, 08, 23), CycleTime = 106},
                new ScatterPlotPoint {WorkItemId = "2", CompletedDate = new DateTime(2022, 08, 23), CycleTime = 43},
                new ScatterPlotPoint {WorkItemId = "3", CompletedDate = new DateTime(2022, 08, 23), CycleTime = 81},
                new ScatterPlotPoint {WorkItemId = "4", CompletedDate = new DateTime(2022, 08, 23), CycleTime = 81},
                new ScatterPlotPoint {WorkItemId = "5", CompletedDate = new DateTime(2022, 08, 23), CycleTime = 46},
                new ScatterPlotPoint {WorkItemId = "6", CompletedDate = new DateTime(2022, 08, 23), CycleTime = 7},
                new ScatterPlotPoint {WorkItemId = "7", CompletedDate = new DateTime(2022, 08, 23), CycleTime = 7},
            };

            var actualPoints = ScatterPlot.CalculateCycleTimeScatterPlotPoints(scatterPlotJson);

            Assert.That(actualPoints, Is.EquivalentTo(expectedPoints));
        }

        [Test]
        public void CalculateEmptyCycleTimesForScatterPlot()
        {
            var scatterPlotJson = new ScatterPlotJsonRecord { Value = new List<ScatterPlotJsonRec>() };
            var expectedPoints = new List<ScatterPlotPoint>();

            var actualPoints = ScatterPlot.CalculateCycleTimeScatterPlotPoints(scatterPlotJson);

            Assert.That(actualPoints, Is.EquivalentTo(expectedPoints));
        }

        [Test]
        public void CalculatePercentilesForScatterPlot()
        {
            var points = new List<ScatterPlotPoint>
            {
                new ScatterPlotPoint {WorkItemId = "1", CompletedDate = new DateTime(2022, 08, 23), CycleTime = 106},
                new ScatterPlotPoint {WorkItemId = "2", CompletedDate = new DateTime(2022, 08, 23), CycleTime = 43},
                new ScatterPlotPoint {WorkItemId = "3", CompletedDate = new DateTime(2022, 08, 23), CycleTime = 81},
                new ScatterPlotPoint {WorkItemId = "4", CompletedDate = new DateTime(2022, 08, 23), CycleTime = 81},
                new ScatterPlotPoint {WorkItemId = "5", CompletedDate = new DateTime(2022, 08, 23), CycleTime = 46},
                new ScatterPlotPoint {WorkItemId = "6", CompletedDate = new DateTime(2022, 08, 23), CycleTime = 7},
                new ScatterPlotPoint {WorkItemId = "7", CompletedDate = new DateTime(2022, 08, 23), CycleTime = 7},
            };
            var expectedPercentiles = new Percentiles { Thirtieth = 7, Fiftieth = 46, Seventieth = 81, EightyFifth = 81, NinetyFifth = 106 };

            var actualPercentiles = ScatterPlot.CalculateCycleTimePercentiles(points);

            Assert.That(actualPercentiles.Thirtieth, Is.EqualTo(expectedPercentiles.Thirtieth));
            Assert.That(actualPercentiles.Fiftieth, Is.EqualTo(expectedPercentiles.Fiftieth));
            Assert.That(actualPercentiles.Seventieth, Is.EqualTo(expectedPercentiles.Seventieth));
            Assert.That(actualPercentiles.EightyFifth, Is.EqualTo(expectedPercentiles.EightyFifth));
            Assert.That(actualPercentiles.NinetyFifth, Is.EqualTo(expectedPercentiles.NinetyFifth));
        }

        [Test]
        public void CalculatePercentilesForScatterPlotEmptyList()
        {
            var points = new List<ScatterPlotPoint>();
            var expectedPercentiles = new Percentiles();

            var actualPercentiles = ScatterPlot.CalculateCycleTimePercentiles(points);

            Assert.That(actualPercentiles.Thirtieth, Is.EqualTo(expectedPercentiles.Thirtieth));
            Assert.That(actualPercentiles.Fiftieth, Is.EqualTo(expectedPercentiles.Fiftieth));
            Assert.That(actualPercentiles.Seventieth, Is.EqualTo(expectedPercentiles.Seventieth));
            Assert.That(actualPercentiles.EightyFifth, Is.EqualTo(expectedPercentiles.EightyFifth));
            Assert.That(actualPercentiles.NinetyFifth, Is.EqualTo(expectedPercentiles.NinetyFifth));
        }

        [Test]
        public void CalculatePercentilesForScatterPlotListOfOne()
        {
            var points = new List<ScatterPlotPoint>
            {
                new ScatterPlotPoint {WorkItemId = "1", CompletedDate = new DateTime(2022, 08, 23), CycleTime = 106},
            };
            var expectedPercentiles = new Percentiles { Thirtieth = 106, Fiftieth = 106, Seventieth = 106, EightyFifth = 106, NinetyFifth = 106 };

            var actualPercentiles = ScatterPlot.CalculateCycleTimePercentiles(points);

            Assert.That(actualPercentiles.Thirtieth, Is.EqualTo(expectedPercentiles.Thirtieth));
            Assert.That(actualPercentiles.Fiftieth, Is.EqualTo(expectedPercentiles.Fiftieth));
            Assert.That(actualPercentiles.Seventieth, Is.EqualTo(expectedPercentiles.Seventieth));
            Assert.That(actualPercentiles.EightyFifth, Is.EqualTo(expectedPercentiles.EightyFifth));
            Assert.That(actualPercentiles.NinetyFifth, Is.EqualTo(expectedPercentiles.NinetyFifth));
        }

        [Test]
        public void CalculatePercentilesForListOfScatterPlots()
        {
            var today = DateTime.Today;
            var scatterPlotJson = new Dictionary<DateTime, ScatterPlotJsonRecord>
            {
                {
                    today.AddDays(-2),
                    new ScatterPlotJsonRecord
                    {
                        Value = new List<ScatterPlotJsonRec>
                        {
                            new ScatterPlotJsonRec { WorkItemId = 1, ActivatedDate = "2022-05-09T17:26:35.007Z", CompletedDate = "2022-08-23T17:23:36.323Z"},
                            new ScatterPlotJsonRec { WorkItemId = 2, ActivatedDate = "2022-07-11T17:31:45.06Z", CompletedDate = "2022-08-23T17:23:36.323Z"},
                            new ScatterPlotJsonRec { WorkItemId = 3, ActivatedDate = "2022-06-03T16:08:13.837Z", CompletedDate = "2022-08-23T17:23:36.323Z"},
                            new ScatterPlotJsonRec { WorkItemId = 4, ActivatedDate = "2022-06-03T14:01:58.107Z", CompletedDate = "2022-08-23T17:23:36.323Z"},
                            new ScatterPlotJsonRec { WorkItemId = 5, ActivatedDate = "2022-07-08T15:45:14.457Z", CompletedDate = "2022-08-23T17:23:36.323Z"},
                            new ScatterPlotJsonRec { WorkItemId = 6, ActivatedDate = "2022-08-16T19:58:36.213Z", CompletedDate = "2022-08-23T17:23:36.323Z"},
                            new ScatterPlotJsonRec { WorkItemId = 7, ActivatedDate = "2022-08-16T19:58:34.337Z", CompletedDate = "2022-08-23T17:23:36.323Z"}
                        }
                    }
                },
                {
                    today.AddDays(-1),
                    new ScatterPlotJsonRecord
                    {
                        Value = new List<ScatterPlotJsonRec>
                        {
                            new ScatterPlotJsonRec { WorkItemId = 1, ActivatedDate = "2022-05-14T17:26:35.007Z", CompletedDate = "2022-08-23T17:23:36.323Z"},
                            new ScatterPlotJsonRec { WorkItemId = 2, ActivatedDate = "2022-07-16T17:31:45.06Z", CompletedDate = "2022-08-23T17:23:36.323Z"},
                            new ScatterPlotJsonRec { WorkItemId = 3, ActivatedDate = "2022-06-08T16:08:13.837Z", CompletedDate = "2022-08-23T17:23:36.323Z"},
                            new ScatterPlotJsonRec { WorkItemId = 4, ActivatedDate = "2022-06-08T14:01:58.107Z", CompletedDate = "2022-08-23T17:23:36.323Z"},
                            new ScatterPlotJsonRec { WorkItemId = 5, ActivatedDate = "2022-07-13T15:45:14.457Z", CompletedDate = "2022-08-23T17:23:36.323Z"},
                            new ScatterPlotJsonRec { WorkItemId = 6, ActivatedDate = "2022-08-21T19:58:36.213Z", CompletedDate = "2022-08-23T17:23:36.323Z"},
                            new ScatterPlotJsonRec { WorkItemId = 7, ActivatedDate = "2022-08-21T19:58:34.337Z", CompletedDate = "2022-08-23T17:23:36.323Z"}
                        }
                    }
                },
                {
                    today,
                    new ScatterPlotJsonRecord
                    {
                        Value = new List<ScatterPlotJsonRec>
                        {
                            new ScatterPlotJsonRec { WorkItemId = 1, ActivatedDate = "2022-05-15T17:26:35.007Z", CompletedDate = "2022-08-23T17:23:36.323Z"},
                            new ScatterPlotJsonRec { WorkItemId = 2, ActivatedDate = "2022-07-17T17:31:45.06Z", CompletedDate = "2022-08-23T17:23:36.323Z"},
                            new ScatterPlotJsonRec { WorkItemId = 3, ActivatedDate = "2022-06-09T16:08:13.837Z", CompletedDate = "2022-08-23T17:23:36.323Z"},
                            new ScatterPlotJsonRec { WorkItemId = 4, ActivatedDate = "2022-06-09T14:01:58.107Z", CompletedDate = "2022-08-23T17:23:36.323Z"},
                            new ScatterPlotJsonRec { WorkItemId = 5, ActivatedDate = "2022-07-14T15:45:14.457Z", CompletedDate = "2022-08-23T17:23:36.323Z"},
                            new ScatterPlotJsonRec { WorkItemId = 6, ActivatedDate = "2022-08-22T19:58:36.213Z", CompletedDate = "2022-08-23T17:23:36.323Z"},
                            new ScatterPlotJsonRec { WorkItemId = 7, ActivatedDate = "2022-08-22T19:58:34.337Z", CompletedDate = "2022-08-23T17:23:36.323Z"}
                        }
                    }
                },
            };
            var expectedPercentiles = new Dictionary<DateTime, Percentiles>
            {
                {
                    today.AddDays(-2),
                    new Percentiles {Thirtieth = 7, Fiftieth = 46, Seventieth = 81, EightyFifth = 81, NinetyFifth = 106 }
                },
                {
                    today.AddDays(-1),
                    new Percentiles {Thirtieth = 2, Fiftieth = 41, Seventieth = 76, EightyFifth = 76, NinetyFifth = 101 }
                },
                {
                    today,
                    new Percentiles {Thirtieth = 1, Fiftieth = 40, Seventieth = 75, EightyFifth = 75, NinetyFifth = 100 }
                }
            };

            var actualPercentiles = ScatterPlot.CalculateCycleTimePercentilesSeries(scatterPlotJson);

            Assert.That(actualPercentiles[today.AddDays(-2)].Thirtieth, Is.EqualTo(expectedPercentiles[today.AddDays(-2)].Thirtieth));
            Assert.That(actualPercentiles[today.AddDays(-2)].Fiftieth, Is.EqualTo(expectedPercentiles[today.AddDays(-2)].Fiftieth));
            Assert.That(actualPercentiles[today.AddDays(-2)].Seventieth, Is.EqualTo(expectedPercentiles[today.AddDays(-2)].Seventieth));
            Assert.That(actualPercentiles[today.AddDays(-2)].EightyFifth, Is.EqualTo(expectedPercentiles[today.AddDays(-2)].EightyFifth));
            Assert.That(actualPercentiles[today.AddDays(-2)].NinetyFifth, Is.EqualTo(expectedPercentiles[today.AddDays(-2)].NinetyFifth));

            Assert.That(actualPercentiles[today.AddDays(-1)].Thirtieth, Is.EqualTo(expectedPercentiles[today.AddDays(-1)].Thirtieth));
            Assert.That(actualPercentiles[today.AddDays(-1)].Fiftieth, Is.EqualTo(expectedPercentiles[today.AddDays(-1)].Fiftieth));
            Assert.That(actualPercentiles[today.AddDays(-1)].Seventieth, Is.EqualTo(expectedPercentiles[today.AddDays(-1)].Seventieth));
            Assert.That(actualPercentiles[today.AddDays(-1)].EightyFifth, Is.EqualTo(expectedPercentiles[today.AddDays(-1)].EightyFifth));
            Assert.That(actualPercentiles[today.AddDays(-1)].NinetyFifth, Is.EqualTo(expectedPercentiles[today.AddDays(-1)].NinetyFifth));

            Assert.That(actualPercentiles[today].Thirtieth, Is.EqualTo(expectedPercentiles[today].Thirtieth));
            Assert.That(actualPercentiles[today].Fiftieth, Is.EqualTo(expectedPercentiles[today].Fiftieth));
            Assert.That(actualPercentiles[today].Seventieth, Is.EqualTo(expectedPercentiles[today].Seventieth));
            Assert.That(actualPercentiles[today].EightyFifth, Is.EqualTo(expectedPercentiles[today].EightyFifth));
            Assert.That(actualPercentiles[today].NinetyFifth, Is.EqualTo(expectedPercentiles[today].NinetyFifth));
        }
    }
}