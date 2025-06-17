using UserManagementBack.Models;
using UserManagementBack.Models.DTO;

namespace UserManagementBack.Data
{
    public interface IGenericRepository<TEntity, TEntityDTO>
        where TEntity : BaseEntity
        where TEntityDTO : BaseDTO
    {
        Task<TEntityDTO> CreateAsync(TEntityDTO entity);
        Task<TEntityDTO> UpdateNotDeletedAsync(TEntityDTO dto);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<TEntityDTO>> GetAllNotDeletedAsync();
        Task<IEnumerable<TEntityDTO>> GetDeletedAsync();
        Task<TEntityDTO?> GetNotDeletedByIdAsync(Guid id);
        Task UndeleteAsync(Guid id);

    }
}
