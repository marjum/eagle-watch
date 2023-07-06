namespace MM.EagleRock.Contract.Cache
{
    /// <summary>
    /// Simple contract for a data item caching service.
    /// </summary>
    public interface ICacheService
    {
        /// <summary>
        /// Attempts to get item from cache using the supplied key.
        /// </summary>
        /// <typeparam name="T">Type of item to de-serialize to</typeparam>
        /// <param name="key">Cache item key</param>
        /// <returns>Cached item, de-serialized to type <see cref="T"/></returns>
        T? Get<T>(string key);

        /// <summary>
        /// Caches supplied item with supplied key.
        /// </summary>
        /// <typeparam name="T">Type of item to serialize from</typeparam>
        /// <param name="key">Cache item key</param>
        /// <param name="value">Cache item</param>
        /// <returns>Same item once cached</returns>
        T Set<T>(string key, T value);
    }
}
