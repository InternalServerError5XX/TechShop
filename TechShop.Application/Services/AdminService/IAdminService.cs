using System.Threading.Tasks;
using TechShop.Domain.DTOs.AdminDto;

namespace TechShop.Application.Services.AdminService
{
    public interface IAdminService
    {
        Task<ResponseAdminDto> GetAdminPanel();
        Task<ResponseAdminDto> GetCachedAdminPanel();
        void RemoveCachedAdminPanel();
    }
}
