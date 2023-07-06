using MM.EagleRock.Business;
using MM.EagleRock.Contract;
using MM.EagleRock.Contract.Models;
using MM.EagleRock.Contract.RoadTraffic;
using MM.EagleRock.Unit.Tests.Mock;
using Moq;
using System.ComponentModel.DataAnnotations;

namespace MM.EagleRock.Unit.Tests.Business.RoadTraffic
{
    [TestFixture]
    public class TestRoadTrafficOfficer
    {
        private static Guid DEVICE_ID = Guid.NewGuid();

        private Mock<IDeviceRegistrar> _mockDeviceRegistrar;

        private Mock<IDeviceSummaryCache> _mockDeviceSummaryCache;

        private IRoadTrafficOfficer _roadTrafficOfficer;

        [SetUp]
        public void TestSetup()
        {
            _mockDeviceRegistrar = new Mock<IDeviceRegistrar>();

            _mockDeviceSummaryCache = new Mock<IDeviceSummaryCache>();

            _roadTrafficOfficer = new RoadTrafficOfficer(_mockDeviceRegistrar.Object, _mockDeviceSummaryCache.Object);
        }

        [Test]
        public void TestProcessTrafficUpdate_WhenValidUpdate_ExpectNewUpdateAdded()
        {
            var trafficUpdate = TestModelBuilder.DefaultNewRoadTrafficUpdate(DEVICE_ID);

            _mockDeviceRegistrar
                .Setup(registrar => registrar.IsDeviceRegistered(DEVICE_ID))
                .Returns(true);

            _roadTrafficOfficer.ProcessTrafficUpdate(trafficUpdate);

            _mockDeviceSummaryCache.Verify(cache => cache.PublishRoadTrafficUpdate(trafficUpdate), Times.Once);
        }

        [Test]
        public void TestProcessTrafficUpdate_WhenDeviceNotRegistered_ExpectFailure()
        {
            var trafficUpdate = TestModelBuilder.DefaultNewRoadTrafficUpdate(DEVICE_ID);

            _mockDeviceRegistrar
                .Setup(registrar => registrar.IsDeviceRegistered(DEVICE_ID))
                .Returns(false);

            Assert.Throws(typeof(ValidationException), () => _roadTrafficOfficer.ProcessTrafficUpdate(trafficUpdate));
        }

        [Test]
        public void TestProcessTrafficUpdate_WhenPublicationWithRetriesFails_ExpectFailure()
        {
            // TODO: Test for cache publication failures
            Assert.Inconclusive();
        }

        [Test]
        public void TestGetDeviceStatuses_WhenStatusesAreCached_ExpectAllDeviceStatuses()
        {
            Guid deviceId2 = Guid.NewGuid();
            Guid deviceId3 = Guid.NewGuid();

            var testDeviceIds = new[] { DEVICE_ID, deviceId2, deviceId3 };
            var testDeviceStatuses = new[]
            {
                Tuple.Create(DEVICE_ID, DeviceStatus.Active),
                Tuple.Create(deviceId2, DeviceStatus.Active),
                Tuple.Create(deviceId3, DeviceStatus.Active)
            };
            var testDeviceSummaries = TestModelBuilder.DefaultDeviceSummaries(testDeviceStatuses);

            _mockDeviceRegistrar
                .Setup(registrar => registrar.GetRegisteredDeviceIds())
                .Returns(testDeviceIds);

            _mockDeviceSummaryCache
                .Setup(cache => cache.GetDeviceSummaries())
                .Returns(testDeviceSummaries);

            IEnumerable<DeviceSummary> deviceSummaries = _roadTrafficOfficer.GetDeviceStatuses();

            CollectionAssert.AreEquivalent(testDeviceSummaries.Values, deviceSummaries);
        }

        [Test]
        public void TestGetDeviceStatuses_WhenDeviceSummaryNotCached_ExpectDeviceStatusUnknown()
        {
            Guid deviceId2 = Guid.NewGuid();

            var testDeviceIds = new[] { DEVICE_ID, deviceId2 };
            var testDeviceStatuses = new[]
            {
                Tuple.Create(DEVICE_ID, DeviceStatus.Active)
            };
            var testDeviceSummaries = TestModelBuilder.DefaultDeviceSummaries(testDeviceStatuses);

            _mockDeviceRegistrar
                .Setup(registrar => registrar.GetRegisteredDeviceIds())
                .Returns(testDeviceIds);

            _mockDeviceSummaryCache
                .Setup(cache => cache.GetDeviceSummaries())
                .Returns(testDeviceSummaries);

            IEnumerable<DeviceSummary> deviceSummaries = _roadTrafficOfficer.GetDeviceStatuses();

            var nonCachedDeviceSummary = deviceSummaries.Single(summary => summary.DeviceId.Equals(deviceId2));
            Assert.That(nonCachedDeviceSummary.Status, Is.EqualTo(DeviceStatus.Unknown));
            Assert.That(nonCachedDeviceSummary.LatestRoadTrafficUpdate, Is.Null);
        }

        [Test]
        public void TestGetDeviceStatuses_WhenLastUpdateStale_ExpectDeviceStatusUnknown()
        {
            // TODO: Implement/test logic to return Unknown status when last update is stale (i.e. timestamp older than X seconds) 
            Assert.Inconclusive();
        }

        [Test]
        public void TestGetDeviceStatuses_WhenDeviceIsSignedOff_ExpectDeviceStatusOffDuty()
        {
            // TODO: Implement device explicitly signing off for device summary status to show as OffDuty,
            //  possibly still with latest update published
            Assert.Inconclusive();
        }
    }
}
