using System;
using System.Collections;
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
using IntakeTracker.Tests.Data;
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
        public static async Task ShouldReturnTheCorrectErrorMessageCountWhenNullDataIsEntered()
        {
            var item = new Item
            {
                Name = null,
                Category = null
            };


            ItemsController itemsController = new ItemsController(Mock.Of<IService<Item>>());


            IActionResult result = await itemsController.CreateItemAsync(item);

            result.Should().BeOfType<ObjectResult>()
                .Which.Value.Should().BeOfType<Response>()
                .Which.Data.Should().BeOfType<Dictionary<string, string>>()
                .Which.Count().Should().Be(2);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("     ")]
        [InlineData("\t\t\t")]
        [InlineData("  \t\t\t          \t\t\t  \t")]
        public static async Task ShouldReturnTheCorrectResponseWhenTheNameIsNullOrEmpty(string itemName)
        {
            Item item = CreateItem(itemName);
            ItemsController itemsController = new ItemsController(Mock.Of<IService<Item>>());


            IActionResult result = await itemsController.CreateItemAsync(item);


            result.Should().BeOfType<ObjectResult>()
                .Which.Value.Should().BeOfType<Response>()
                .Which.Should().Match<Response>(response =>
                    response.StatusCode == HttpStatusCode.BadRequest &&
                    response.Message == ItemErrors.ValidationErrorsPresent);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("     ")]
        [InlineData("\t\t\t")]
        [InlineData("  \t\t\t          \t\t\t  \t")]
        public static async Task ShouldReturnTheCorrectResponseWhenTheCategoryIsNullOrEmpty(string categoryName)
        {
            Item item = CreateItem(categoryName);
            ItemsController itemsController = new ItemsController(Mock.Of<IService<Item>>());


            IActionResult result = await itemsController.CreateItemAsync(item);


            result.Should().BeOfType<ObjectResult>()
                .Which.Value.Should().BeOfType<Response>()
                .Which.Should().Match<Response>(response =>
                    response.StatusCode == HttpStatusCode.BadRequest &&
                    response.Message == ItemErrors.ValidationErrorsPresent);
        }

        [Theory]
        [ClassData(typeof(NumericData))]
        public static async Task ShouldReturnTheCorrectValidationMessageWhenTheCaloriesAreNotWithinValidRanges(int calories)
        {
            var errorDictionary = new Dictionary<string, string>
            {
                {nameof(Item.Calories), ItemErrors.InvalidCalorieRange}
            };
            
            Item item = CreateItem(calories: calories);
            ItemsController itemsController = new ItemsController(Mock.Of<IService<Item>>());
            
            
            IActionResult result = await itemsController.CreateItemAsync(item);


            result.Should().BeOfType<ObjectResult>()
                .Which.Value.Should().BeOfType<Response>()
                .Which.Data.Should().BeOfType<Dictionary<string, string>>()
                .Which.Should().Equal(errorDictionary);
        }

        [Fact]
        public static async Task ShouldReturnTheCorrectResponseWhenTheItemInformationIsValidAndInserted()
        {
            Item item = CreateItem();
            
            var mockService = new Mock<IService<Item>>();
            mockService.Setup(mock => mock.CreateAsync(It.IsAny<Item>()))
                .ReturnsAsync(new Response(HttpStatusCode.Created));

            ItemsController itemsController = new ItemsController(mockService.Object);


            IActionResult result = await itemsController.CreateItemAsync(item);


            result.Should().BeOfType<ObjectResult>()
                .Which.Value.Should().BeOfType<Response>()
                .Which.Should().Match<Response>(response =>
                    response.StatusCode == HttpStatusCode.Created &&
                    response.Message == null &&
                    response.Data == null);
        }

        [Fact]
        public static async Task ShouldReturnTheCorrectResponseWhenTheCaloriesAreNegative()
        {
            Item item = CreateItem(calories: -1);
            ItemsController itemsController = new ItemsController(Mock.Of<IService<Item>>());
            
            
            IActionResult result = await itemsController.CreateItemAsync(item);


            result.Should().BeOfType<ObjectResult>()
                .Which.Value.Should().BeOfType<Response>()
                .Which.Should().Match<Response>(response =>
                    response.StatusCode == HttpStatusCode.BadRequest &&
                    response.Message == ItemErrors.ValidationErrorsPresent);
        }

        private static Item CreateItem(string name = "test", string category = "test", int calories = 0)
        {
            return new Item
            {
                Name = name,
                Category = category,
                Calories = calories
            };
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
