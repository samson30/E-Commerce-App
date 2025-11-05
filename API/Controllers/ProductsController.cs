using System.Collections;
using Core.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly StoreContext context;

        public ProductsController(StoreContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await context.Products.ToListAsync();
        }
        [HttpGet("{id:int}")] // api/product/id
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await context.Products.FindAsync(id);
            if (product != null)
            {
                return product;
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            context.Add(product);
            await context.SaveChangesAsync();
            return product;
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult> EditProduct(int id, Product product)
        {
            if (ProductExists(id) && product.Id == id)
            {
                context.Entry(product).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return NoContent();
            }
            return BadRequest("Cannot Update product");
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await context.Products.FindAsync(id);
            if (product != null)
            {
                context.Products.Remove(product);
                await context.SaveChangesAsync();
                return NoContent();
            }
            return NotFound();
        }
        
        private bool ProductExists(int id)
        {
            return context.Products.Any(x => x.Id == id);
        }
    }
}
