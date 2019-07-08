using System.Collections.Generic;
using System.Threading.Tasks;

namespace IntakeTracker.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> FetchAllAsync();
        Task CreateAsync(TEntity newEntity);
        Task<bool> ExistsAsync(TEntity entity);
    }
}
