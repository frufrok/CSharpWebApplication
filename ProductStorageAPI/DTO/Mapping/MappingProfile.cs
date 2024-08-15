using AutoMapper;
using SharedModels.Models;

namespace ProductStorageAPI.DTO.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<ProductStorage, ProductStorageDto>(MemberList.Destination).ReverseMap();
            CreateMap<Storage, StorageDto>(MemberList.Destination).ReverseMap();
        }
    }
}
