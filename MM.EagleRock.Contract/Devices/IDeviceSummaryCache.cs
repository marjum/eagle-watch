﻿using MM.EagleRock.Contract.Models;

namespace MM.EagleRock.Contract
{
    /// <summary>
    /// Contract for component maintaining the devices summary cache.
    /// </summary>
    public interface IDeviceSummaryCache
    {
        /// <summary>
        /// Gets a list of registered device summaries.
        /// </summary>
        /// <remarks>
        /// TODO: enable device criteria filtering, result pagination
        /// </remarks>
        /// <param name="deviceIds">Device identifiers for which to get cached summaries.</param>
        /// <returns>Per-registered device summaries with latest road traffic updates published</returns>
        IDictionary<Guid, DeviceSummary> GetDeviceSummaries(IEnumerable<Guid> deviceIds);

        /// <summary>
        /// Publishes road traffic update for the corresponding device's summary cache item.
        /// </summary>
        /// <param name="trafficUpdate">Road traffic update.</param>
        void PublishRoadTrafficUpdate(RoadTrafficUpdatePayload trafficUpdate);
    }
}