using IntakeTracker.Controllers;
using IntakeTracker.Repositories;
using IntakeTracker.Services;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using IntakeTracker.Database.Errors;
using IntakeTracker.Database.Errors.Resources;
using IntakeTracker.Entities;
using IntakeTracker.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
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

        [Fact]
        public static async Task ShouldReturnTheCorrectResponseWhenMongoDbExceptionIsThrown()
        {
            var mockRepository = new Mock<IRepository<Item>>();
            mockRepository.Setup(mock => mock.FetchAllAsync())
                .ThrowsAsync(new MongoException(string.Empty));

            ItemsController controller = CreateSimpleItemController(mockRepository.Object);


            IActionResult result = await controller.FetchAllAsync();


            result.Should().BeOfType<ObjectResult>()
                .Which.Value.Should().BeOfType<Response>()
                .Which.Should().Match<Response>(response =>
                    response.StatusCode == HttpStatusCode.InternalServerError &&
                    response.Message == DbErrors.ServerError &&
                    response.Data == null);
        }

        [Fact]
        public static async Task ShouldReturnTheCorrectResponseWhenTheItemIsNull()
        {
            ItemsController controller = new ItemsController(Mock.Of<IService<Item>>());


            IActionResult result = await controller.CreateItemAsync(null);


            result.Should().BeOfType<ObjectResult>()
                .Which.Value.Should().BeOfType<Response>()
                .Which.Should().Match<Response>(response =>
                    response.StatusCode == HttpStatusCode.BadRequest &&
                    response.Message == ItemErrors.InvalidItem &&
                    response.Data == null);
        }

        [Fact]
        public static async Task ShouldReturnAnErrorMessage()
        {
            var item = new Item
            {
                Name = null
            };


            ItemsController itemsController = new ItemsController(Mock.Of<IService<Item>>());


            IActionResult result = await itemsController.CreateItemAsync(item);


            result.Should().BeOfType<ObjectResult>()
                .Which.Value.Should().BeOfType<Response>()
                .Which.Should().Match<Response>(response =>
                    response.StatusCode == HttpStatusCode.BadRequest &&
                    response.Message == ItemErrors.ValidationErrorsPresent);
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
