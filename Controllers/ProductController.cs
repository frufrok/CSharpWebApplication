using CSharpWebApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace CSharpWebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        [HttpGet("getProducts")]
        public IActionResult GetProducts()
        {
            try
            {
                using (var context = new ProductContext())
                {
                    var products = context.Products.Select(x => new Product()
                    {
                        ID = x.ID,
                        Name = x.Name,
                        Description = x.Description,
                        Price = x.Price
                    });
                    return Ok(products);
                }
            }
            catch
            {
                return StatusCode(500);
            }
        }
        [HttpPost("putProducts")]
        public IActionResult PutProducts([FromQuery] string name, string description, double price)
        {
            try
            {
                using (var context = new ProductContext())
                {
                    if (!context.Products.Any(x => x.Name.ToLower().Equals(name.ToLower())
                    {
                        context.Add(new Product()
                        {
                            Name = name,
                            Description = description,
                            Price = price
                        });
                        context.SaveChanges();
                        return Ok();
                    }
                    else 
                    {
                        return StatusCode(409);
                    }
                }
            }
            catch
            {
                return StatusCode(500);
            }
        }
    }
}
