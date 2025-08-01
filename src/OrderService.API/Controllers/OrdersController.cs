using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Commands;
using OrderService.Application.Dtos;

namespace OrderService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(IMediator mediator, ILogger<OrdersController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SubmitOrder([FromBody] SubmittedOrderDto dto)
        {
            var result = await _mediator.Send(new SubmitOrderCommand(dto));
            if (result.IsSuccess)
            {
                return Ok();
            }

            return BadRequest(new
            {
                result.ErrorCode,
                result.ErrorMessage
            });
        }
    }
}
