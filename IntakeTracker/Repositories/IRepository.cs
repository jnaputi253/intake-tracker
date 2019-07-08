using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace IntakeTracker.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> FetchAllAsync();
        Task CreateAsync([NotNull] TEntity newEntity);
        Task<bool> ExistsAsync([NotNull] TEntity entity);
    }
}
