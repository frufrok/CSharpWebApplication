namespace CSharpWebApplication.Models
{
    public class Category : BaseModel
    {
        public virtual List<Product> Products { get; set; } = [];
    }
}
