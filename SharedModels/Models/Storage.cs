namespace SharedModels.Models
{
    public class Storage : BaseModel
    {
        public virtual List<ProductStorage> ProductStorages { get; set; } = [];
    }
}
