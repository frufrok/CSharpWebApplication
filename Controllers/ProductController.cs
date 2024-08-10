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
        [HttpPost("postProduct")]
        public IActionResult AddProduct([FromQuery] string name, string description, double price)
        {
            try
            {
                using (var context = new ProductContext())
                {
                    if (!context.Products.Any(x => x.Name.ToLower().Equals(name.ToLower())))
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
        [HttpDelete("deleteProduct")]
        public IActionResult RemoveProduct([FromQuery] int id)
        {
            try
            {
                using (var context = new ProductContext())
                {
                    if (context.Products.Any(x => x.ID == id))
                    {
                        var product = context.Products.Find(id);
                        if (product != null)
                        {
                            context.Products.Remove(product);
                            context.SaveChanges();
                            return Ok();
                        }
                        else return StatusCode(409);
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
        [HttpPut("putProduct")]
        public IActionResult UpdateProduct([FromQuery] int id, string name, string description, double price)
        {
            try
            {
                using (var context = new ProductContext())
                {
                    if (context.Products.Any(x => x.ID == id))
                    {
                        var product = context.Products.Find(id);
                        if (product != null)
                        {
                            IActionResult applyChanges()
                            {
                                product.Name = name;
                                product.Description = description;
                                product.Price = price;
                                context.Products.Update(product);
                                context.SaveChanges();
                                return Ok();
                            }
                            
                            if (!product.Name.ToLower().Equals(name.ToLower())) 
                            {
                                if (context.Products.Any(x => x.Name.ToLower().Equals(name.ToLower())))
                                {
                                    return StatusCode(409);
                                }
                                else return applyChanges();
                            }
                            else
                            {
                                return applyChanges();
                            }
                        }
                        else return StatusCode(409);
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
