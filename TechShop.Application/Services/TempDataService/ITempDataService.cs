namespace TechShop.Application.Services.TempDataService
{
    public interface ITempDataService
    {
        void Set(string key, object value);
        T? Get<T>(string key);
    }
}
