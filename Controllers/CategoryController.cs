using CSharpWebApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace CSharpWebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        [HttpGet("getCategories")]
        public IActionResult GetCategories()
        {
            try
            {
                using (var context = new ProductContext())
                {
                    var categories = context.Categories.Select(x => new Category()
                    {
                        ID = x.ID,
                        Name = x.Name,
                        Description = x.Description
                    });
                    return Ok(categories);
                }
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpPost("postCategory")]
        public IActionResult AddCategory([FromQuery] string name, string description)
        {
            try
            {
                using (var context = new ProductContext())
                {
                    if (!context.Categories.Any(x => x.Name.ToLower().Equals(name.ToLower())))
                    {
                        context.Add(new Category()
                        {
                            Name = name,
                            Description = description,
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
        [HttpDelete("deleteCategory")]
        public IActionResult RemoveCategory([FromQuery] int id)
        {
            try
            {
                using (var context = new ProductContext())
                {
                    if (context.Categories.Any(x => x.ID == id))
                    {
                        var category = context.Categories.Find(id);
                        if (category != null)
                        {
                            context.Categories.Remove(category);
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

        [HttpPut("putCategory")]
        public IActionResult UpdateCategory([FromQuery] int id, string name, string description)
        {
            try
            {
                using (var context = new ProductContext())
                {
                    if (context.Categories.Any(x => x.ID == id))
                    {
                        var category = context.Categories.Find(id);
                        if (category != null)
                        {
                            IActionResult applyChanges()
                            {
                                category.Name = name;
                                category.Description = description;
                                context.Categories.Update(category);
                                context.SaveChanges();
                                return Ok();
                            }

                            if (!category.Name.ToLower().Equals(name.ToLower()))
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
