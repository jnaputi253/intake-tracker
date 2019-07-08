using System.Collections.Generic;
using System.Threading.Tasks;
using IntakeTracker.Infrastructure;
using JetBrains.Annotations;

namespace IntakeTracker.Services
{
    public interface IService<in TEntity> where TEntity : class
    {
        Task<Response> FetchAllAsync();
        Task<Response> InsertAsync([NotNull] TEntity newEntity);
    }
}
