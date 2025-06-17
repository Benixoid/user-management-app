using Microsoft.EntityFrameworkCore;
using UserManagementBack.Config;
using UserManagementBack.Models;
using UserManagementBack.Models.DTO;

namespace UserManagementBack.Data
{
    public interface IUserRepository : IGenericRepository<User, UserDTO>
    {
        Task<IEnumerable<UserDTO>> GetNotDeletedPaginatedAsync(PaginationParams paginationParams);
    }
}
