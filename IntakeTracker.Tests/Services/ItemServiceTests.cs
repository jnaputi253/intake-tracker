using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using IntakeTracker.Entities;
using IntakeTracker.Infrastructure;
using IntakeTracker.Repositories;
using IntakeTracker.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace IntakeTracker.Tests.Services
{
    public static class ItemServiceTests
    {
        [Fact]
        public static async Task ShouldReturnNoData()
        {
            var data = new List<Item>();
            IService<Item> itemService = CreateService(CreateRepositoryWithData(data));


            Response serviceResponse = await itemService.FetchAllAsync();


            TestUtil.AssertResponseStatus(serviceResponse, null, HttpStatusCode.OK);
            TestUtil.AssertCollectionSize<Item>(serviceResponse, 0);
        }

        [Fact]
        public static async Task ShouldReturnTheCorrectDataCount()
        {
            var data = new List<Item>
            {
                Mock.Of<Item>(),
                Mock.Of<Item>(),
                Mock.Of<Item>()
            };

            IService<Item> itemService = CreateService(CreateRepositoryWithData(data));


            Response serviceResponse = await itemService.FetchAllAsync();


            TestUtil.AssertResponseStatus(serviceResponse, null, HttpStatusCode.OK);
            TestUtil.AssertCollectionSize<Item>(serviceResponse, 3);
        }

        private static IRepository<Item> CreateRepositoryWithData(IEnumerable<Item> items)
        {
            var mockRepository = new Mock<IRepository<Item>>();
            mockRepository.Setup(mock => mock.FetchAllAsync())
                .ReturnsAsync(items);

            return mockRepository.Object;
        }

        private static IService<Item> CreateService(IRepository<Item> itemRepository)
        {
            return new ItemService(Mock.Of<ILogger<ItemService>>(), itemRepository);
        }
    }
}
