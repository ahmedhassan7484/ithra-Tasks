using Application.DTOs;
using Application.Features.Orders.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ithra_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// Create a new order for a user. The request should include the user ID and a list of items with their quantities. The response will contain the order ID, total amount, discount applied, and details of the ordered items.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("CreateOrder")]
        [Authorize]
        public async Task<IActionResult> CreateOrder(CreateOrderRequestDto request)
        {
            var result = await _mediator.Send(new CreateOrderCommand(request));

            return Ok(result);
        }

    }
}
