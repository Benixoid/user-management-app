using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UserManagementBack.Config;
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

        public async Task<IEnumerable<UserDTO>> GetNotDeletedPaginatedAsync(PaginationParams paginationParams)
        {
            var query = _dbSet.Where(u => !u.IsDeleted);
            if (!string.IsNullOrEmpty(paginationParams.EmailFilter))
            {
                query = query.Where(u => u.Email.ToUpper().Contains(paginationParams.EmailFilter.ToUpper()));
            }
            if (!string.IsNullOrEmpty(paginationParams.NameFilter))
            {
                query = query.Where(u => u.FullName.ToUpper().Contains(paginationParams.NameFilter.ToUpper()));
            }
            var entities = await query
                .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                .Take(paginationParams.PageSize)
                .AsNoTracking()
                .ToListAsync();
            return _mapper.Map<IEnumerable<UserDTO>>(entities);
        }
    }
}
