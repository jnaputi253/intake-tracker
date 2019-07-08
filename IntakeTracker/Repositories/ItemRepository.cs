using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IntakeTracker.Database.Configuration;
using IntakeTracker.Entities;
using MongoDB.Driver;

namespace IntakeTracker.Repositories
{
    public class ItemRepository : IRepository<Item>
    {
        private readonly IMongoCollection<Item> _itemsCollection;

        public ItemRepository(IntakeTrackerConfiguration configuration)
        {
            var client = new MongoClient(configuration.ConnectionString);
            var database = client.GetDatabase(configuration.DatabaseName);
            _itemsCollection = database.GetCollection<Item>("items");
        }

        public async Task<IEnumerable<Item>> FetchAllAsync()
        {
            IAsyncCursor<Item> fetchedItems = await _itemsCollection.FindAsync(item => true);

            return await fetchedItems.ToListAsync();
        }

        public async Task CreateAsync(Item newEntity)
        {
            await _itemsCollection.InsertOneAsync(newEntity);
        }

        public async Task<bool> ExistsAsync(Item entity)
        {
            IAsyncCursor<Item> fetchedItems = await _itemsCollection.FindAsync(item =>
                item.Name.Equals(entity.Name, StringComparison.CurrentCultureIgnoreCase));

            Item targetItem = await fetchedItems.FirstOrDefaultAsync();

            return targetItem != null;
        }
    }
}
