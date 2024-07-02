using System.Linq.Expressions;

namespace TechShop.Domain.DTOs.FilterDto
{
    public class RequestFilterDto<T> where T : class
    {
        public Expression<Func<T, bool>>? SearchTerm { get; set; }
        public Expression<Func<T, object>>? OrderBy { get; set; }
        public bool IsAsc { get; set; } = true;
    }
}
