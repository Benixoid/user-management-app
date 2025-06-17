using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UserManagementBack.Models;
using UserManagementBack.Models.DTO;

namespace UserManagementBack.Data
{
    public class UserRepository : GenericRepository<User, UserDTO>, IUserRepository
    {
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(AppDbContext context, ILogger<UserRepository> logger, IMapper mapper) : base(context, logger, mapper)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<UserDTO>> GetNotDeletedPaginatedAsync(int skip = 0, int take = 20, string emailFilter = "", string nameFilter = "")
        {
            var query = _dbSet.Where(u => !u.IsDeleted);
            if (!string.IsNullOrEmpty(emailFilter))
            {
                query = query.Where(u => u.Email.Contains(emailFilter, StringComparison.OrdinalIgnoreCase));
            }
            if (!string.IsNullOrEmpty(nameFilter))
            {
                query = query.Where(u => u.FullName.Contains(nameFilter, StringComparison.OrdinalIgnoreCase));
            }
            var entities = await query.Skip(skip).Take(take).ToListAsync();
            return _mapper.Map<IEnumerable<UserDTO>>(entities);
        }
    }
}
