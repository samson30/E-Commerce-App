using System.Collections;
using Core.Entities;
using Core.Interfaces;
using Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IProductRepository repo) : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand,string? type,string? sort)
        {
            return Ok(await repo.GetProductsAsync(brand,type,sort));
        }
        [HttpGet("{id:int}")] // api/product/id
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await repo.GetProductByIdAsync(id);
            if (product != null)
            {
                return product;
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            repo.AddProduct(product);
            if (await repo.SaveChangesAsync())
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
                repo.UpdateProduct(product);
                if (await repo.SaveChangesAsync())
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
            var product = await repo.GetProductByIdAsync(id);
            if (product != null)
            {
                repo.DeleteProduct(product);

                if (await repo.SaveChangesAsync())
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
            return Ok(await repo.GetBrandsAsync());
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
        {
            return Ok(await repo.GetTypesAsync());
        }

        private bool ProductExists(int id)
        {
            return repo.ProductExists(id);
        }
    }
}
