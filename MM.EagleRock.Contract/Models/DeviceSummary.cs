using System.ComponentModel.DataAnnotations;

namespace MM.EagleRock.Contract.Models
{
    /// <summary>
    /// Models an EagleBot device's status and latest road traffic update received from the device.
    /// </summary>
    public class DeviceSummary
    {
        /// <summary>
        /// EagleBot device identifier.
        /// </summary>
        [Required]
        public Guid DeviceId { get; set; }

        [Required]
        public DeviceStatus Status { get; set; }

        public RoadTrafficUpdatePayload LatestRoadTrafficUpdate { get; set; }
    }
}
