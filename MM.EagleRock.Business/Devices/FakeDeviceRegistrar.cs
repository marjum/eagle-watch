using MM.EagleRock.Contract;

namespace MM.EagleRock.Business.Devices
{
    /// <summary>
    /// A stubbed device registrar, using a list of static fake device IDs.
    /// </summary>
    /// <remarks>
    /// TODO: Replace with data-driven devices registrar.
    /// </remarks>
    public class FakeDeviceRegistrar : IDeviceRegistrar
    {
        private static IEnumerable<Guid> REGISTERED_FAKE_DEVICE_IDS = new HashSet<Guid> 
        {
            new Guid("cda4687e-b8bc-4a50-a9d1-00ef4007e8ca"),
            new Guid("c4567668-5073-41d6-a633-13e5049e7775"),
            new Guid("71b3978d-2a35-4314-9908-ee5fa704caac")
        };

        /// <inheritdoc/>
        public IEnumerable<Guid> GetRegisteredDeviceIds()
        {
            return REGISTERED_FAKE_DEVICE_IDS.AsEnumerable();
        }

        /// <inheritdoc/>
        public bool IsDeviceRegistered(Guid deviceId)
        {
            return REGISTERED_FAKE_DEVICE_IDS.Contains(deviceId);
        }
    }
}
