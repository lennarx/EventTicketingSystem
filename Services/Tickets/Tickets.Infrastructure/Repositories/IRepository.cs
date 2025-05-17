using Tickets.Domain.Entities;

namespace Tickets.Infrastructure.Repositories
{
    public interface IRepository<TEntity>
    {
        Task<TEntity?> GetByIdAsync(Guid id);
        IEnumerable<TEntity> GetByCondition(Func<TEntity, bool> predicate);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task UpdateRangeAsync(IEnumerable<Ticket> tickets);
        Task AddAsync(TEntity entity);
        Task AddRangeAsync(IEnumerable<TEntity> entities);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
    }
}
