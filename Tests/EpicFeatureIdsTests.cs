using System.Linq;
using AgileMetricsRules;

namespace Tests
{
    public class EpicFeatureIdTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void EpicFeatureIdResultsIntoString()
        {
            var featureIds = new EpicFeatureIdsJsonRecord
            {
                Value = new List<EpicFeatureIdsJsonRec>
                {
                    new EpicFeatureIdsJsonRec { WorkItemId = 1000 },
                    new EpicFeatureIdsJsonRec { WorkItemId = 2000 },
                    new EpicFeatureIdsJsonRec { WorkItemId = 3333 },
                    new EpicFeatureIdsJsonRec { WorkItemId = 444444444 },
                    new EpicFeatureIdsJsonRec { WorkItemId = 5000 },
                },
                BadRequest = false,
                NotAuthorized = false
            };
            var expected = "1000;2000;3333;444444444;5000";

            var actual = EpicFeatureIds.CollapseFeatureIds(featureIds);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void EpicFeatureIdEmptyValueIntoEmptyString()
        {
            var featureIds = new EpicFeatureIdsJsonRecord
            {
                Value = new List<EpicFeatureIdsJsonRec>(),
                BadRequest = false,
                NotAuthorized = false
            };

            var actual = EpicFeatureIds.CollapseFeatureIds(featureIds);

            Assert.That(string.IsNullOrEmpty(actual), Is.True);
        }

        [Test]
        public void EpicFeatureIdOneValueIntoNoSemiColon()
        {
            var featureIds = new EpicFeatureIdsJsonRecord
            {
                Value = new List<EpicFeatureIdsJsonRec> { new EpicFeatureIdsJsonRec { WorkItemId = 1} },
                BadRequest = false,
                NotAuthorized = false
            };
            var expected = "1";

            var actual = EpicFeatureIds.CollapseFeatureIds(featureIds);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void EpicFeatureIdBadRequest()
        {
            var featureIds = new EpicFeatureIdsJsonRecord
            {
                Value = new List<EpicFeatureIdsJsonRec>(),
                BadRequest = true,
                NotAuthorized = false
            };
            var expected = EpicFeatureIds.BadRequest;

            var actual = EpicFeatureIds.CollapseFeatureIds(featureIds);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void EpicFeatureIdNotAuthorized()
        {
            var featureIds = new EpicFeatureIdsJsonRecord
            {
                Value = new List<EpicFeatureIdsJsonRec>(),
                BadRequest = false,
                NotAuthorized = true
            };
            var expected = EpicFeatureIds.NotAuthorized;

            var actual = EpicFeatureIds.CollapseFeatureIds(featureIds);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void EpicFeatureIdBadRequestAndValueIsBadRequest()
        {
            var featureIds = new EpicFeatureIdsJsonRecord
            {
                Value = new List<EpicFeatureIdsJsonRec> { new EpicFeatureIdsJsonRec { WorkItemId = 1} },
                BadRequest = true,
                NotAuthorized = false
            };
            var expected = EpicFeatureIds.BadRequest;

            var actual = EpicFeatureIds.CollapseFeatureIds(featureIds);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void EpicFeatureIdNotAuthorizedAndValueIsNotAuthorized()
        {
            var featureIds = new EpicFeatureIdsJsonRecord
            {
                Value = new List<EpicFeatureIdsJsonRec> { new EpicFeatureIdsJsonRec { WorkItemId = 1 } },
                BadRequest = false,
                NotAuthorized = true
            };
            var expected = EpicFeatureIds.NotAuthorized;

            var actual = EpicFeatureIds.CollapseFeatureIds(featureIds);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void EpicFeatureIdBadRequestAndNotAuthorizedIsBadRequest()
        {
            var featureIds = new EpicFeatureIdsJsonRecord
            {
                Value = new List<EpicFeatureIdsJsonRec>(),
                BadRequest = true,
                NotAuthorized = true
            };
            var expected = EpicFeatureIds.BadRequest;

            var actual = EpicFeatureIds.CollapseFeatureIds(featureIds);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void EpicFeatureIdBadRequestAndNotAuthorizedAndValueIsBadRequest()
        {
            var featureIds = new EpicFeatureIdsJsonRecord
            {
                Value = new List<EpicFeatureIdsJsonRec> { new EpicFeatureIdsJsonRec { WorkItemId = 1 } },
                BadRequest = true,
                NotAuthorized = true
            };
            var expected = EpicFeatureIds.BadRequest;

            var actual = EpicFeatureIds.CollapseFeatureIds(featureIds);

            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}