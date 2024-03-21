using Microsoft.AspNetCore.Mvc;
using MediatR;
using Order.Application.Features.Order.Commands.Create;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]OrderCreateDto order)
        {
            CreateOrderCommand createOrderCommand = new CreateOrderCommand() { Order = order };
            var response = await _mediator.Send(createOrderCommand);

            return Ok(response);
        }
    }
}
