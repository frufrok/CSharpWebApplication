namespace SharedModels.Models
{
    public class Category : BaseModel
    {
        public virtual List<Product> Products { get; set; } = [];
    }
}
