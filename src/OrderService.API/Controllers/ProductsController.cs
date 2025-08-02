using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Queries;

namespace OrderService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetProducts()
        {
            var result = await _mediator.Send(new GetProductsQuery());
            return Ok(result.Value);
        }

        [HttpGet("[action]/{name}")]
        public async Task<IActionResult> GetProductByName(string name)
        {
            var result = await _mediator.Send(new GetProductByNameQuery(name));
            if (result.Value == null)
            {
                return NotFound();
            }

            return Ok(result.Value);
        }
    }
}
