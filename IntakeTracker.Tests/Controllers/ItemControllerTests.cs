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
    public class ItemControllerTests
    {
        [Fact]
        public async Task ShouldReturnNoItems()
        {
            var mockRepository = new Mock<IRepository<Item>>();
            mockRepository.Setup(mock => mock.FetchAllAsync())
                .ReturnsAsync(new List<Item>());

            IRepository<Item> repository = mockRepository.Object;
            IService<Item> service = new ItemService(Mock.Of<ILogger<ItemService>>(), repository);
            ItemsController controller = new ItemsController(service);


            IActionResult result = await controller.FetchAllAsync();


            result.Should().BeOfType<ObjectResult>()
                .Which.Value.Should().BeOfType<Response>()
                .Which.Data.Should().BeAssignableTo<IEnumerable<Item>>()
                .Which.Count().Should().Be(0);
        }

        [Fact]
        public async Task ShouldReturnItems()
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

            IRepository<Item> repository = mockRepository.Object;
            IService<Item> service = new ItemService(Mock.Of<ILogger<ItemService>>(), repository);
            ItemsController controller = new ItemsController(service);


            IActionResult result = await controller.FetchAllAsync();


            result.Should().BeOfType<ObjectResult>()
                .Which.Value.Should().BeOfType<Response>()
                .Which.Data.Should().BeAssignableTo<IEnumerable<Item>>()
                .Which.Count().Should().Be(3);
        }
    }
}
