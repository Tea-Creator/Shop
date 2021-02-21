using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Catalog.Application.Exceptions;
using Catalog.Application.Repositories;
using Catalog.Domain.Models;

namespace Catalog.WebApi.Controllers
{
    [Route("api/v1/[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly ILogger<CatalogController> _logger;
        private readonly IProductRepository _repository;

        public CatalogController(
            ILogger<CatalogController> logger,
            IProductRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<Product>>> Get(CancellationToken cancellationToken)
        {
            return Ok(await _repository.Get(cancellationToken));
        }

        [HttpGet("{id:length(24)}", Name = "Get")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> Get(string id, CancellationToken cancellationToken)
        {
            try
            {
                return Ok(await _repository.Get(id, cancellationToken));
            }
            catch (EntityNotFoundException)
            {
                _logger.LogInformation($"Product with id '{id}' hasn't been found in database");
                return NotFound();
            }
        }

        [HttpGet("GetProductByCategory/{category}")]
        [ProducesResponseType(typeof(List<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<Product>>> GetByCategory(string category, CancellationToken cancellationToken)
        {
            return Ok(await _repository.GetByCategory(category, cancellationToken));
        }

        [HttpPost]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.Created)]
        public async Task<ActionResult<Product>> Create([FromBody] Product product)
        {
            await _repository.Create(product);

            return CreatedAtAction("Get", new { product.Id }, product);
        }

        [HttpPut]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> Update([FromBody] Product product)
        {
            return Ok(await _repository.Update(product));
        }

        [HttpDelete("{id:length(24)}")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> Delete(string id)
        {
            return Ok(await _repository.Delete(id));
        }
    }
}
