namespace SharedModels.Models
{
    public class ProductStorage : BaseModel
    {
        public int? StorageID { get; set; }
        public int? ProductID { get; set; }
        public int? Count { get; set; }
        public virtual Product? Product { get; set; }
        public virtual Storage? Storage { get; set; }
    }
}
