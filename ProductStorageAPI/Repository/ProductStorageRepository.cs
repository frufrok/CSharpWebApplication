using AutoMapper;
using ProductStorageAPI.DTO;
using SharedModels.Models;
using ProductStorageAPI.DTO.Mapping;

namespace ProductStorageAPI.Repository
{
    public class ProductStorageRepository
    {
        private readonly ProductContext _context;
        private readonly Mapper _mapper;

        public ProductStorageRepository(ProductContext context)
        {
            _context = context;
            _mapper = new Mapper(new MapperConfiguration(config => config.AddProfile(new MappingProfile())));
        }

        public IEnumerable<StorageDto> GetStorages()
        {
            return _context.Storages.Select(x => _mapper.Map<StorageDto>(x)).ToList();
        }

        public IEnumerable<ProductStorageDto> GetProductsAllocation()
        {
            return _context.ProductStorages.Select(x => _mapper.Map<ProductStorageDto>(x)).ToList();
        }

        public int AddStorage(string name, string description)
        {
            var result = new Storage()
            {
                Name = name,
                Description = description
            };
            _context.Storages.Add(result);
            _context.SaveChanges();
            return result.ID;
        }

        public int GetStorageId(string name)
        {
            var result = _context.Storages.FirstOrDefault(x => x.Name.ToLower().Equals(name.ToLower()));
            if (result != null) return result.ID;
            else return -1;
        }
    }
}
