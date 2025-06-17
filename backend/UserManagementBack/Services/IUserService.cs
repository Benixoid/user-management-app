using UserManagementBack.Config;
using UserManagementBack.Data;
using UserManagementBack.Models.DTO;
using UserManagementBack.Models.Response;

namespace UserManagementBack.Services
{
    public interface IUserService
    {
        public IDataManager dataManager { get; }
        Task<Result<UserDTO>> CreateUserAsync(UserDTO createDto);
        Task<Result<IEnumerable<UserDTO>>> GetUsersAsync(PaginationParams paginationParams);
        Task<Result<UserDTO>> UpdateUserAsync(UserDTO updateDto);
        Task<Result<UserDTO>> DeleteAsync(Guid id);        
    }
}
