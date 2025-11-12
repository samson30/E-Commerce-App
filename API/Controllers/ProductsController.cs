using System.Collections;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IGenericRepository<Product> repo) : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand, string? type, string? sort)
        {
            var spec = new ProductSpecification(brand, type,sort);
            var products =await repo.ListAsync(spec);

            return Ok(products);
        }
        [HttpGet("{id:int}")] // api/product/id
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await repo.GetByIdAsync(id);
            if (product != null)
            {
                return product;
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            repo.Add(product);
            if (await repo.SaveAllAsync())
            {
                return CreatedAtAction("GetProduct", new { id = product.Id }, product);
            }
            return BadRequest("Problem Creating the product");
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult> EditProduct(int id, Product product)
        {
            if (ProductExists(id) && product.Id == id)
            {
                repo.Update(product);
                if (await repo.SaveAllAsync())
                {
                    return NoContent();
                }
                return BadRequest("Problem updating the product");
            }
            return BadRequest("Problem updating the product");
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await repo.GetByIdAsync(id);
            if (product != null)
            {
                repo.Remove(product);

                if (await repo.SaveAllAsync())
                {
                    return NoContent();
                }

                return BadRequest("Problem Deleting the product");
            }
            return BadRequest("Problem Deleting the product");
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
        {
            var spec = new BrandListSpecification();
            return Ok(await repo.ListAsync(spec));
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
        {
            var spec = new TypeListSpecification();

            return Ok(await repo.ListAsync(spec));
        }

        private bool ProductExists(int id)
        {
            return repo.Exists(id);
        }
    }
}
