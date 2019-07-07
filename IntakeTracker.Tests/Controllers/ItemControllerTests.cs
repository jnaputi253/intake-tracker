using IntakeTracker.Controllers;
using IntakeTracker.Repositories;
using IntakeTracker.Services;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using IntakeTracker.Entities;
using IntakeTracker.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace IntakeTracker.Tests.Controllers
{
    public static class ItemControllerTests
    {
        [Fact]
        public static async Task ShouldReturnNoItems()
        {
            var mockRepository = new Mock<IRepository<Item>>();
            mockRepository.Setup(mock => mock.FetchAllAsync())
                .ReturnsAsync(new List<Item>());

            ItemsController controller = CreateSimpleItemController(mockRepository.Object);


            IActionResult result = await controller.FetchAllAsync();


            AssertFetchListSize(result, 0);
        }

        [Fact]
        public static async Task ShouldReturnItems()
        {
            var items = new List<Item>
            {
                Mock.Of<Item>(),
                Mock.Of<Item>(),
                Mock.Of<Item>()
            };

            var mockRepository = new Mock<IRepository<Item>>();
            mockRepository.Setup(mock => mock.FetchAllAsync())
                .ReturnsAsync(items);

            ItemsController controller = CreateSimpleItemController(mockRepository.Object);


            IActionResult result = await controller.FetchAllAsync();


            AssertFetchListSize(result, 3);
        }

        private static ItemsController CreateSimpleItemController(IRepository<Item> repository)
        {
            IService<Item> service = new ItemService(Mock.Of<ILogger<ItemService>>(), repository);

            return new ItemsController(service);
        }

        private static void AssertFetchListSize(IActionResult result, int expected)
        {
            result.Should().BeOfType<ObjectResult>()
                .Which.Value.Should().BeOfType<Response>()
                .Which.Data.Should().BeAssignableTo<IEnumerable<Item>>()
                .Which.Count().Should().Be(expected);
        }
    }
}
