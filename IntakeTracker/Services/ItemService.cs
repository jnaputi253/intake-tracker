using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using IntakeTracker.Entities;
using IntakeTracker.Infrastructure;
using IntakeTracker.Repositories;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace IntakeTracker.Services
{
    public class ItemService : IService<Item>
    {
        private readonly ILogger<ItemService> _logger;
        private readonly IRepository<Item> _repository;

        public ItemService(ILogger<ItemService> logger, IRepository<Item> repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<Response> FetchAllAsync()
        {
            IEnumerable<Item> items;

            try
            {
                items = await _repository.FetchAllAsync();
            }
            catch (MongoException e)
            {
                _logger.LogError(e.Message);

                return new Response(HttpStatusCode.InternalServerError)
                {
                    Message = "There was an error on our side.  Resolving it as soon as we can!"
                };
            }

            return new Response(HttpStatusCode.OK)
            {
                Data = items
            };
        }
    }
}
