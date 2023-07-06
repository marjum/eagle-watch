using MM.EagleRock.Contract;
using MM.EagleRock.Contract.Cache;
using MM.EagleRock.Contract.Models;

namespace MM.EagleRock.Business.Devices
{
    /// <summary>
    /// Devices summary caching component.
    /// </summary>
    /// <remarks>
    /// TODO: Add unit test coverage.
    /// </remarks>
    public class DeviceSummaryCache : IDeviceSummaryCache
    {
        private ICacheService _cacheService;

        public DeviceSummaryCache(ICacheService cacheService) 
        {
            _cacheService = cacheService;
        }

        /// <inheritdoc/>
        public IDictionary<Guid, DeviceSummary> GetDeviceSummaries(IEnumerable<Guid> deviceIds)
        {
            return deviceIds
                .Select(deviceId => 
                    {
                        // TODO: Evaluate either retrieving values from cache for all device Ids in batch, or
                        //  parallelizing per-device cached value retrievals
                        var lastTrafficUpdate = _cacheService.Get<RoadTrafficUpdatePayload>(deviceId.ToString());

                        return new DeviceSummary
                        { 
                            DeviceId = deviceId, 
                            Status = lastTrafficUpdate != null ? DeviceStatus.Active : DeviceStatus.Unknown,
                            LatestRoadTrafficUpdate = lastTrafficUpdate
                        };
                    })
                .ToDictionary(summary => summary.DeviceId, summary => summary);
        }

        /// <inheritdoc/>
        public void PublishRoadTrafficUpdate(RoadTrafficUpdatePayload trafficUpdate)
        {
            _cacheService.Set(trafficUpdate.DeviceId.ToString(), trafficUpdate);
        }
    }
}
