using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using UserManagementBack.Config;
using UserManagementBack.Models;
using UserManagementBack.Models.DTO;
using UserManagementBack.Models.Response;
using UserManagementBack.Services;

namespace UserManagementBack.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUserService _userService;
        public UsersController(ILogger<UsersController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpGet]
        public async Task<ApiResponse<IEnumerable<UserDTO>>> GetUsers(PaginationParams paginationParams)
        {
            var result = await _userService.GetUsersAsync(paginationParams);
            return result.IsSuccess ?
                ApiResponse<IEnumerable<UserDTO>>.SuccessResponse(result.Value) :
                ApiResponse<IEnumerable<UserDTO>>.ErrorResponse(result.ErrorCode, result.ErrorMessage);
        }

        [HttpPost]
        public async Task<ApiResponse<UserDTO>> CreateUser(UserDTO user)
        {
            var result = await _userService.CreateUserAsync(user);
            return result.IsSuccess ?
                ApiResponse<UserDTO>.SuccessResponse(result.Value) :
                ApiResponse<UserDTO>.ErrorResponse(result.ErrorCode, result.ErrorMessage);

        }

        [HttpPut("{id}")]
        public async Task<ApiResponse<UserDTO>> UpdateUser(Guid id, UserDTO dto)
        {
            if (id != dto.Id)
            {
                return ApiResponse<UserDTO>.ErrorResponse("INVALID_ID", "The provided ID does not match the user ID in the request body.");
            }
            var result = await _userService.UpdateUserAsync(dto);
            return result.IsSuccess ?
                ApiResponse<UserDTO>.SuccessResponse(result.Value) :
                ApiResponse<UserDTO>.ErrorResponse(result.ErrorCode, result.ErrorMessage);
        }

        [HttpDelete("{id}")]
        public async Task<ApiResponse<UserDTO>> DeleteUser(Guid id)
        {
            if (id == Guid.Empty)
            {
                return ApiResponse<UserDTO>.ErrorResponse("INVALID_ID", "The provided ID is invalid.");
            }
            var result = await _userService.DeleteAsync(id);
            return result.IsSuccess ?
                ApiResponse<UserDTO>.SuccessResponse(result.Value) :
                ApiResponse<UserDTO>.ErrorResponse(result.ErrorCode, result.ErrorMessage);            
        }
    }
}
