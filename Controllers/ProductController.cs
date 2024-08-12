﻿using CSharpWebApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace CSharpWebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        ProductContext _context = new ProductContext();

        [HttpGet("get")]
        public ActionResult<List<Product>> Get()
        {
            try
            {
                var products = _context.Products.Select(x => new Product()
                {
                    ID = x.ID,
                    Name = x.Name,
                    Description = x.Description,
                    Price = x.Price
                });
                return Ok(products);
            }
            catch
            {
                return StatusCode(500);
            }
        }
        [HttpPost("add")]
        public ActionResult<int> Add([FromQuery] string name, string description, double price, int categoryID)
        {
            try
            {
                if (!_context.Products.Any(x => x.Name.ToLower().Equals(name.ToLower())))
                {
                    var product = new Product()
                    {
                        Name = name,
                        Description = description,
                        Price = price,
                        CategoryID = categoryID
                    };
                    _context.Add(product);
                    _context.SaveChanges();
                    return Ok(product.ID);
                }
                else
                {
                    return StatusCode(409);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}\r\n{ex.InnerException}");
            }
        }
        [HttpDelete("delete")]
        public ActionResult Delete([FromQuery] int id)
        {
            try
            {
                    if (_context.Products.Any(x => x.ID == id))
                    {
                        var product = _context.Products.Find(id);
                        if (product != null)
                        {
                            _context.Products.Remove(product);
                            _context.SaveChanges();
                            return Ok();
                        }
                        else return StatusCode(409);
                    }
                    else
                    {
                        return StatusCode(409);
                    }

            }
            catch
            {
                return StatusCode(500);
            }
        }
        [HttpPut("update")]
        public ActionResult Update([FromQuery] int id, string name, string description, double price)
        {
            try
            {
                    if (_context.Products.Any(x => x.ID == id))
                    {
                        var product = _context.Products.Find(id);
                        if (product != null)
                        {
                            ActionResult applyChanges()
                            {
                                product.Name = name;
                                product.Description = description;
                                product.Price = price;
                                _context.Products.Update(product);
                                _context.SaveChanges();
                                return Ok();
                            }
                            
                            if (!product.Name.ToLower().Equals(name.ToLower())) 
                            {
                                if (_context.Products.Any(x => x.Name.ToLower().Equals(name.ToLower())))
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
            catch
            {
                return StatusCode(500);
            }
        }
    }
}
