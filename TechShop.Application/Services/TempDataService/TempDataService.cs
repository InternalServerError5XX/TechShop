using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace TechShop.Application.Services.TempDataService
{
    public class TempDataService : ITempDataService
    {
        private readonly ITempDataDictionary _tempData;

        public TempDataService(ITempDataDictionaryFactory tempDataDictionaryFactory, IHttpContextAccessor httpContextAccessor)
        {
            _tempData = tempDataDictionaryFactory.GetTempData(httpContextAccessor.HttpContext);
        }

        public void Set(string key, object value)
        {
            _tempData[key] = value;
        }

        public T? Get<T>(string key)
        {
            _tempData.TryGetValue(key, out var obj);
            return obj is T value ? value : default;
        }
    }
}
