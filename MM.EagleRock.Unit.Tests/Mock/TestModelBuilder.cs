using MM.EagleRock.Contract.Models;

namespace MM.EagleRock.Unit.Tests.Mock
{
    internal class TestModelBuilder
    {
        internal static RoadTrafficUpdatePayload DefaultNewRoadTrafficUpdate(Guid deviceId)
        {
            return new RoadTrafficUpdatePayload() 
            {
                PayloadId = Guid.NewGuid(),
                DeviceId = deviceId,
                GeoLocation = DefaultNewGeoLocation(),
                Timestamp = DateTimeOffset.Now.ToUnixTimeSeconds(),
                Address = DefaultNewRoadAddress(),
                TrafficDirection = TrafficDirection.NorthBound,
                AverageTrafficFlowRate = 40.56,
                AverageVehicleSpeed = 33
            };
        }

        internal static GeoLocation DefaultNewGeoLocation()
        {
            return new GeoLocation() 
            {
                Latitude = -27.471407748292812,
                Longitude = 153.02465432944825
            };
        }

        internal static RoadAddress DefaultNewRoadAddress() 
        {
            return new RoadAddress() 
            {
                Segment = DefaultNewRoadSegment(),
                StreetName = "Elizabeth Street",
                City = "Brisbane",
                State = "Queensland",
                Country = "Australia",
                PostalCode = "QLD4000",
            };
        }

        /// <summary>
        /// Returns GeoJSON for an example road segment.
        /// </summary>
        /// <remarks>
        /// Example road segment for Elizabeth St in Brisbane CBD.
        /// </remarks>
        internal static String DefaultNewRoadSegment() 
        {
            return @"{
                ""type"": ""LineString"", 
                ""coordinates"": [
                    [153.02425414910306, -27.471674040322867],
                    [153.0259882639341, -27.470460926973487]
                ]
            }";
        }

        internal static IDictionary<Guid, DeviceSummary> DefaultDeviceSummaries(
            IEnumerable<Tuple<Guid, DeviceStatus>> deviceStatuses)
        {
            return deviceStatuses
                .Select(deviceStatus => DefaultNewDeviceSummary(deviceStatus.Item1, deviceStatus.Item2))
                .ToDictionary(summary => summary.DeviceId, summary => summary);
        }

        internal static DeviceSummary DefaultNewDeviceSummary(Guid deviceId, DeviceStatus deviceStatus)
        {
            return new DeviceSummary() 
            {
                DeviceId = deviceId,
                Status = deviceStatus,
                LatestRoadTrafficUpdate = deviceStatus == DeviceStatus.Active 
                    ? DefaultNewRoadTrafficUpdate(deviceId) 
                    : null
            };
        }
    }
}