using Microsoft.AspNetCore.Mvc;

using Ordering.Application.DTOs;
using Ordering.Application.Services;

using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.WebApi.Controllers
{
    [Route("api/v1/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("{username}")]
        [ProducesResponseType(typeof(List<OrderDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<OrderDto>>> Get(string username, CancellationToken cancellationToken)
        {
            return Ok(await _orderService.GetByUsername(username, cancellationToken));
        }
    }
}
