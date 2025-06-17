using Microsoft.EntityFrameworkCore;
using UserManagementBack.Models.DTO;
using UserManagementBack.Models;
using AutoMapper;

namespace UserManagementBack.Data
{
    public class GenericRepository<TEntity, TEntityDTO> : IGenericRepository<TEntity, TEntityDTO>
         where TEntity : BaseEntity
         where TEntityDTO : BaseDTO
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;
        protected readonly IMapper _mapper;
        private readonly ILogger<GenericRepository<TEntity, TEntityDTO>> _logger;
        public GenericRepository(AppDbContext context, ILogger<GenericRepository<TEntity, TEntityDTO>> logger, IMapper mapper)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
            _logger = logger;
            _mapper = mapper;
        }
        public virtual async Task<TEntityDTO> CreateAsync(TEntityDTO createDto)
        {
            var entity = _mapper.Map<TEntity>(createDto);
            entity.CreatedAt = DateTime.UtcNow;
            _logger.LogInformation($"Saving new entity of type: {entity.GetType().Name}");
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return _mapper.Map<TEntityDTO>(entity);
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            var entity = await _dbSet.SingleOrDefaultAsync(e => e.Id == id);
            if (entity is not null)
            {
                entity.IsDeleted = true;
                entity.DeletedAt = DateTime.UtcNow;
                _context.Update(entity);
                await _context.SaveChangesAsync();
            }
        }

        public virtual async Task<IEnumerable<TEntityDTO>> GetAllNotDeletedAsync()
        {
            var entities = await _dbSet.Where(u => !u.IsDeleted).ToListAsync();
            return _mapper.Map<IEnumerable<TEntityDTO>>(entities);
        }

        public virtual async Task<IEnumerable<TEntityDTO>> GetDeletedAsync()
        {
            var entities = await _dbSet.Where(u => u.IsDeleted).ToListAsync();
            return _mapper.Map<IEnumerable<TEntityDTO>>(entities);
        }

        public virtual async Task<TEntityDTO?> GetNotDeletedByIdAsync(Guid id)
        {
            var entity = await _dbSet.AsNoTracking().SingleOrDefaultAsync(e => e.Id == id && !e.IsDeleted);
            return _mapper.Map<TEntityDTO>(entity);
        }

        public virtual async Task<TEntityDTO> UpdateNotDeletedAsync(TEntityDTO updateDTO)
        {
            
            var entity = _mapper.Map<TEntity>(updateDTO);
            entity.UpdatedAt = DateTime.UtcNow;
            _logger.LogInformation($"Updating entity of type: {entity.GetType().Name}");
            _context.Update(entity);
            await _context.SaveChangesAsync();
            return _mapper.Map<TEntityDTO>(entity);
        }

        public async Task UndeleteAsync(Guid id)
        {
            var entity = await _dbSet.SingleOrDefaultAsync(e => e.Id == id);
            if (entity is not null)
            {
                entity.IsDeleted = false;
                entity.DeletedAt = null;
                _context.Update(entity);
                await _context.SaveChangesAsync();
            }
            else
            {
                _logger.LogWarning($"Attempted to undelete non-existing entity with ID: {id}");
            }
        }
    }
}
