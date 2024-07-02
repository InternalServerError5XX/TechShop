namespace TechShop.Domain.DTOs.PaginationDto
{
    public class ResponsePaginationDto<T> where T : class
    {
        public IEnumerable<T> Data { get; set; } = null!;
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }
    }
}
