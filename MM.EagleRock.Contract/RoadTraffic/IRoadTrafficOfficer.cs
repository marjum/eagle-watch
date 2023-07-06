using MM.EagleRock.Contract.Models;

namespace MM.EagleRock.Contract.RoadTraffic
{
    /// <summary>
    /// Contract for a road traffic "officer" component responsible for accepting traffic updates from devices, 
    /// and returning per-registered device summaries.
    /// </summary>
    public interface IRoadTrafficOfficer
    {
        /// <summary>
        /// Gets a summary per registered device, with last traffic update reported per device. 
        /// (if the device has reported road traffic updates)
        /// </summary>
        IEnumerable<DeviceSummary> GetDevicesSummary();

        /// <summary>
        /// Processes an incoming road traffic update payload.
        /// </summary>
        /// <param name="roadTrafficUpdatePayload">Road traffic update.</param>
        void ProcessTrafficUpdate(RoadTrafficUpdatePayload roadTrafficUpdatePayload);
    }
}