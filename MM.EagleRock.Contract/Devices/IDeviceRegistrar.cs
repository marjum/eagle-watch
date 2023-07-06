namespace MM.EagleRock.Contract
{
    /// <summary>
    /// Contract for device registration. (e.g. EagleBot device)
    /// </summary>
    public interface IDeviceRegistrar
    {
        /// <summary>
        /// Provides list of registered device IDs.
        /// </summary>
        /// <returns>Registered device IDs.</returns>
        IEnumerable<Guid> GetRegisteredDeviceIds();

        /// <summary>
        /// Determines whether a device is registered.
        /// </summary>
        /// <param name="deviceId">Id for device to check registration.</param>
        /// <returns>[true] if device is registered</returns>
        bool IsDeviceRegistered(Guid deviceId);
    }
}