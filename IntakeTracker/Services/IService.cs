using System.Collections.Generic;
using System.Threading.Tasks;
using IntakeTracker.Infrastructure;

namespace IntakeTracker.Services
{
    public interface IService<TEntity> where TEntity : class
    {
        Task<Response> FetchAllAsync();
    }
}
