using System.Threading.Tasks;
using IntakeTracker.Entities;
using IntakeTracker.Infrastructure;
using IntakeTracker.Services;
using Microsoft.AspNetCore.Mvc;

namespace IntakeTracker.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]")]
    public class ItemsController
    {
        private readonly IService<Item> _service;

        public ItemsController(IService<Item> service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> FetchAllAsync()
        {
            Response response = await _service.FetchAllAsync();

            return new ObjectResult(response);
        }
    }
}
