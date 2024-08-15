using SharedModels.Models;
using CSharpWebApplication.OutModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace CSharpWebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ProductContext _context;
        private readonly IMemoryCache _cache;
        public CategoryController(ProductContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        [HttpGet("GetCategories")]
        public ActionResult<IEnumerable<CategoryOutModel>> GetCategories()
        {
            if (_cache.TryGetValue("categories", out List<CategoryOutModel> categories))
            {
                return categories;
            }
            else
            {
                try
                {
                    var result = _context.Categories.Select(x => new CategoryOutModel()
                    {
                        ID = x.ID,
                        Name = x.Name,
                        Description = x.Description
                    }).ToList();
                    _cache.Set("categories", result.Select(x => new CategoryOutModel() 
                        {
                            ID = x.ID, 
                            Name = x.Name.ToUpper(), 
                            Description = x.Description 
                        }).ToList(), TimeSpan.FromMinutes(30));
                    return Ok(result);
                }
                catch
                {
                    return StatusCode(500);
                }
            }
        }

        [HttpPost("add")]
        public ActionResult<int> Add([FromQuery] string name, string description)
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
                    _cache.Remove("categories");
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
        [HttpDelete("delete")]
        public IActionResult Delete([FromQuery] int id)
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
                        _cache.Remove("categories");
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
        public ActionResult Update([FromQuery] int id, string name, string description)
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
                            _cache.Remove("categories");
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

        [HttpGet(template:"GetCacheStatistics")]
        public ActionResult<MemoryCacheStatistics> GetCacheStatistics()
        {
            return _cache.GetCurrentStatistics();
        }

        [HttpGet(template:"GetCacheStatisticsUrl")]
        public ActionResult<string> GetCacheStatisticsUrl()
        {
            var filename = $"cashe_statistics_{DateTime.Now.ToBinary().ToString()}.txt";
            System.IO.File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles", filename), JsonSerializer.Serialize<MemoryCacheStatistics>(_cache.GetCurrentStatistics()));
            return $"https://{Request.Host.ToString()}/static/{filename}";
        }
    }
}
