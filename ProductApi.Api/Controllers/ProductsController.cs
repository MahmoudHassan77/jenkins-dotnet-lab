using Microsoft.AspNetCore.Mvc;
using ProductApi.Api.Models;
using ProductApi.Api.Services;

namespace ProductApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _repository;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IProductRepository repository, ILogger<ProductsController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    // GET: api/products
    [HttpGet]
    public ActionResult<IEnumerable<Product>> GetAll()
    {
        _logger.LogInformation("Getting all products");
        var products = _repository.GetAll();
        return Ok(products);
    }

    // GET: api/products/5
    [HttpGet("{id}")]
    public ActionResult<Product> GetById(int id)
    {
        _logger.LogInformation("Getting product with id {Id}", id);
        var product = _repository.GetById(id);
        
        if (product == null)
        {
            _logger.LogWarning("Product with id {Id} not found", id);
            return NotFound(new { message = $"Product with id {id} not found" });
        }

        return Ok(product);
    }

    // POST: api/products
    [HttpPost]
    public ActionResult<Product> Create([FromBody] Product product)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _logger.LogInformation("Creating new product: {ProductName}", product.Name);
        var createdProduct = _repository.Create(product);
        return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, createdProduct);
    }

    // PUT: api/products/5
    [HttpPut("{id}")]
    public ActionResult<Product> Update(int id, [FromBody] Product product)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _logger.LogInformation("Updating product with id {Id}", id);
        var updatedProduct = _repository.Update(id, product);
        
        if (updatedProduct == null)
        {
            _logger.LogWarning("Product with id {Id} not found for update", id);
            return NotFound(new { message = $"Product with id {id} not found" });
        }

        return Ok(updatedProduct);
    }

    // DELETE: api/products/5
    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        _logger.LogInformation("Deleting product with id {Id}", id);
        var deleted = _repository.Delete(id);
        
        if (!deleted)
        {
            _logger.LogWarning("Product with id {Id} not found for deletion", id);
            return NotFound(new { message = $"Product with id {id} not found" });
        }

        return NoContent();
    }
}
