namespace SharedModels.Models
{
    public class Product : BaseModel
    {
        public double Price { get; set; }
        public int CategoryID { get; set; }
        public virtual Category? Category { get; set; }
        //public virtual List<ProductStorage> ProductStorages { get; set; } = [];
    }
}
