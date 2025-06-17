using Microsoft.AspNetCore.Mvc;

namespace UserManagementBack.Controllers
{
    [ApiController]
    [Route("api/")]
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
