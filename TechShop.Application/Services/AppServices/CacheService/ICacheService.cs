namespace TechShop.Application.Services.AppServices.CacheService
{
    public interface ICacheService
    {
        void Set<T>(string key, T value, TimeSpan absoluteExpiration, TimeSpan slidingExpiration);
        T? Get<T>(string key);
        void Remove(string key);
    }
}
