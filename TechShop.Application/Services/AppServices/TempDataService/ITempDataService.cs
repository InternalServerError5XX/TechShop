namespace TechShop.Application.Services.AppServices.TempDataService
{
    public interface ITempDataService
    {
        void Set(string key, object value);
        T? Get<T>(string key);
    }
}
