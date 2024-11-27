using System.Linq;
using System.Xml.Linq;
using AgileMetricsRules;

namespace Tests
{
    public class CumulativeFlowTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CreateColumnMap()
        {
            var columnData = new ColumnJsonRecord { Value = new List<ColumnJsonRec>()};
            columnData.Value = new List<ColumnJsonRec>
            {
                new ColumnJsonRec
                {
                    IsDone = null,
                    Done = "Unknown",
                    ColumnName = "Backlog",
                    ColumnId = "76edda43-7397-417f-9b6b-b01c6dc35cc3",
                    ColumnOrder = 0
                },
                new ColumnJsonRec
                {
                    IsDone = null,
                    Done = "Unknown",
                    ColumnName = "Research",
                    ColumnId = "31caa191-b96f-4697-bc90-7de08cf73b3e",
                    ColumnOrder = 1
                },
                new ColumnJsonRec
                {
                    IsDone = null,
                    Done = "Unknown",
                    ColumnName = "Blocked",
                    ColumnId = "4be22af9-6cec-4461-bea1-596f21e09f20",
                    ColumnOrder = 2
                },
                new ColumnJsonRec
                {
                    IsDone = false,
                    Done = "Doing",
                    ColumnName = "In Progress",
                    ColumnId = "c2d8e6ff-f237-4ce5-b8d2-a3710c3cd13f",
                    ColumnOrder = 3
                },
                new ColumnJsonRec
                {
                    IsDone = true,
                    Done = "Done",
                    ColumnName = "In Progress",
                    ColumnId = "c2d8e6ff-f237-4ce5-b8d2-a3710c3cd13f",
                    ColumnOrder = 3
                },
                new ColumnJsonRec
                {
                    IsDone = null,
                    Done = "Unknown",
                    ColumnName = "In Progress",
                    ColumnId = "c2d8e6ff-f237-4ce5-b8d2-a3710c3cd13f",
                    ColumnOrder = 3
                },
                new ColumnJsonRec
                {
                    IsDone = null,
                    Done = "Unknown",
                    ColumnName = "Dev Testing",
                    ColumnId = "8ed5387f-f834-4cdb-9f50-a0c7bbc5c5e1",
                    ColumnOrder = 4
                },
                new ColumnJsonRec
                {
                    IsDone = null,
                    Done = "Unknown",
                    ColumnName = "Test Returned",
                    ColumnId = "e5a7ba82-d952-488d-ab17-3943ba224b91",
                    ColumnOrder = 5
                }
            };
            var expected = new Dictionary<string, ColumnResultRecord>
            {
                {"76edda43-7397-417f-9b6b-b01c6dc35cc3", new ColumnResultRecord("Backlog", 0, string.Empty, string.Empty) },
                {"31caa191-b96f-4697-bc90-7de08cf73b3e", new ColumnResultRecord("Research", 1, string.Empty, string.Empty) },
                {"4be22af9-6cec-4461-bea1-596f21e09f20", new ColumnResultRecord("Blocked", 2, string.Empty, string.Empty) },
                {"c2d8e6ff-f237-4ce5-b8d2-a3710c3cd13f", new ColumnResultRecord("In Progress", 3, "Doing", "Done") },
                {"8ed5387f-f834-4cdb-9f50-a0c7bbc5c5e1", new ColumnResultRecord("Dev Testing", 4, string.Empty, string.Empty) },
                {"e5a7ba82-d952-488d-ab17-3943ba224b91", new ColumnResultRecord("Test Returned", 5, string.Empty, string.Empty) },
            };

            var actual = CumulativeFlow.CreateColumnMap(columnData);

            foreach (var item in actual.Keys)
            {
                Assert.That(actual[item].ColumnName, Is.EqualTo(expected[item].ColumnName));
                Assert.That(actual[item].ColumnOrder, Is.EqualTo(expected[item].ColumnOrder));
                Assert.That(actual[item].Doing, Is.EqualTo(expected[item].Doing));
                Assert.That(actual[item].Done, Is.EqualTo(expected[item].Done));
            }

            foreach (var item in expected.Keys)
            {
                Assert.That(actual[item].ColumnName, Is.EqualTo(expected[item].ColumnName));
                Assert.That(actual[item].ColumnOrder, Is.EqualTo(expected[item].ColumnOrder));
                Assert.That(actual[item].Doing, Is.EqualTo(expected[item].Doing));
                Assert.That(actual[item].Done, Is.EqualTo(expected[item].Done));
            }
        }

        [Test]
        public void CreateColumnMapNoColumnData()
        {
            var columnData = new ColumnJsonRecord { Value = new List<ColumnJsonRec>() };
            columnData.Value = new List<ColumnJsonRec>();

            var actual = CumulativeFlow.CreateColumnMap(columnData);

            Assert.IsEmpty(actual);
        }

        [Test]
        public void CreateColumnMapNullColumnData()
        {
            var columnData = new ColumnJsonRecord { Value = new List<ColumnJsonRec>() };

            var actual = CumulativeFlow.CreateColumnMap(columnData);

            Assert.IsEmpty(actual);
        }

        [Test]
        public void ArrangeCumulativeFlowData()
        {
            var columnData = new Dictionary<string, ColumnResultRecord>
            {
                {"76edda43-7397-417f-9b6b-b01c6dc35cc3", new ColumnResultRecord("Backlog", 0, string.Empty, string.Empty) },
                {"4be22af9-6cec-4461-bea1-596f21e09f20", new ColumnResultRecord("Blocked", 1, string.Empty, string.Empty) },
                {"c2d8e6ff-f237-4ce5-b8d2-a3710c3cd13f", new ColumnResultRecord("In Progress", 2, "Doing", "Done") },
                {"8ed5387f-f834-4cdb-9f50-a0c7bbc5c5e1", new ColumnResultRecord("Dev Testing", 3, string.Empty, string.Empty) },
                {"31caa191-b96f-4697-bc90-7de08cf73b3e", new ColumnResultRecord("Closed", 4, string.Empty, string.Empty) },
            };
            var cumulativeFlowJson = new CumulativeFlowJsonRecord { Value = new List<CumulativeFlowJsonRec>() };
            cumulativeFlowJson.Value = new List<CumulativeFlowJsonRec>
            {
                new CumulativeFlowJsonRec { IsDone = false, ColumnId = "c2d8e6ff-f237-4ce5-b8d2-a3710c3cd13f", DateSK = 20230321,WorkItemId = 2835058 },
                new CumulativeFlowJsonRec { IsDone = true, ColumnId = "c2d8e6ff-f237-4ce5-b8d2-a3710c3cd13f", DateSK = 20230322, WorkItemId = 2835058 },
                new CumulativeFlowJsonRec { IsDone = null,ColumnId = "4be22af9-6cec-4461-bea1-596f21e09f20",DateSK = 20230320,WorkItemId = 2841795},
                new CumulativeFlowJsonRec { IsDone = null,ColumnId = "4be22af9-6cec-4461-bea1-596f21e09f20",  DateSK = 20230321,WorkItemId = 2841795},
                new CumulativeFlowJsonRec { IsDone = null,ColumnId = "4be22af9-6cec-4461-bea1-596f21e09f20",DateSK = 20230322,WorkItemId = 2841795},
                new CumulativeFlowJsonRec { IsDone = true,ColumnId = "c2d8e6ff-f237-4ce5-b8d2-a3710c3cd13f",DateSK = 20230320,WorkItemId = 2868999},
                new CumulativeFlowJsonRec { IsDone = true,ColumnId = "c2d8e6ff-f237-4ce5-b8d2-a3710c3cd13f",DateSK = 20230321,WorkItemId = 2868999},
                new CumulativeFlowJsonRec { IsDone = true,ColumnId = "c2d8e6ff-f237-4ce5-b8d2-a3710c3cd13f",DateSK = 20230322,WorkItemId = 2868999},
                new CumulativeFlowJsonRec { IsDone = null,ColumnId = "4be22af9-6cec-4461-bea1-596f21e09f20",DateSK = 20230320,WorkItemId = 2869008},
                new CumulativeFlowJsonRec { IsDone = null,ColumnId = "4be22af9-6cec-4461-bea1-596f21e09f20",DateSK = 20230321,WorkItemId = 2869008},
                new CumulativeFlowJsonRec { IsDone = null,ColumnId = "4be22af9-6cec-4461-bea1-596f21e09f20",DateSK = 20230322,WorkItemId = 2869008},
                new CumulativeFlowJsonRec { IsDone = null,ColumnId = "4be22af9-6cec-4461-bea1-596f21e09f20",DateSK = 20230320,WorkItemId = 2869070},
                new CumulativeFlowJsonRec { IsDone = null,ColumnId = "4be22af9-6cec-4461-bea1-596f21e09f20",DateSK = 20230321,WorkItemId = 2869070},
                new CumulativeFlowJsonRec {IsDone = null,ColumnId = "4be22af9-6cec-4461-bea1-596f21e09f20",DateSK = 20230322,WorkItemId = 2869070},
                new CumulativeFlowJsonRec {IsDone = true,ColumnId = "c2d8e6ff-f237-4ce5-b8d2-a3710c3cd13f",DateSK = 20230320,WorkItemId = 2884111},
                new CumulativeFlowJsonRec {IsDone = true,ColumnId = "c2d8e6ff-f237-4ce5-b8d2-a3710c3cd13f",DateSK = 20230321,WorkItemId = 2884111},
                new CumulativeFlowJsonRec {IsDone = true,ColumnId = "c2d8e6ff-f237-4ce5-b8d2-a3710c3cd13f",DateSK = 20230322,WorkItemId = 2884111},
                new CumulativeFlowJsonRec {IsDone = null,ColumnId = "4be22af9-6cec-4461-bea1-596f21e09f20",DateSK = 20230320,WorkItemId = 2950621},
                new CumulativeFlowJsonRec {IsDone = null,ColumnId = "4be22af9-6cec-4461-bea1-596f21e09f20",DateSK = 20230321,WorkItemId = 2950621},
                new CumulativeFlowJsonRec {IsDone = null,ColumnId = "4be22af9-6cec-4461-bea1-596f21e09f20",DateSK = 20230322,WorkItemId = 2950621},
                new CumulativeFlowJsonRec {IsDone = false,ColumnId = "c2d8e6ff-f237-4ce5-b8d2-a3710c3cd13f",DateSK = 20230321,WorkItemId = 2958002},
                new CumulativeFlowJsonRec {IsDone = true,ColumnId = "c2d8e6ff-f237-4ce5-b8d2-a3710c3cd13f",DateSK = 20230322,WorkItemId = 2958002},
                new CumulativeFlowJsonRec {IsDone = null,ColumnId = "4be22af9-6cec-4461-bea1-596f21e09f20",DateSK = 20230320,WorkItemId = 2961605},
                new CumulativeFlowJsonRec {IsDone = null,ColumnId = "4be22af9-6cec-4461-bea1-596f21e09f20",DateSK = 20230321,WorkItemId = 2961605},
                new CumulativeFlowJsonRec {IsDone = null,ColumnId = "4be22af9-6cec-4461-bea1-596f21e09f20",DateSK = 20230322,WorkItemId = 2961605},
                new CumulativeFlowJsonRec {IsDone = null,ColumnId = "8ed5387f-f834-4cdb-9f50-a0c7bbc5c5e1",DateSK = 20230320,WorkItemId = 2961899},
                new CumulativeFlowJsonRec {IsDone = null,ColumnId = "8ed5387f-f834-4cdb-9f50-a0c7bbc5c5e1",DateSK = 20230321,WorkItemId = 2961899},
                new CumulativeFlowJsonRec {IsDone = null,ColumnId = "8ed5387f-f834-4cdb-9f50-a0c7bbc5c5e1",DateSK = 20230322,WorkItemId = 2961899},
                new CumulativeFlowJsonRec {IsDone = false,ColumnId = "c2d8e6ff-f237-4ce5-b8d2-a3710c3cd13f",DateSK = 20230320,WorkItemId = 2968228},
                new CumulativeFlowJsonRec {IsDone = true,ColumnId = "c2d8e6ff-f237-4ce5-b8d2-a3710c3cd13f",DateSK = 20230321,WorkItemId = 2968228},
                new CumulativeFlowJsonRec {IsDone = null,ColumnId = "31caa191-b96f-4697-bc90-7de08cf73b3e",DateSK = 20230320,WorkItemId = 2973718},
                new CumulativeFlowJsonRec {IsDone = null,ColumnId = "31caa191-b96f-4697-bc90-7de08cf73b3e",DateSK = 20230321,WorkItemId = 2973718},
                new CumulativeFlowJsonRec {IsDone = null,ColumnId = "31caa191-b96f-4697-bc90-7de08cf73b3e",DateSK = 20230322,WorkItemId = 2973718},
                new CumulativeFlowJsonRec {IsDone = null,ColumnId = "4be22af9-6cec-4461-bea1-596f21e09f20",DateSK = 20230320,WorkItemId = 2973752},
                new CumulativeFlowJsonRec {IsDone = null,ColumnId = "4be22af9-6cec-4461-bea1-596f21e09f20",DateSK = 20230321,WorkItemId = 2973752},
                new CumulativeFlowJsonRec {IsDone = null,ColumnId = "4be22af9-6cec-4461-bea1-596f21e09f20",DateSK = 20230322,WorkItemId = 2973752},
                new CumulativeFlowJsonRec {IsDone = null,ColumnId = "4be22af9-6cec-4461-bea1-596f21e09f20",DateSK = 20230320,WorkItemId = 2980982},
                new CumulativeFlowJsonRec {IsDone = null,ColumnId = "4be22af9-6cec-4461-bea1-596f21e09f20",DateSK = 20230321,WorkItemId = 2980982},
                new CumulativeFlowJsonRec {IsDone = null,ColumnId = "4be22af9-6cec-4461-bea1-596f21e09f20",DateSK = 20230322,WorkItemId = 2980982},
                new CumulativeFlowJsonRec {IsDone = false,ColumnId = "c2d8e6ff-f237-4ce5-b8d2-a3710c3cd13f",DateSK = 20230320,WorkItemId = 2990008},
                new CumulativeFlowJsonRec {IsDone = false,ColumnId = "c2d8e6ff-f237-4ce5-b8d2-a3710c3cd13f",DateSK = 20230321,WorkItemId = 2990008},
                new CumulativeFlowJsonRec {IsDone = false,ColumnId = "c2d8e6ff-f237-4ce5-b8d2-a3710c3cd13f",DateSK = 20230322,WorkItemId = 2990008},
            };
            var expected = new Dictionary<string, List<CumulativeFlowResultRecord>>
            {
                {
                    "Blocked",
                    new List<CumulativeFlowResultRecord>
                    {
                        new CumulativeFlowResultRecord { WorkItemId = 2841795, Date = new DateTime(2023, 3, 20)},
                        new CumulativeFlowResultRecord { WorkItemId = 2841795, Date = new DateTime(2023, 3, 21)},
                        new CumulativeFlowResultRecord { WorkItemId = 2841795, Date = new DateTime(2023, 3, 22)},
                        new CumulativeFlowResultRecord { WorkItemId = 2869008, Date = new DateTime(2023, 3, 20)},
                        new CumulativeFlowResultRecord { WorkItemId = 2869008, Date = new DateTime(2023, 3, 21)},
                        new CumulativeFlowResultRecord { WorkItemId = 2869008, Date = new DateTime(2023, 3, 22)},
                        new CumulativeFlowResultRecord { WorkItemId = 2869070, Date = new DateTime(2023, 3, 20)},
                        new CumulativeFlowResultRecord { WorkItemId = 2869070, Date = new DateTime(2023, 3, 21)},
                        new CumulativeFlowResultRecord { WorkItemId = 2869070, Date = new DateTime(2023, 3, 22)},
                        new CumulativeFlowResultRecord { WorkItemId = 2950621, Date = new DateTime(2023, 3, 20)},
                        new CumulativeFlowResultRecord { WorkItemId = 2950621, Date = new DateTime(2023, 3, 21)},
                        new CumulativeFlowResultRecord { WorkItemId = 2950621, Date = new DateTime(2023, 3, 22)},
                        new CumulativeFlowResultRecord { WorkItemId = 2961605, Date = new DateTime(2023, 3, 20)},
                        new CumulativeFlowResultRecord { WorkItemId = 2961605, Date = new DateTime(2023, 3, 21)},
                        new CumulativeFlowResultRecord { WorkItemId = 2961605, Date = new DateTime(2023, 3, 22)},
                        new CumulativeFlowResultRecord { WorkItemId = 2973752, Date = new DateTime(2023, 3, 20)},
                        new CumulativeFlowResultRecord { WorkItemId = 2973752, Date = new DateTime(2023, 3, 21)},
                        new CumulativeFlowResultRecord { WorkItemId = 2973752, Date = new DateTime(2023, 3, 22)},
                        new CumulativeFlowResultRecord { WorkItemId = 2980982, Date = new DateTime(2023, 3, 20)},
                        new CumulativeFlowResultRecord { WorkItemId = 2980982, Date = new DateTime(2023, 3, 21)},
                        new CumulativeFlowResultRecord { WorkItemId = 2980982, Date = new DateTime(2023, 3, 22)}
                    }
                },
                {
                    "In Progress - Doing",
                    new List<CumulativeFlowResultRecord>
                    {
                        new CumulativeFlowResultRecord { WorkItemId = 2835058, Date = new DateTime(2023, 3, 21)},
                        new CumulativeFlowResultRecord { WorkItemId = 2958002, Date = new DateTime(2023, 3, 21)},
                        new CumulativeFlowResultRecord { WorkItemId = 2968228, Date = new DateTime(2023, 3, 20)},
                        new CumulativeFlowResultRecord { WorkItemId = 2990008, Date = new DateTime(2023, 3, 20)},
                        new CumulativeFlowResultRecord { WorkItemId = 2990008, Date = new DateTime(2023, 3, 21)},
                        new CumulativeFlowResultRecord { WorkItemId = 2990008, Date = new DateTime(2023, 3, 22)},
                    }
                },
                {
                    "In Progress - Done",
                    new List<CumulativeFlowResultRecord>
                    {
                        new CumulativeFlowResultRecord { WorkItemId = 2835058, Date = new DateTime(2023, 3, 22)},
                        new CumulativeFlowResultRecord { WorkItemId = 2868999, Date = new DateTime(2023, 3, 20)},
                        new CumulativeFlowResultRecord { WorkItemId = 2868999, Date = new DateTime(2023, 3, 21)},
                        new CumulativeFlowResultRecord { WorkItemId = 2868999, Date = new DateTime(2023, 3, 22)},
                        new CumulativeFlowResultRecord { WorkItemId = 2884111, Date = new DateTime(2023, 3, 20)},
                        new CumulativeFlowResultRecord { WorkItemId = 2884111, Date = new DateTime(2023, 3, 21)},
                        new CumulativeFlowResultRecord { WorkItemId = 2884111, Date = new DateTime(2023, 3, 22)},
                        new CumulativeFlowResultRecord { WorkItemId = 2958002, Date = new DateTime(2023, 3, 22)},
                        new CumulativeFlowResultRecord { WorkItemId = 2968228, Date = new DateTime(2023, 3, 21)},
                    }
                },
                {
                    "Dev Testing",
                    new List<CumulativeFlowResultRecord>
                    {
                        new CumulativeFlowResultRecord { WorkItemId = 2961899, Date = new DateTime(2023, 3, 20)},
                        new CumulativeFlowResultRecord { WorkItemId = 2961899, Date = new DateTime(2023, 3, 21)},
                        new CumulativeFlowResultRecord { WorkItemId = 2961899, Date = new DateTime(2023, 3, 22)},
                    }
                },
                {
                    "Closed",
                    new List<CumulativeFlowResultRecord>
                    {
                        new CumulativeFlowResultRecord { WorkItemId = 2973718, Date = new DateTime(2023, 3, 20)},
                        new CumulativeFlowResultRecord { WorkItemId = 2973718, Date = new DateTime(2023, 3, 21)},
                        new CumulativeFlowResultRecord { WorkItemId = 2973718, Date = new DateTime(2023, 3, 22)},
                    }
                }
            };

            var actual = CumulativeFlow.ArrangeCumulativeFlowData(cumulativeFlowJson, columnData);

            foreach (var key in expected.Keys)
            {
                Assert.That(actual.ContainsKey(key));
                Assert.That(actual[key].Count(), Is.EqualTo(expected[key].Count));
                var orderedActual = actual[key].OrderBy(item => item.WorkItemId).ThenBy(item => item.Date);
                var orderedExpected = expected[key].OrderBy(item => item.WorkItemId).ThenBy(item => item.Date);
                for (int i = 0; i < orderedActual.Count(); i++)
                {
                    Assert.That(orderedActual.ElementAt(i).WorkItemId, Is.EqualTo(orderedExpected.ElementAt(i).WorkItemId));
                    Assert.That(orderedActual.ElementAt(i).Date, Is.EqualTo(orderedExpected.ElementAt(i).Date));
                }
            }
            foreach (var key in actual.Keys)
            {
                Assert.That(expected.ContainsKey(key));
            }
        }

        [Test]
        public void ArrangeCumulativeFlowDataEmptyData()
        {
            var columnData = new Dictionary<string, ColumnResultRecord>
            {
                {"76edda43-7397-417f-9b6b-b01c6dc35cc3", new ColumnResultRecord("Backlog", 0, string.Empty, string.Empty) },
                {"4be22af9-6cec-4461-bea1-596f21e09f20", new ColumnResultRecord("Blocked", 1, string.Empty, string.Empty) },
                {"c2d8e6ff-f237-4ce5-b8d2-a3710c3cd13f", new ColumnResultRecord("In Progress", 2, "Doing", "Done") },
                {"8ed5387f-f834-4cdb-9f50-a0c7bbc5c5e1", new ColumnResultRecord("Dev Testing", 3, string.Empty, string.Empty) },
                {"31caa191-b96f-4697-bc90-7de08cf73b3e", new ColumnResultRecord("Closed", 4, string.Empty, string.Empty) },
            };
            var cumulativeFlowJson = new CumulativeFlowJsonRecord { Value = new List<CumulativeFlowJsonRec>() };
            cumulativeFlowJson.Value = new List<CumulativeFlowJsonRec>();

            var actual = CumulativeFlow.ArrangeCumulativeFlowData(cumulativeFlowJson, columnData);

            Assert.IsEmpty(actual);
        }

        [Test]
        public void ArrangeCumulativeFlowDataNullData()
        {
            var columnData = new Dictionary<string, ColumnResultRecord>
            {
                {"76edda43-7397-417f-9b6b-b01c6dc35cc3", new ColumnResultRecord("Backlog", 0, string.Empty, string.Empty) },
                {"4be22af9-6cec-4461-bea1-596f21e09f20", new ColumnResultRecord("Blocked", 1, string.Empty, string.Empty) },
                {"c2d8e6ff-f237-4ce5-b8d2-a3710c3cd13f", new ColumnResultRecord("In Progress", 2, "Doing", "Done") },
                {"8ed5387f-f834-4cdb-9f50-a0c7bbc5c5e1", new ColumnResultRecord("Dev Testing", 3, string.Empty, string.Empty) },
                {"31caa191-b96f-4697-bc90-7de08cf73b3e", new ColumnResultRecord("Closed", 4, string.Empty, string.Empty) },
            };
            var cumulativeFlowJson = new CumulativeFlowJsonRecord { Value = new List<CumulativeFlowJsonRec>() };

            var actual = CumulativeFlow.ArrangeCumulativeFlowData(cumulativeFlowJson, columnData);

            Assert.IsEmpty(actual);
        }

        [Test]
        public void AggregateCumulativeFlowData()
        {
            var cumulativeFlowData = new Dictionary<string, List<CumulativeFlowResultRecord>>
            {
                {
                    "Blocked",
                    new List<CumulativeFlowResultRecord>
                    {
                        new CumulativeFlowResultRecord { WorkItemId = 2841795, Date = new DateTime(2023, 3, 20)},
                        new CumulativeFlowResultRecord { WorkItemId = 2841795, Date = new DateTime(2023, 3, 21)},
                        new CumulativeFlowResultRecord { WorkItemId = 2841795, Date = new DateTime(2023, 3, 22)},
                        new CumulativeFlowResultRecord { WorkItemId = 2869008, Date = new DateTime(2023, 3, 20)},
                        new CumulativeFlowResultRecord { WorkItemId = 2869008, Date = new DateTime(2023, 3, 21)},
                        new CumulativeFlowResultRecord { WorkItemId = 2869008, Date = new DateTime(2023, 3, 22)},
                        new CumulativeFlowResultRecord { WorkItemId = 2869070, Date = new DateTime(2023, 3, 20)},
                        new CumulativeFlowResultRecord { WorkItemId = 2869070, Date = new DateTime(2023, 3, 21)},
                        new CumulativeFlowResultRecord { WorkItemId = 2869070, Date = new DateTime(2023, 3, 22)},
                        new CumulativeFlowResultRecord { WorkItemId = 2950621, Date = new DateTime(2023, 3, 20)},
                        new CumulativeFlowResultRecord { WorkItemId = 2950621, Date = new DateTime(2023, 3, 21)},
                        new CumulativeFlowResultRecord { WorkItemId = 2950621, Date = new DateTime(2023, 3, 22)},
                        new CumulativeFlowResultRecord { WorkItemId = 2961605, Date = new DateTime(2023, 3, 20)},
                        new CumulativeFlowResultRecord { WorkItemId = 2961605, Date = new DateTime(2023, 3, 21)},
                        new CumulativeFlowResultRecord { WorkItemId = 2961605, Date = new DateTime(2023, 3, 22)},
                        new CumulativeFlowResultRecord { WorkItemId = 2973752, Date = new DateTime(2023, 3, 20)},
                        new CumulativeFlowResultRecord { WorkItemId = 2973752, Date = new DateTime(2023, 3, 21)},
                        new CumulativeFlowResultRecord { WorkItemId = 2973752, Date = new DateTime(2023, 3, 22)},
                        new CumulativeFlowResultRecord { WorkItemId = 2980982, Date = new DateTime(2023, 3, 20)},
                        new CumulativeFlowResultRecord { WorkItemId = 2980982, Date = new DateTime(2023, 3, 21)},
                        new CumulativeFlowResultRecord { WorkItemId = 2980982, Date = new DateTime(2023, 3, 22)}
                    }
                },
                {
                    "In Progress - Doing",
                    new List<CumulativeFlowResultRecord>
                    {
                        new CumulativeFlowResultRecord { WorkItemId = 2835058, Date = new DateTime(2023, 3, 21)},
                        new CumulativeFlowResultRecord { WorkItemId = 2958002, Date = new DateTime(2023, 3, 21)},
                        new CumulativeFlowResultRecord { WorkItemId = 2968228, Date = new DateTime(2023, 3, 20)},
                        new CumulativeFlowResultRecord { WorkItemId = 2990008, Date = new DateTime(2023, 3, 20)},
                        new CumulativeFlowResultRecord { WorkItemId = 2990008, Date = new DateTime(2023, 3, 21)},
                        new CumulativeFlowResultRecord { WorkItemId = 2990008, Date = new DateTime(2023, 3, 22)},
                    }
                },
                {
                    "In Progress - Done",
                    new List<CumulativeFlowResultRecord>
                    {
                        new CumulativeFlowResultRecord { WorkItemId = 2835058, Date = new DateTime(2023, 3, 22)},
                        new CumulativeFlowResultRecord { WorkItemId = 2868999, Date = new DateTime(2023, 3, 20)},
                        new CumulativeFlowResultRecord { WorkItemId = 2868999, Date = new DateTime(2023, 3, 21)},
                        new CumulativeFlowResultRecord { WorkItemId = 2868999, Date = new DateTime(2023, 3, 22)},
                        new CumulativeFlowResultRecord { WorkItemId = 2884111, Date = new DateTime(2023, 3, 20)},
                        new CumulativeFlowResultRecord { WorkItemId = 2884111, Date = new DateTime(2023, 3, 21)},
                        new CumulativeFlowResultRecord { WorkItemId = 2884111, Date = new DateTime(2023, 3, 22)},
                        new CumulativeFlowResultRecord { WorkItemId = 2958002, Date = new DateTime(2023, 3, 22)},
                        new CumulativeFlowResultRecord { WorkItemId = 2968228, Date = new DateTime(2023, 3, 21)},
                    }
                },
                {
                    "Dev Testing",
                    new List<CumulativeFlowResultRecord>
                    {
                        new CumulativeFlowResultRecord { WorkItemId = 2961899, Date = new DateTime(2023, 3, 20)},
                        new CumulativeFlowResultRecord { WorkItemId = 2961899, Date = new DateTime(2023, 3, 21)},
                        new CumulativeFlowResultRecord { WorkItemId = 2961899, Date = new DateTime(2023, 3, 22)},
                    }
                },
                {
                    "Closed",
                    new List<CumulativeFlowResultRecord>
                    {
                        new CumulativeFlowResultRecord { WorkItemId = 2973718, Date = new DateTime(2023, 3, 20)},
                        new CumulativeFlowResultRecord { WorkItemId = 2973718, Date = new DateTime(2023, 3, 21)},
                        new CumulativeFlowResultRecord { WorkItemId = 2973718, Date = new DateTime(2023, 3, 22)},
                    }
                }
            };
            var expected = new Dictionary<DateTime, Dictionary<string, int>>
            {
                {
                    new DateTime(2023, 3, 20),
                    new Dictionary<string, int>
                    {
                        { "Blocked", 7 },
                        { "In Progress - Doing", 2 },
                        { "In Progress - Done", 2 },
                        { "Dev Testing", 1 },
                        { "Closed", 1 }
                    }
                },
                {
                    new DateTime(2023, 3, 21),
                    new Dictionary<string, int>
                    {
                        { "Blocked", 7 },
                        { "In Progress - Doing", 3 },
                        { "In Progress - Done", 3 },
                        { "Dev Testing", 1 },
                        { "Closed", 1 }
                    }
                },
                {
                    new DateTime(2023, 3, 22),
                    new Dictionary<string, int>
                    {
                        { "Blocked", 7 },
                        { "In Progress - Doing", 1 },
                        { "In Progress - Done", 4 },
                        { "Dev Testing", 1 },
                        { "Closed", 1 }
                    }
                }
            };

            var actual = CumulativeFlow.AggregateCumulativeFlowData(cumulativeFlowData);

            foreach (var key in expected.Keys)
            {
                Assert.That(actual.ContainsKey(key));
                Assert.That(actual[key], Is.EquivalentTo(expected[key]));
            }
            foreach (var key in actual.Keys)
            {
                Assert.That(expected.ContainsKey(key));
            }
        }

        [Test]
        public void AggregateCumulativeFlowDataEmptyData()
        {
            var cumulativeFlowData = new Dictionary<string, List<CumulativeFlowResultRecord>>();

            var actual = CumulativeFlow.AggregateCumulativeFlowData(cumulativeFlowData);

            Assert.IsEmpty(actual);
        }

        [Test]
        public void ColumnOrder()
        {
            var columnData = new Dictionary<string, ColumnResultRecord>
            {
                {"76edda43-7397-417f-9b6b-b01c6dc35cc3", new ColumnResultRecord("Backlog", 0, string.Empty, string.Empty) },
                {"4be22af9-6cec-4461-bea1-596f21e09f20", new ColumnResultRecord("Blocked", 1, string.Empty, string.Empty) },
                {"c2d8e6ff-f237-4ce5-b8d2-a3710c3cd13f", new ColumnResultRecord("In Progress", 2, "Doing", "Done") },
                {"8ed5387f-f834-4cdb-9f50-a0c7bbc5c5e1", new ColumnResultRecord("Dev Testing", 3, string.Empty, string.Empty) },
                {"31caa191-b96f-4697-bc90-7de08cf73b3e", new ColumnResultRecord("Closed", 4, string.Empty, string.Empty) },
            };
            var expected = new List<string>
            {
                "Closed",
                "Dev Testing",
                "In Progress - Done",
                "In Progress - Doing",
                "In Progress",
                "Blocked",
                "Backlog",
            };

            var actual = CumulativeFlow.ColumnOrder(columnData);

            Assert.That(actual.Count(), Is.EqualTo(expected.Count()));
            for (int i = 0; i < expected.Count; i++)
            {
                Assert.That(actual[i], Is.EqualTo(expected[i]));
            }
        }

        [Test]
        public void ColumnOrdeEmptyData()
        {
            var columnData = new Dictionary<string, ColumnResultRecord>();

            var actual = CumulativeFlow.ColumnOrder(columnData);

            Assert.IsEmpty(actual);
        }

        [Test]
        public void PutItAllTogtherForAChart()
        {
            var columnOrder = new List<string>
            {
                "Closed",
                "Dev Testing",
                "In Progress - Done",
                "In Progress - Doing",
                "In Progress",
                "Blocked",
                "Backlog",
            };
            var aggregatedData = new Dictionary<DateTime, Dictionary<string, int>>
            {
                {
                    new DateTime(2023, 3, 20),
                    new Dictionary<string, int>
                    {
                        { "Blocked", 7 },
                        { "In Progress - Doing", 2 },
                        { "In Progress - Done", 2 },
                        { "Dev Testing", 1 },
                        { "Closed", 1 }
                    }
                },
                {
                    new DateTime(2023, 3, 21),
                    new Dictionary<string, int>
                    {
                        { "Blocked", 7 },
                        { "In Progress - Doing", 3 },
                        { "In Progress - Done", 3 },
                        { "Dev Testing", 1 },
                        { "Closed", 1 }
                    }
                },
                {
                    new DateTime(2023, 3, 22),
                    new Dictionary<string, int>
                    {
                        { "Blocked", 7 },
                        { "In Progress - Doing", 1 },
                        { "In Progress - Done", 4 },
                        { "Dev Testing", 1 },
                        { "Closed", 1 }
                    }
                }
            };
            var expected = new ChartJsStackedLineDataSet[]
            {
                new ChartJsStackedLineDataSet
                {
                    data = new ChartPoint[]
                    {
                        new ChartPoint { x = "2023-03-20T00:00:00Z", y = 1},
                        new ChartPoint { x = "2023-03-21T00:00:00Z", y = 1},
                        new ChartPoint { x = "2023-03-22T00:00:00Z", y = 1}
                    },
                    label = "Closed",
                    borderColor = "SteelBlue",
                    backgroundColor = "SteelBlue",
                    fill = true,
                    pointStyle = false,
                },
                new ChartJsStackedLineDataSet
                {
                    data = new ChartPoint[]
                    {
                        new ChartPoint { x = "2023-03-20T00:00:00Z", y = 1},
                        new ChartPoint { x = "2023-03-21T00:00:00Z", y = 1},
                        new ChartPoint { x = "2023-03-22T00:00:00Z", y = 1}
                    },
                    label = "Dev Testing",
                    borderColor = "Coral",
                    backgroundColor = "Coral",
                    fill = true,
                    pointStyle = false,
                },
                new ChartJsStackedLineDataSet
                {
                    data = new ChartPoint[]
                    {
                        new ChartPoint { x = "2023-03-20T00:00:00Z", y = 2},
                        new ChartPoint { x = "2023-03-21T00:00:00Z", y = 3},
                        new ChartPoint { x = "2023-03-22T00:00:00Z", y = 4}
                    },
                    label = "In Progress - Done",
                    borderColor = "Gold",
                    backgroundColor = "Gold",
                    fill = true,
                    pointStyle = false,
                },
                new ChartJsStackedLineDataSet
                {
                    data = new ChartPoint[]
                    {
                        new ChartPoint { x = "2023-03-20T00:00:00Z", y = 2},
                        new ChartPoint { x = "2023-03-21T00:00:00Z", y = 3},
                        new ChartPoint { x = "2023-03-22T00:00:00Z", y = 1}
                    },
                    label = "In Progress - Doing",
                    borderColor = "LightSeaGreen",
                    backgroundColor = "LightSeaGreen",
                    fill = true,
                    pointStyle = false,
                },
                new ChartJsStackedLineDataSet
                {
                    data = new ChartPoint[]
                    {
                        new ChartPoint { x = "2023-03-20T00:00:00Z", y = 7},
                        new ChartPoint { x = "2023-03-21T00:00:00Z", y = 7},
                        new ChartPoint { x = "2023-03-22T00:00:00Z", y = 7}
                    },
                    label = "Blocked",
                    borderColor = "SlateBlue",
                    backgroundColor = "SlateBlue",
                    fill = true,
                    pointStyle = false,
                },
            };

            var actual = CumulativeFlow.PutItAllTogtherForAChart(aggregatedData, columnOrder);

            Assert.That(actual.Length, Is.EqualTo(expected.Length));
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.That(actual[i].data.Length, Is.EqualTo(expected[i].data.Length));
                for (int x = 0; x < expected[i].data.Length; x++)
                {
                    Assert.That(actual[i].data[x].x, Is.EqualTo(expected[i].data[x].x));
                    Assert.That(actual[i].data[x].y, Is.EqualTo(expected[i].data[x].y));
                }
                Assert.That(actual[i].label, Is.EqualTo(expected[i].label));
                Assert.That(actual[i].borderColor, Is.EqualTo(expected[i].borderColor));
                Assert.That(actual[i].backgroundColor, Is.EqualTo(expected[i].backgroundColor));
                Assert.That(actual[i].fill, Is.EqualTo(expected[i].fill));
                Assert.That(actual[i].pointStyle, Is.EqualTo(expected[i].pointStyle));
            }
        }

        [Test]
        public void PutItAllTogtherForAChartReturnsSortedByDate()
        {
            var columnOrder = new List<string>
            {
                "Closed",
                "Dev Testing",
                "In Progress - Done",
                "In Progress - Doing",
                "In Progress",
                "Blocked",
                "Backlog",
            };
            var aggregatedData = new Dictionary<DateTime, Dictionary<string, int>>
            {
                {
                    new DateTime(2023, 3, 22),
                    new Dictionary<string, int>
                    {
                        { "Blocked", 7 },
                        { "In Progress - Doing", 1 },
                        { "In Progress - Done", 4 },
                        { "Dev Testing", 1 },
                        { "Closed", 1 }
                    }
                },
                {
                    new DateTime(2023, 3, 20),
                    new Dictionary<string, int>
                    {
                        { "Blocked", 7 },
                        { "In Progress - Doing", 2 },
                        { "In Progress - Done", 2 },
                        { "Dev Testing", 1 },
                        { "Closed", 1 }
                    }
                },
                {
                    new DateTime(2023, 3, 21),
                    new Dictionary<string, int>
                    {
                        { "Blocked", 7 },
                        { "In Progress - Doing", 3 },
                        { "In Progress - Done", 3 },
                        { "Dev Testing", 1 },
                        { "Closed", 1 }
                    }
                },
            };
            var expected = new ChartJsStackedLineDataSet[]
            {
                new ChartJsStackedLineDataSet
                {
                    data = new ChartPoint[]
                    {
                        new ChartPoint { x = "2023-03-20T00:00:00Z", y = 1},
                        new ChartPoint { x = "2023-03-21T00:00:00Z", y = 1},
                        new ChartPoint { x = "2023-03-22T00:00:00Z", y = 1}
                    },
                    label = "Closed",
                    borderColor = "SteelBlue",
                    backgroundColor = "SteelBlue",
                    fill = true,
                    pointStyle = false,
                },
                new ChartJsStackedLineDataSet
                {
                    data = new ChartPoint[]
                    {
                        new ChartPoint { x = "2023-03-20T00:00:00Z", y = 1},
                        new ChartPoint { x = "2023-03-21T00:00:00Z", y = 1},
                        new ChartPoint { x = "2023-03-22T00:00:00Z", y = 1}
                    },
                    label = "Dev Testing",
                    borderColor = "Coral",
                    backgroundColor = "Coral",
                    fill = true,
                    pointStyle = false,
                },
                new ChartJsStackedLineDataSet
                {
                    data = new ChartPoint[]
                    {
                        new ChartPoint { x = "2023-03-20T00:00:00Z", y = 2},
                        new ChartPoint { x = "2023-03-21T00:00:00Z", y = 3},
                        new ChartPoint { x = "2023-03-22T00:00:00Z", y = 4}
                    },
                    label = "In Progress - Done",
                    borderColor = "Gold",
                    backgroundColor = "Gold",
                    fill = true,
                    pointStyle = false,
                },
                new ChartJsStackedLineDataSet
                {
                    data = new ChartPoint[]
                    {
                        new ChartPoint { x = "2023-03-20T00:00:00Z", y = 2},
                        new ChartPoint { x = "2023-03-21T00:00:00Z", y = 3},
                        new ChartPoint { x = "2023-03-22T00:00:00Z", y = 1}
                    },
                    label = "In Progress - Doing",
                    borderColor = "LightSeaGreen",
                    backgroundColor = "LightSeaGreen",
                    fill = true,
                    pointStyle = false,
                },
                new ChartJsStackedLineDataSet
                {
                    data = new ChartPoint[]
                    {
                        new ChartPoint { x = "2023-03-20T00:00:00Z", y = 7},
                        new ChartPoint { x = "2023-03-21T00:00:00Z", y = 7},
                        new ChartPoint { x = "2023-03-22T00:00:00Z", y = 7}
                    },
                    label = "Blocked",
                    borderColor = "SlateBlue",
                    backgroundColor = "SlateBlue",
                    fill = true,
                    pointStyle = false,
                },
            };

            var actual = CumulativeFlow.PutItAllTogtherForAChart(aggregatedData, columnOrder);

            Assert.That(actual.Length, Is.EqualTo(expected.Length));
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.That(actual[i].data.Length, Is.EqualTo(expected[i].data.Length));
                for (int x = 0; x < expected[i].data.Length; x++)
                {
                    Assert.That(actual[i].data[x].x, Is.EqualTo(expected[i].data[x].x));
                    Assert.That(actual[i].data[x].y, Is.EqualTo(expected[i].data[x].y));
                }
                Assert.That(actual[i].label, Is.EqualTo(expected[i].label));
                Assert.That(actual[i].borderColor, Is.EqualTo(expected[i].borderColor));
                Assert.That(actual[i].backgroundColor, Is.EqualTo(expected[i].backgroundColor));
                Assert.That(actual[i].fill, Is.EqualTo(expected[i].fill));
                Assert.That(actual[i].pointStyle, Is.EqualTo(expected[i].pointStyle));
            }
        }

        [Test]
        public void ColumnPercentagesOfTimeSpent()
        {
            var actual = new ChartJsStackedLineDataSet[]
            {
                new ChartJsStackedLineDataSet
                {
                    data = new ChartPoint[]
                    {   // 0 of 32
                        new ChartPoint { x = "2023-03-20T00:00:00Z", y = 1},
                        new ChartPoint { x = "2023-03-21T00:00:00Z", y = 1},
                        new ChartPoint { x = "2023-03-22T00:00:00Z", y = 1}
                    },
                    label = "Closed",
                    borderColor = "SteelBlue",
                    backgroundColor = "SteelBlue",
                    fill = true,
                    pointStyle = false,
                },
                new ChartJsStackedLineDataSet
                {
                    data = new ChartPoint[]
                    {   // 3 of 32 .09375 9%
                        new ChartPoint { x = "2023-03-20T00:00:00Z", y = 1},
                        new ChartPoint { x = "2023-03-21T00:00:00Z", y = 1},
                        new ChartPoint { x = "2023-03-22T00:00:00Z", y = 1}
                    },
                    label = "Dev Testing",
                    borderColor = "Coral",
                    backgroundColor = "Coral",
                    fill = true,
                    pointStyle = false,
                },
                new ChartJsStackedLineDataSet
                {
                    data = new ChartPoint[]
                    {   // 9 of 32 .28125 28%
                        new ChartPoint { x = "2023-03-20T00:00:00Z", y = 2},
                        new ChartPoint { x = "2023-03-21T00:00:00Z", y = 3},
                        new ChartPoint { x = "2023-03-22T00:00:00Z", y = 4}
                    },
                    label = "In Progress - Done",
                    borderColor = "Gold",
                    backgroundColor = "Gold",
                    fill = true,
                    pointStyle = false,
                },
                new ChartJsStackedLineDataSet
                {   
                    data = new ChartPoint[]
                    {   // 6 of 32 .1875 19%
                        new ChartPoint { x = "2023-03-20T00:00:00Z", y = 2},
                        new ChartPoint { x = "2023-03-21T00:00:00Z", y = 3},
                        new ChartPoint { x = "2023-03-22T00:00:00Z", y = 1}
                    },
                    label = "In Progress - Doing",
                    borderColor = "LightSeaGreen",
                    backgroundColor = "LightSeaGreen",
                    fill = true,
                    pointStyle = false,
                },
                new ChartJsStackedLineDataSet
                {
                    data = new ChartPoint[]
                    {   // 14 of 32 .4375 44%
                        new ChartPoint { x = "2023-03-20T00:00:00Z", y = 7},
                        new ChartPoint { x = "2023-03-21T00:00:00Z", y = 7},
                        new ChartPoint { x = "2023-03-22T00:00:00Z", y = 7}
                    },
                    label = "Blocked",
                    borderColor = "SlateBlue",
                    backgroundColor = "SlateBlue",
                    fill = true,
                    pointStyle = false,
                },
            };
            var expected = new ChartJsStackedLineDataSet[]
            {
                new ChartJsStackedLineDataSet
                {
                    data = new ChartPoint[]
                    {   // 0 of 39
                        new ChartPoint { x = "2023-03-20T00:00:00Z", y = 1},
                        new ChartPoint { x = "2023-03-21T00:00:00Z", y = 1},
                        new ChartPoint { x = "2023-03-22T00:00:00Z", y = 1}
                    },
                    label = "Closed",
                    borderColor = "SteelBlue",
                    backgroundColor = "SteelBlue",
                    fill = true,
                    pointStyle = false,
                },
                new ChartJsStackedLineDataSet
                {
                    data = new ChartPoint[]
                    {   // 3 of 39 .076923 7.7%
                        new ChartPoint { x = "2023-03-20T00:00:00Z", y = 1},
                        new ChartPoint { x = "2023-03-21T00:00:00Z", y = 1},
                        new ChartPoint { x = "2023-03-22T00:00:00Z", y = 1}
                    },
                    label = "Dev Testing 7.7%",
                    borderColor = "Coral",
                    backgroundColor = "Coral",
                    fill = true,
                    pointStyle = false,
                },
                new ChartJsStackedLineDataSet
                {
                    data = new ChartPoint[]
                    {   // 9 of 39 .2307 23.1%
                        new ChartPoint { x = "2023-03-20T00:00:00Z", y = 2},
                        new ChartPoint { x = "2023-03-21T00:00:00Z", y = 3},
                        new ChartPoint { x = "2023-03-22T00:00:00Z", y = 4}
                    },
                    label = "In Progress - Done 23.1%",
                    borderColor = "Gold",
                    backgroundColor = "Gold",
                    fill = true,
                    pointStyle = false,
                },
                new ChartJsStackedLineDataSet
                {
                    data = new ChartPoint[]
                    {   // 6 of 39 .1538 15.4%
                        new ChartPoint { x = "2023-03-20T00:00:00Z", y = 2},
                        new ChartPoint { x = "2023-03-21T00:00:00Z", y = 3},
                        new ChartPoint { x = "2023-03-22T00:00:00Z", y = 1}
                    },
                    label = "In Progress - Doing 15.4%",
                    borderColor = "LightSeaGreen",
                    backgroundColor = "LightSeaGreen",
                    fill = true,
                    pointStyle = false,
                },
                new ChartJsStackedLineDataSet
                {
                    data = new ChartPoint[]
                    {   // 21 of 39 .5384 53.8%
                        new ChartPoint { x = "2023-03-20T00:00:00Z", y = 7},
                        new ChartPoint { x = "2023-03-21T00:00:00Z", y = 7},
                        new ChartPoint { x = "2023-03-22T00:00:00Z", y = 7}
                    },
                    label = "Blocked 53.8%",
                    borderColor = "SlateBlue",
                    backgroundColor = "SlateBlue",
                    fill = true,
                    pointStyle = false,
                },
            };

            CumulativeFlow.AddPercentagesToLabels(actual);

            Assert.That(actual.Length, Is.EqualTo(expected.Length));
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.That(actual[i].data.Length, Is.EqualTo(expected[i].data.Length));
                for (int x = 0; x < expected[i].data.Length; x++)
                {
                    Assert.That(actual[i].data[x].x, Is.EqualTo(expected[i].data[x].x));
                    Assert.That(actual[i].data[x].y, Is.EqualTo(expected[i].data[x].y));
                }
                Assert.That(actual[i].label, Is.EqualTo(expected[i].label));
                Assert.That(actual[i].borderColor, Is.EqualTo(expected[i].borderColor));
                Assert.That(actual[i].backgroundColor, Is.EqualTo(expected[i].backgroundColor));
                Assert.That(actual[i].fill, Is.EqualTo(expected[i].fill));
                Assert.That(actual[i].pointStyle, Is.EqualTo(expected[i].pointStyle));
            }
        }
    }
}