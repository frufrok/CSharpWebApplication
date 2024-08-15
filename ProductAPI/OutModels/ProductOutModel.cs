using SharedModels.Models;

namespace CSharpWebApplication.OutModels
{
    public class ProductOutModel : BaseModel
    {
        public double Price { get; set; }
        public int CategoryID { get; set; }
    }
}
