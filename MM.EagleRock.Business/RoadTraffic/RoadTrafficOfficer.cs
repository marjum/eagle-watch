using MM.EagleRock.Contract;
using MM.EagleRock.Contract.Models;
using MM.EagleRock.Contract.RoadTraffic;
using System.ComponentModel.DataAnnotations;

namespace MM.EagleRock.Business
{
    /// <summary>
    /// Road traffic "officer" implementation.
    /// </summary>
    public class RoadTrafficOfficer : IRoadTrafficOfficer
    {
        private IDeviceRegistrar _deviceRegistrar;
        private IDeviceSummaryCache _deviceSummaryCache;

        public RoadTrafficOfficer(IDeviceRegistrar deviceRegistrar, IDeviceSummaryCache deviceSummaryCache)
        {
            this._deviceRegistrar = deviceRegistrar;
            this._deviceSummaryCache = deviceSummaryCache;
        }

        /// <inheritdoc/>
        public IEnumerable<DeviceSummary> GetDeviceStatuses()
        {
            var registeredDeviceIds = _deviceRegistrar.GetRegisteredDeviceIds();

            var deviceSummaries = _deviceSummaryCache.GetDeviceSummaries();

            return registeredDeviceIds
                .Select(deviceId => 
                {
                    if (deviceSummaries.ContainsKey(deviceId)) 
                    {
                        return deviceSummaries[deviceId];
                    };

                    return new DeviceSummary()
                    {
                        DeviceId = deviceId,
                        Status = DeviceStatus.Unknown,
                        LatestRoadTrafficUpdate = null
                    };
                });
        }

        /// <inheritdoc/>
        public void ProcessTrafficUpdate(RoadTrafficUpdatePayload roadTrafficUpdatePayload)
        {
            if (_deviceRegistrar.IsDeviceRegistered(roadTrafficUpdatePayload.DeviceId) == false) 
            {
                throw new ValidationException(
                    $"Device [{roadTrafficUpdatePayload.DeviceId}] for traffic update payload " +
                    $"with Id [{roadTrafficUpdatePayload.PayloadId}] is not registered");
            }

            _deviceSummaryCache.PublishRoadTrafficUpdate(roadTrafficUpdatePayload);
        }
    }
}