using Microsoft.EntityFrameworkCore;
using UserManagementBack.Models;
using UserManagementBack.Models.DTO;

namespace UserManagementBack.Data
{
    public interface IUserRepository : IGenericRepository<User, UserDTO>
    {
        Task<IEnumerable<UserDTO>> GetNotDeletedPaginatedAsync(int skip = 0, int take = 20, string emailFilter = "", string nameFilter = "");
    }
}
