using Microsoft.AspNetCore.Http.HttpResults;
using System.Data;
using UserManagementBack.Config;
using UserManagementBack.Data;
using UserManagementBack.Models.DTO;
using UserManagementBack.Models.Response;

namespace UserManagementBack.Services
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        
        public IDataManager dataManager { get; }
        public UserService(IDataManager dataManager, ILogger<UserService> logger)
        {
            this.dataManager = dataManager;
            _logger = logger;            
        }

        public async Task<Result<UserDTO>> CreateUserAsync(UserDTO createDto)
        {
            _logger.LogInformation($"Creating user: FullName: '{createDto.FullName}', role: '{createDto.Role}', email: {createDto.Email}");
            if (!IsValidRole(createDto.Role))
            {
                _logger.LogError($"Invalid role specified: {createDto.Role} for user: {createDto.FullName}");
                return await Task.FromResult(Result<UserDTO>.Failure("INVALID_ROLE", "Invalid role specified. Valid roles are 'Admin', 'Manager', or 'User'."));
            }
            if (string.IsNullOrEmpty(createDto.FullName))
            {
                _logger.LogError("FullName is required for user creation.");
                return await Task.FromResult(Result<UserDTO>.Failure("INVALID_DATA", "FullName is required."));
            }
            if (string.IsNullOrEmpty(createDto.Email))
            {
                _logger.LogError("Email is required for user creation.");
                return await Task.FromResult(Result<UserDTO>.Failure("INVALID_DATA", "Email is required."));
            }
            createDto.Id = Guid.Empty;
            var created = await dataManager.Users.CreateAsync(createDto);
            return Result<UserDTO>.Success(created);
        }

        public async Task<Result<IEnumerable<UserDTO>>> GetUsersAsync(PaginationParams paginationParams)
        {
            try
            {
                var list = await dataManager.Users.GetNotDeletedPaginatedAsync(paginationParams);
                return Result<IEnumerable<UserDTO>>.Success(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving users.");
                return Result<IEnumerable<UserDTO>>.Failure("INTERNAL_ERROR", $"An error occurred while retrieving users: {ex.Message}");
            }            
        }

        public async Task<Result<UserDTO>> DeleteAsync(Guid id)
        {
            try
            {
                var entity = await dataManager.Users.GetNotDeletedByIdAsync(id);
                if (entity == null)
                {
                    _logger.LogWarning($"User with ID {id} not found for update.");
                    return Result<UserDTO>.Failure("USER_NOT_FOUND", $"User with ID {id} not found.");
                }
                await dataManager.Users.DeleteAsync(id);
                return Result<UserDTO>.Success(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting user with ID {id}.");
                return Result<UserDTO>.Failure("INTERNAL_ERROR", $"An error occurred while retrieving user for update: {ex.Message}");
            }
        }

        public async Task<Result<UserDTO>> UpdateUserAsync(UserDTO updateDto)
        {
            try
            {
                var entity = await dataManager.Users.GetNotDeletedByIdAsync(updateDto.Id);
                if (entity == null)
                {
                    _logger.LogWarning($"User with ID {updateDto.Id} not found for update.");
                    return Result<UserDTO>.Failure("USER_NOT_FOUND", $"User with ID {updateDto.Id} not found.");
                }
                if (!IsValidRole(updateDto.Role))
                {
                    _logger.LogError($"Invalid role specified: {updateDto.Role} for user ID: {updateDto.Id}");
                    return Result<UserDTO>.Failure("INVALID_ROLE", "Invalid role specified. Valid roles are 'Admin', 'Manager', or 'User'.");
                }
                if (string.IsNullOrEmpty(updateDto.FullName))
                {
                    _logger.LogError("FullName is required for user creation.");
                    return await Task.FromResult(Result<UserDTO>.Failure("INVALID_DATA", "FullName is required."));
                }
                if (string.IsNullOrEmpty(updateDto.Email))
                {
                    _logger.LogError("Email is required for user creation.");
                    return await Task.FromResult(Result<UserDTO>.Failure("INVALID_DATA", "Email is required."));
                }
                // Update the entity with the new values
                var updated = await dataManager.Users.UpdateNotDeletedAsync(updateDto);
                return Result<UserDTO>.Success(updated);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving user with ID {updateDto.Id} for update.");
                return Result<UserDTO>.Failure("INTERNAL_ERROR", $"An error occurred while retrieving user for update: {ex.Message}");

            }
            

        }

        public bool IsValidRole(string role)
        {
            return role == "Admin" || role == "Manager" || role == "User";
        }        
    }
}
