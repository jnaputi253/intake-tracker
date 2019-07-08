using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using IntakeTracker.Database.Errors;
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
                    Message = DbErrors.ServerError
                };
            }

            return new Response(HttpStatusCode.OK)
            {
                Data = items
            };
        }

        public async Task<Response> InsertAsync(Item newEntity)
        {
            try
            {
                if (await _repository.ExistsAsync(newEntity))
                {
                    return new Response(HttpStatusCode.Conflict)
                    {
                        Message = DbErrors.ResourceExists
                    };
                }
                
                await _repository.CreateAsync(newEntity);
            }
            catch (MongoException e)
            {
                _logger.LogError(e.Message);
                
                return new Response(HttpStatusCode.InternalServerError)
                {
                    Message = DbErrors.ServerError
                };
            }
            
            return new Response(HttpStatusCode.Created);
        }
    }
}
