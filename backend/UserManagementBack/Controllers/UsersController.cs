using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace UserManagementBack.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        public UsersController(ILogger<UsersController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string GetUsers()
        {
            return "123";
        }
    }
}
