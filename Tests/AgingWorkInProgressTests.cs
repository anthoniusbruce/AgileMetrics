using System.IO;
using System.Linq;
using System.Text.Json;
using AgileMetricsRules;

namespace Tests
{
    public class AgingWorkInProgressTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ParseJsonForUserStoryScatterPlot()
        {
            AgingWipJsonRecord? userStoryData = new AgingWipJsonRecord { value = new List<AgingWipJsonRec>() };

            using (FileStream fs = File.Open("CannedData/usAWipOut.json", FileMode.Open))
            {
                userStoryData = JsonSerializer.Deserialize<AgingWipJsonRecord>(fs);
            }

            Assert.NotNull(userStoryData);
            Assert.NotNull(userStoryData.value);
            Assert.That(userStoryData.value.Count, Is.EqualTo(70));
        }

        [Test]
        public void CalculateAgingHappyPath()
        {
            var today = DateTime.Now.Date;
            var age = 10;
            var activeDate = Utility.DateToDateSk(today.AddDays(-age));
            var agingRec = new AgingWipJsonRec { ActivatedDateSK = activeDate, ColumnName = "One", ColumnOrder = 1, IsDone = null, WorkItemId = 1 };
            var expectedResult = age;

            var result = AgingWip.CalculateAging(agingRec, today);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void CalculateAgingPassingToday()
        {
            var today = DateTime.Now.Date;
            var agingRec = new AgingWipJsonRec { ActivatedDateSK = Utility.DateToDateSk(today), ColumnName = "One", ColumnOrder = 1, IsDone = null, WorkItemId = 1 };
            var expectedResult = 0;

            var result = AgingWip.CalculateAging(agingRec, today);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void CalculateAgingPassingTomorrow()
        {
            var today = DateTime.Now.Date;
            var age = -1;
            var activeDate = Utility.DateToDateSk(today.AddDays(-age));
            var agingRec = new AgingWipJsonRec { ActivatedDateSK = activeDate, ColumnName = "One", ColumnOrder = 1, IsDone = null, WorkItemId = 1 };
            var expectedResult = age;

            var result = AgingWip.CalculateAging(agingRec, today);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void GetColumnNameNoDoneSplit()
        {
            var agingRec = new AgingWipJsonRec { ActivatedDateSK = 20230202, ColumnName = "One", ColumnOrder = 1, IsDone = null, WorkItemId = 1 };
            var expectedName = "One";

            var result = AgingWip.GetColumnName(agingRec);

            Assert.That(result, Is.EqualTo(expectedName));
        }

        [Test]
        public void GetColumnNameIsDone()
        {
            var agingRec = new AgingWipJsonRec { ActivatedDateSK = 20230202, ColumnName = "One", ColumnOrder = 1, IsDone = true, WorkItemId = 1 };
            var expectedName = "One - Done";

            var result = AgingWip.GetColumnName(agingRec);

            Assert.That(result, Is.EqualTo(expectedName));
        }

        [Test]
        public void GetColumnNameIsDoing()
        {
            var agingRec = new AgingWipJsonRec { ActivatedDateSK = 20230202, ColumnName = "One", ColumnOrder = 1, IsDone = false, WorkItemId = 1 };
            var expectedName = "One - Doing";

            var result = AgingWip.GetColumnName(agingRec);

            Assert.That(result, Is.EqualTo(expectedName));
        }

        [Test]
        public void CalculateColumnOrderNoDoneSplit()
        {
            var agingRec = new AgingWipJsonRec { ActivatedDateSK = 20230202, ColumnName = "One", ColumnOrder = 1, IsDone = null, WorkItemId = 1 };
            decimal expectedColumnOrder = 1;

            var result = AgingWip.CalculateColumnOrder(agingRec);

            Assert.That(result, Is.EqualTo(expectedColumnOrder));
        }

        [Test]
        public void CalculateColumnOrderIsDoing()
        {
            var agingRec = new AgingWipJsonRec { ActivatedDateSK = 20230202, ColumnName = "One", ColumnOrder = 1, IsDone = false, WorkItemId = 1 };
            decimal expectedColumnOrder = 1;

            var result = AgingWip.CalculateColumnOrder(agingRec);

            Assert.That(result, Is.EqualTo(expectedColumnOrder));
        }

        [Test]
        public void CalculateColumnOrderIsDone()
        {
            var agingRec = new AgingWipJsonRec { ActivatedDateSK = 20230202, ColumnName = "One", ColumnOrder = 1, IsDone = true, WorkItemId = 1 };
            decimal expectedColumnOrder = 1.5m;

            var result = AgingWip.CalculateColumnOrder(agingRec);

            Assert.That(result, Is.EqualTo(expectedColumnOrder));
        }

        [Test]
        public void GatherAllChartData()
        {
            var today = DateTime.Now.Date;
            var agingWip = new List<AgingWipJsonRec>
            {
                new AgingWipJsonRec
                {
                    ActivatedDateSK = Utility.DateToDateSk(today.AddDays(-1)),
                    ColumnName = "One",
                    ColumnOrder = 1,
                    IsDone = null,
                    WorkItemId = 1
                },
                new AgingWipJsonRec
                {
                    ActivatedDateSK = Utility.DateToDateSk(today.AddDays(-1)),
                    ColumnName = "One",
                    ColumnOrder = 1,
                    IsDone = null,
                    WorkItemId = 2
                },
                new AgingWipJsonRec
                {
                    ActivatedDateSK = Utility.DateToDateSk(today.AddDays(-2)),
                    ColumnName = "One",
                    ColumnOrder = 1,
                    IsDone = null,
                    WorkItemId = 3
                },
                new AgingWipJsonRec
                {
                    ActivatedDateSK = Utility.DateToDateSk(today.AddDays(-3)),
                    ColumnName = "Two",
                    ColumnOrder = 2,
                    IsDone = false,
                    WorkItemId = 4
                },
                new AgingWipJsonRec
                {
                    ActivatedDateSK = Utility.DateToDateSk(today.AddDays(-4)),
                    ColumnName = "Two",
                    ColumnOrder = 2,
                    IsDone = false,
                    WorkItemId = 5
                },
                new AgingWipJsonRec
                {
                    ActivatedDateSK = Utility.DateToDateSk(today.AddDays(-3)),
                    ColumnName = "Two",
                    ColumnOrder = 2,
                    IsDone = true,
                    WorkItemId = 6
                },
                new AgingWipJsonRec
                {
                    ActivatedDateSK = Utility.DateToDateSk(today.AddDays(-4)),
                    ColumnName = "Two",
                    ColumnOrder = 2,
                    IsDone = true,
                    WorkItemId = 7
                },
                new AgingWipJsonRec
                {
                    ActivatedDateSK = Utility.DateToDateSk(today.AddDays(-7)),
                    ColumnName = "Three",
                    ColumnOrder = 3,
                    IsDone = false,
                    WorkItemId = 8
                },
                new AgingWipJsonRec
                {
                    ActivatedDateSK = Utility.DateToDateSk(today.AddDays(-8)),
                    ColumnName = "Four",
                    ColumnOrder = 4,
                    IsDone = true,
                    WorkItemId = 9
                },
                new AgingWipJsonRec
                {
                    ActivatedDateSK = Utility.DateToDateSk(today.AddDays(-5)),
                    ColumnName = "One",
                    ColumnOrder = 1,
                    IsDone = null,
                    WorkItemId = 15
                },
            };
            var expected = new AgingWipResults();
            expected.ColumnInfo.Add(1m, "One");
            expected.ColumnInfo.Add(2m, "Two - Doing");
            expected.ColumnInfo.Add(2.5m, "Two - Done");
            expected.ColumnInfo.Add(3m, "Three - Doing");
            expected.ColumnInfo.Add(4.5m, "Four - Done");
            expected.WorkItems.Add(new AgingWipResult { WorkItemId = 1, Age = 1, ColumnOrder = 1 });
            expected.WorkItems.Add(new AgingWipResult { WorkItemId = 2, Age = 1, ColumnOrder = 1 });
            expected.WorkItems.Add(new AgingWipResult { WorkItemId = 3, Age = 2, ColumnOrder = 1 });
            expected.WorkItems.Add(new AgingWipResult { WorkItemId = 4, Age = 3, ColumnOrder = 2 });
            expected.WorkItems.Add(new AgingWipResult { WorkItemId = 5, Age = 4, ColumnOrder = 2 });
            expected.WorkItems.Add(new AgingWipResult { WorkItemId = 6, Age = 3, ColumnOrder = 2.5m });
            expected.WorkItems.Add(new AgingWipResult { WorkItemId = 7, Age = 4, ColumnOrder = 2.5m });
            expected.WorkItems.Add(new AgingWipResult { WorkItemId = 8, Age = 7, ColumnOrder = 3 });
            expected.WorkItems.Add(new AgingWipResult { WorkItemId = 9, Age = 8, ColumnOrder = 4.5m });
            expected.WorkItems.Add(new AgingWipResult { WorkItemId = 15, Age = 5, ColumnOrder = 1 });

            var result = AgingWip.GatherAllChartData(agingWip);

            Assert.That(result.WorkItems, Is.EquivalentTo(expected.WorkItems));
            Assert.That(result.ColumnInfo, Is.EquivalentTo(expected.ColumnInfo));
        }
    }
}