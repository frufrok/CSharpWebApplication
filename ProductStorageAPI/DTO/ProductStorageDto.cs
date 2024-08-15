using SharedModels.Models;

namespace ProductStorageAPI.DTO
{
    public class ProductStorageDto : BaseDto
    {
        public int? StorageID { get; set; }
        public int? ProductID { get; set; }
        public int? Count { get; set; }
    }
}
