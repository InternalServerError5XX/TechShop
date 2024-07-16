using System.Linq.Expressions;

namespace TechShop.Domain.DTOs.FilterDto
{
    public class RequestFilterDto<T> where T : class
    {
        public List<Expression<Func<T, bool>>> SearchTerms { get; set; } = new List<Expression<Func<T, bool>>>();
        public Expression<Func<T, object>>? OrderBy { get; set; }
        public bool IsAsc { get; set; } = true;

        public void AddSearchTerm(Expression<Func<T, bool>> searchTerm)
        {
            SearchTerms.Add(searchTerm);
        }
    }
}
