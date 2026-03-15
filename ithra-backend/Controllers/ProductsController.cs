using Application.DTOs;
using Application.Features.Products.Commands;
using Application.Features.Products.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ithra_backend.Controllers
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
        /// <summary>
        /// Retrieves a list of products that match the specified filter criteria.
        /// </summary>
        /// <remarks>This method requires the caller to be authorized. The filter parameter must be
        /// provided to specify the criteria for product retrieval.</remarks>
        /// <param name="filter">The filter criteria used to narrow down the list of products. This includes properties such as category,
        /// price range, and availability status. Cannot be null.</param>
        /// <returns>An IActionResult containing the filtered list of products. Returns a 200 OK response with the product data
        /// if successful.</returns>
        [HttpGet("GetProducts")]
        [Authorize]
        public async Task<IActionResult> GetProducts([FromQuery] ProductFilterDto filter)
        {
            var result = await _mediator.Send(new GetProductsQuery(filter));
            return Ok(result);
        }
        /// <summary>
        /// Creates a new product using the specified product data transfer object.
        /// </summary>
        /// <remarks>This method requires the caller to be authorized. The operation is performed
        /// asynchronously and uses the mediator pattern to process the product creation request.</remarks>
        /// <param name="dto">The productResponseDto containing the details of the product to create. This parameter must not be null and
        /// should include all required product information.</param>
        /// <returns>An IActionResult that represents the result of the product creation operation. Returns an OK response with
        /// the created product details if the operation is successful.</returns>
        [HttpPost("CreateProduct")]
        [Authorize]
        public async Task<IActionResult> CreateProduct(productResponseDto dto)
        {
            var result = await _mediator.Send(new CreateProductCommand(dto));

            return Ok(result);
        }
    }
}
