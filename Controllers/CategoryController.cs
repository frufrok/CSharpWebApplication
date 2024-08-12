using CSharpWebApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace CSharpWebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        ProductContext _context = new ProductContext();
        [HttpGet("getCategories")]
        public ActionResult<List<Category>> GetCategories()
        {
            try
            {
                var categories = _context.Categories.Select(x => new Category()
                {
                    ID = x.ID,
                    Name = x.Name,
                    Description = x.Description
                });
                return Ok(categories);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpPost("postCategory")]
        public ActionResult<int> PostCategory([FromQuery] string name, string description)
        {
            try
            {
                if (!_context.Categories.Any(x => x.Name.ToLower().Equals(name.ToLower())))
                {
                    var category = new Category()
                    {
                        Name = name,
                        Description = description,
                    };
                    _context.Add(category);
                    _context.SaveChanges();
                    return Ok(category.ID);
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
        [HttpDelete("deleteCategory")]
        public IActionResult DeleteCategory([FromQuery] int id)
        {
            try
            {
                if (_context.Categories.Any(x => x.ID == id))
                {
                    var category = _context.Categories.Find(id);
                    if (category != null)
                    {
                        _context.Categories.Remove(category);
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

        [HttpPut("putCategory")]
        public ActionResult PutCategory([FromQuery] int id, string name, string description)
        {
            try
            {
                if (_context.Categories.Any(x => x.ID == id))
                {
                    var category = _context.Categories.Find(id);
                    if (category != null)
                    {
                        ActionResult applyChanges()
                        {
                            category.Name = name;
                            category.Description = description;
                            _context.Categories.Update(category);
                            _context.SaveChanges();
                            return Ok();
                        }

                        if (!category.Name.ToLower().Equals(name.ToLower()))
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
