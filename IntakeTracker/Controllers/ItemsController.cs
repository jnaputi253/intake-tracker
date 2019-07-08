using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using IntakeTracker.Database.Errors.Resources;
using IntakeTracker.Entities;
using IntakeTracker.Infrastructure;
using IntakeTracker.Infrastructure.Api;
using IntakeTracker.Infrastructure.Extensions;
using IntakeTracker.Services;
using IntakeTracker.Validators;
using Microsoft.AspNetCore.Http;
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

        [HttpPost("/create")]
        [Consumes(ContentTypes.Json)]
        [Produces(ContentTypes.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateItemAsync([FromHeader] Item item)
        {
            Response response;
            
            if (string.IsNullOrEmpty(item.Name.Trim()))
            {
                response = new Response(HttpStatusCode.BadRequest)
                {
                    Message = ItemErrors.EmptyItemName
                };
                
                return new ObjectResult(response);
            }

            Dictionary<string, string> validationErrors = ItemValidator.Validate(item);

            if (validationErrors.Count() != 0)
            {
                response = new Response(HttpStatusCode.BadRequest)
                {
                    Data = validationErrors
                };

                return new ObjectResult(response);
            }

            Item newItem = item.Move();

            response = await _service.CreateAsync(newItem);

            return new ObjectResult(response);
        }
    }
}
