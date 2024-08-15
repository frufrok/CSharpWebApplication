using SharedModels.Models;
using Microsoft.AspNetCore.Mvc;

namespace ProductStorageAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductStorageController : ControllerBase
    {
        private readonly ProductContext _context;

        public ProductStorageController(ProductContext context)
        {
            _context = context;
        }

        [HttpGet("GetProductsAllocation")]
        public ActionResult GetProductsAllocation()
        {
            return Ok();
        }
    }
}
