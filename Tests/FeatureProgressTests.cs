using System.Linq;
using AgileMetricsRules;

namespace Tests
{
    public class FeatureProgressTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CalculateFeatureChildPointForFeatureChildrenOnePerState()
        {
            var day = new DateTime(2023, 2, 1);
            var featureChildren = new FeatureChildrenJsonRecord
            {
                Value = new List<FeatureChildrenJsonRec>
                {
                    new FeatureChildrenJsonRec { DateSK = 20230201, WorkItemId = 1, State = "New"},
                    new FeatureChildrenJsonRec { DateSK = 20230201, WorkItemId = 2, State = "Old"},
                    new FeatureChildrenJsonRec { DateSK = 20230201, WorkItemId = 3, State = "Active"},
                    new FeatureChildrenJsonRec { DateSK = 20230201, WorkItemId = 4, State = "Resolved"},
                    new FeatureChildrenJsonRec { DateSK = 20230201, WorkItemId = 5, State = "Inactive"},
                    new FeatureChildrenJsonRec { DateSK = 20230201, WorkItemId = 6, State = "Unresolved"},
                    new FeatureChildrenJsonRec { DateSK = 20230201, WorkItemId = 7, State = "Closed"}
                }
            };
            var expectedPoints = new Dictionary<string, List<FeatureChildPoint>>
            {
                {"New",  new List<FeatureChildPoint>{ new FeatureChildPoint { Date = day, Count = 1 } } },
                {"Active",  new List<FeatureChildPoint>{ new FeatureChildPoint { Date = day, Count = 1 } } },
                {"Resolved",  new List<FeatureChildPoint>{ new FeatureChildPoint { Date = day, Count = 1 } } },
                {"Closed",  new List<FeatureChildPoint>{ new FeatureChildPoint { Date = day, Count = 1 } } },
                {"theRest", new List<FeatureChildPoint>{ new FeatureChildPoint { Date = day, Count = 3 } } }
            };

            var actualPoints = FeatureChildren.CalculateChildrenDataPoints(featureChildren);

            Assert.That(actualPoints, Is.EquivalentTo(expectedPoints));
        }

        [Test]
        public void CalculateFeatureChildPointForFeatureChildren12and4States()
        {
            var day = new DateTime(2023, 2, 1);
            var featureChildren = new FeatureChildrenJsonRecord
            {
                Value = new List<FeatureChildrenJsonRec>
                {
                    new FeatureChildrenJsonRec { DateSK = 20230201, WorkItemId = 1, State = "New"},
                    new FeatureChildrenJsonRec { DateSK = 20230201, WorkItemId = 2, State = "Closed"},
                    new FeatureChildrenJsonRec { DateSK = 20230201, WorkItemId = 3, State = "Active"},
                    new FeatureChildrenJsonRec { DateSK = 20230201, WorkItemId = 4, State = "New"},
                    new FeatureChildrenJsonRec { DateSK = 20230201, WorkItemId = 5, State = "Closed"},
                    new FeatureChildrenJsonRec { DateSK = 20230201, WorkItemId = 6, State = "New"},
                    new FeatureChildrenJsonRec { DateSK = 20230201, WorkItemId = 7, State = "New"}
                }
            };
            var expectedPoints = new Dictionary<string, List<FeatureChildPoint>>
            {
                {"New",  new List<FeatureChildPoint>{ new FeatureChildPoint { Date = day, Count = 4 } } },
                {"Active",  new List<FeatureChildPoint>{ new FeatureChildPoint { Date = day, Count = 1 } } },
                {"Closed",  new List<FeatureChildPoint>{ new FeatureChildPoint { Date = day, Count = 2 } } }
            };

            var actualPoints = FeatureChildren.CalculateChildrenDataPoints(featureChildren);

            Assert.That(actualPoints, Is.EquivalentTo(expectedPoints));
        }

        [Test]
        public void CalculateFeatureChildPointForFeatureChildrenNullReturnsEmpty()
        {
            var featureChildren = new FeatureChildrenJsonRecord
            {
                Value = new List<FeatureChildrenJsonRec>()
            };
            var expectedPoints = new Dictionary<string, List<FeatureChildPoint>>();

            var actualPoints = FeatureChildren.CalculateChildrenDataPoints(featureChildren);

            Assert.That(actualPoints, Is.EquivalentTo(expectedPoints));
        }

        [Test]
        public void CalculateFeatureChildPointForFeatureChildrenEmptyReturnsEmpty()
        {
            var featureChildren = new FeatureChildrenJsonRecord
            {
                Value = new List<FeatureChildrenJsonRec>()
            };
            var expectedPoints = new Dictionary<string, List<FeatureChildPoint>>();

            var actualPoints = FeatureChildren.CalculateChildrenDataPoints(featureChildren);

            Assert.That(actualPoints, Is.EquivalentTo(expectedPoints));
        }

        [Test]
        public void CalculateFeatureChildPointForFeatureChildrenOnePerStateMultipleDates()
        {
            var day1 = new DateTime(2023, 2, 1);
            var day2 = new DateTime(2023, 3, 1);
            var featureChildren = new FeatureChildrenJsonRecord
            {
                Value = new List<FeatureChildrenJsonRec>
                {
                    new FeatureChildrenJsonRec { DateSK = 20230201, WorkItemId = 1, State = "New"},
                    new FeatureChildrenJsonRec { DateSK = 20230201, WorkItemId = 2, State = "Old"},
                    new FeatureChildrenJsonRec { DateSK = 20230201, WorkItemId = 3, State = "Active"},
                    new FeatureChildrenJsonRec { DateSK = 20230201, WorkItemId = 4, State = "Resolved"},
                    new FeatureChildrenJsonRec { DateSK = 20230201, WorkItemId = 5, State = "Inactive"},
                    new FeatureChildrenJsonRec { DateSK = 20230201, WorkItemId = 6, State = "Unresolved"},
                    new FeatureChildrenJsonRec { DateSK = 20230201, WorkItemId = 7, State = "Closed"},
                    new FeatureChildrenJsonRec { DateSK = 20230301, WorkItemId = 1, State = "New"},
                    new FeatureChildrenJsonRec { DateSK = 20230301, WorkItemId = 2, State = "Old"},
                    new FeatureChildrenJsonRec { DateSK = 20230301, WorkItemId = 3, State = "Active"},
                    new FeatureChildrenJsonRec { DateSK = 20230301, WorkItemId = 4, State = "Resolved"},
                    new FeatureChildrenJsonRec { DateSK = 20230301, WorkItemId = 5, State = "Inactive"},
                    new FeatureChildrenJsonRec { DateSK = 20230301, WorkItemId = 6, State = "Unresolved"},
                    new FeatureChildrenJsonRec { DateSK = 20230301, WorkItemId = 7, State = "Closed"}
                }
            };
            var expectedPoints = new Dictionary<string, List<FeatureChildPoint>>
            {
                {
                    "New",
                    new List<FeatureChildPoint>
                    {
                        new FeatureChildPoint {Date = day1, Count = 1},
                        new FeatureChildPoint {Date = day2, Count = 1}
                    }
                },
                {
                    "Active",
                    new List<FeatureChildPoint>
                    {
                        new FeatureChildPoint {Date = day1, Count = 1},
                        new FeatureChildPoint {Date = day2, Count = 1}
                    }
                },
                {
                    "Resolved",
                    new List<FeatureChildPoint>
                    {
                        new FeatureChildPoint {Date = day1, Count = 1},
                        new FeatureChildPoint {Date = day2, Count = 1}
                    }
                },
                {
                    "Closed",
                    new List<FeatureChildPoint>
                    {
                        new FeatureChildPoint {Date = day1, Count = 1},
                        new FeatureChildPoint {Date = day2, Count = 1}
                    }
                },
                {
                    "theRest",
                    new List<FeatureChildPoint>
                    {
                        new FeatureChildPoint {Date = day1, Count = 3},
                        new FeatureChildPoint {Date = day2, Count = 3}
                    }
                }
            };

            var actualPoints = FeatureChildren.CalculateChildrenDataPoints(featureChildren);

            Assert.That(actualPoints, Is.EquivalentTo(expectedPoints));
        }

        [Test]
        public void CalculateFeatureChildPointForFeatureChildren12and4StatesMultipleDates()
        {
            var day1 = new DateTime(2023, 2, 1);
            var day2 = new DateTime(2023, 3, 1);
            var day3 = new DateTime(2023, 4, 1);
            var featureChildren = new FeatureChildrenJsonRecord
            {
                Value = new List<FeatureChildrenJsonRec>
                {
                    new FeatureChildrenJsonRec { DateSK = 20230201, WorkItemId = 1, State = "New"},
                    new FeatureChildrenJsonRec { DateSK = 20230201, WorkItemId = 2, State = "Closed"},
                    new FeatureChildrenJsonRec { DateSK = 20230201, WorkItemId = 3, State = "Active"},
                    new FeatureChildrenJsonRec { DateSK = 20230201, WorkItemId = 4, State = "New"},
                    new FeatureChildrenJsonRec { DateSK = 20230201, WorkItemId = 5, State = "Closed"},
                    new FeatureChildrenJsonRec { DateSK = 20230201, WorkItemId = 6, State = "New"},
                    new FeatureChildrenJsonRec { DateSK = 20230201, WorkItemId = 7, State = "New"},
                    new FeatureChildrenJsonRec { DateSK = 20230301, WorkItemId = 1, State = "New"},
                    new FeatureChildrenJsonRec { DateSK = 20230301, WorkItemId = 2, State = "Closed"},
                    new FeatureChildrenJsonRec { DateSK = 20230301, WorkItemId = 3, State = "Active"},
                    new FeatureChildrenJsonRec { DateSK = 20230301, WorkItemId = 4, State = "New"},
                    new FeatureChildrenJsonRec { DateSK = 20230301, WorkItemId = 5, State = "Active"},
                    new FeatureChildrenJsonRec { DateSK = 20230301, WorkItemId = 6, State = "New"},
                    new FeatureChildrenJsonRec { DateSK = 20230301, WorkItemId = 7, State = "New"},
                    new FeatureChildrenJsonRec { DateSK = 20230401, WorkItemId = 1, State = "Closed"},
                    new FeatureChildrenJsonRec { DateSK = 20230401, WorkItemId = 2, State = "New"},
                    new FeatureChildrenJsonRec { DateSK = 20230401, WorkItemId = 3, State = "Active"},
                    new FeatureChildrenJsonRec { DateSK = 20230401, WorkItemId = 4, State = "Closed"},
                    new FeatureChildrenJsonRec { DateSK = 20230401, WorkItemId = 5, State = "Active"},
                    new FeatureChildrenJsonRec { DateSK = 20230401, WorkItemId = 6, State = "Closed"},
                    new FeatureChildrenJsonRec { DateSK = 20230401, WorkItemId = 7, State = "Closed"}
                }
            };
            var expectedPoints = new Dictionary<string, List<FeatureChildPoint>>
            {
                {
                    "New",
                    new List<FeatureChildPoint>
                    {
                        new FeatureChildPoint {Date = day1, Count = 4},
                        new FeatureChildPoint {Date = day2, Count = 4},
                        new FeatureChildPoint {Date = day3, Count = 1}
                    }
                },
                {
                    "Active",
                    new List<FeatureChildPoint>
                    {
                        new FeatureChildPoint {Date = day1, Count = 1},
                        new FeatureChildPoint {Date = day2, Count = 2},
                        new FeatureChildPoint {Date = day3, Count = 2}
                        }
                    },
                {
                    "Closed",
                    new List<FeatureChildPoint>
                    {
                        new FeatureChildPoint {Date = day1, Count = 2},
                        new FeatureChildPoint {Date = day2, Count = 1},
                        new FeatureChildPoint {Date = day3, Count = 4}
                    }
                }
            };

            var actualPoints = FeatureChildren.CalculateChildrenDataPoints(featureChildren);

            Assert.That(actualPoints, Is.EquivalentTo(expectedPoints));
        }
    }
}