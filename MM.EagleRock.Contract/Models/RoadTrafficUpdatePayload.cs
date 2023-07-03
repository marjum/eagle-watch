using System.ComponentModel.DataAnnotations;

namespace MM.EagleRock.Contract.Models;

/// <summary>
/// The payload for a road traffic update, as provided by an EagleBot device.
/// </summary>
public class RoadTrafficUpdatePayload
{
    [Required]
    public Guid PayloadId { get; set; }

    /// <summary>
    /// EagleBot device identifier.
    /// </summary>
    [Required]
    public Guid DeviceId { get; set; }

    [Required]
    public GeoLocation GeoLocation { get; set; }

    [Required]
    public long Timestamp { get; set; }

    [Required]
    public RoadAddress Address { get; set; }

    [Required]
    public TrafficDirection TrafficDirection { get; set; }

    /// <summary>
    /// Rate of traffic flow, defined as average number of vehicles entering the road segment per 1 minute intervals.
    /// </summary>
    [Required]
    public double AverageTrafficFlowRate { get; set; }

    /// <summary>
    /// Average speed of vehicles at road segment's entry point, assuming kilometers per hour.
    /// </summary>
    /// <remarks>
    /// TODO: Support MPH or other speed units.
    /// </remarks>
    public short AverageVehicleSpeed { get; set; }
}
