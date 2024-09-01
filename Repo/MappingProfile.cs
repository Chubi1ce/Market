using AutoMapper;
using Market.Models;
using Market.Models.DTO;

namespace Market.Repo
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Product,ProductDTO>(MemberList.Destination).ReverseMap();
            CreateMap<ProductGroup,ProductGroupDTO>(MemberList.Destination).ReverseMap();
            CreateMap<Storage,StorageDTO>(MemberList.Destination).ReverseMap();
        }
    }
}
