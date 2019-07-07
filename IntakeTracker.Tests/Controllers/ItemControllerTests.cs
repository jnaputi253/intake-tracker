using IntakeTracker.Controllers;
using IntakeTracker.Repositories;
using IntakeTracker.Services;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        private ItemsController _controller;
        private IRepository<Item> _repository;
        private IService<Item> _service;

        [Fact]
        public async void ShouldReturnNoItems()
        {
            var mockRepository = new Mock<IRepository<Item>>();
            mockRepository.Setup(mock => mock.FetchAllAsync())
                .ReturnsAsync(new List<Item>());

            _repository = mockRepository.Object;
            _service = new ItemService(Mock.Of<ILogger<ItemService>>(), _repository);
            _controller = new ItemsController(_service);


            IActionResult result = await _controller.FetchAllAsync();


            result.Should().BeOfType<ObjectResult>()
                .Which.Value.Should().BeOfType<Response>()
                .Which.Data.Should().BeAssignableTo<IEnumerable<Item>>()
                .Which.Count().Should().Be(0);
        }

        [Fact]
        public async void ShouldReturnItems()
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

            _repository = mockRepository.Object;
            _service = new ItemService(Mock.Of<ILogger<ItemService>>(), _repository);
            _controller = new ItemsController(_service);


            IActionResult result = await _controller.FetchAllAsync();


            result.Should().BeOfType<ObjectResult>()
                .Which.Value.Should().BeOfType<Response>()
                .Which.Data.Should().BeAssignableTo<IEnumerable<Item>>()
                .Which.Count().Should().Be(3);
        }
    }
}
