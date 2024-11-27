using System;
using AgileMetricsRules;

namespace Tests
{
    public class SimulationResultsTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CreateCsvDataFromSimulationResults()
        {
            var simResults = new SimulationResults
            {
                simulations = new[]
                {
                    new ChartPoint { x = "2", y = 5},
                    new ChartPoint { x = "3", y = 6},
                    new ChartPoint { x = "4", y = 8},
                    new ChartPoint { x = "5", y = 11},
                    new ChartPoint { x = "6", y = 9},
                    new ChartPoint { x = "7", y = 7},
                    new ChartPoint { x = "8", y = 6}
                },
                thirtieth = new[]
                {
                    new ChartPoint {x="3", y=6}
                },
                fiftieth = new[]
                {
                    new ChartPoint {x="4", y=8}
                },
                seventieth = new[]
                {
                    new ChartPoint {x="5", y=11}
                },
                eightyFifth = new[]
                {
                    new ChartPoint {x="6", y=9}
                },
                ninetyFifth = new[]
                {
                    new ChartPoint {x="7", y=7}
                }
            };
            var adoTeam = "team";
            var workItemType = "user story";
            var startingDate = new DateTime(2022, 10, 28);
            var endingDate = new DateTime(2022, 10, 29);
            var numberOfStories = 15;
            var numberOfSimulations = 10000;
            var expectedResult = "Team,Work item type,Starting date,Ending date,Number of stories,Number of simulations\nteam,user story,10/28/2022,10/29/2022,15,10000\n,,,\ndays,count of occurrences,,\n2,5,,\n3,6,,\n4,8,,\n5,11,,\n6,9,,\n7,7,,\n8,6,,\n,,,\npercentile,days,,\n30th,3,,,\n50th,4,,,\n70th,5,,\n85th,6,,\n95th,7,,";

            var actualResult = simResults.ToCsv(adoTeam, workItemType, startingDate, endingDate, numberOfStories, numberOfSimulations);

            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }

        [Test]
        public void CreateCsvDataFromEmptySimulationResults()
        {
            var simResults = new SimulationResults
            {
                simulations = new[]
                {
                    new ChartPoint { x = "0", y = 10}
                },
                thirtieth = new[]
                {
                    new ChartPoint {x="0", y=10}
                },
                fiftieth = new[]
                {
                    new ChartPoint {x="0", y=10}
                },
                seventieth = new[]
                {
                    new ChartPoint {x="0", y=10}
                },
                eightyFifth = new[]
                {
                    new ChartPoint {x="0", y=10}
                },
                ninetyFifth = new[]
                {
                    new ChartPoint {x="0", y=10}
                }
            };
            var adoTeam = "team";
            var workItemType = "user story";
            var startingDate = new DateTime(2022, 10, 28);
            var endingDate = new DateTime(2022, 10, 29);
            var numberOfStories = 15;
            var numberOfSimulations = 10000;
            var expectedResult = "Team,Work item type,Starting date,Ending date,Number of stories,Number of simulations\nteam,user story,10/28/2022,10/29/2022,15,10000\n,,,\ndays,count of occurrences,,\n0,10,,\n,,,\npercentile,days,,\n30th,0,,,\n50th,0,,,\n70th,0,,\n85th,0,,\n95th,0,,";

            var actualResult = simResults.ToCsv(adoTeam, workItemType, startingDate, endingDate, numberOfStories, numberOfSimulations);

            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }
    }
}