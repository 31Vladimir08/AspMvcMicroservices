using AutoMapper;
using DataAccessLayer.Models;

using WebAPI.Models;

namespace WebAPI.AutoMapperProfile
{
    public class AutoMapProfiler : Profile
    {
        public AutoMapProfiler()
        {
            CreateProfile();
        }

        private void CreateProfile()
        {
            CreateMap<ProductDto, ProductEntity>();
            CreateMap<ProductEntity, ProductDto>()
                .ForMember(x => x.CategoryName, y => y.Ignore())
                .ForMember(x => x.SupplierName, y => y.Ignore());

            CreateMap<СategoryDto, СategoryEntity>();
            CreateMap<СategoryEntity, СategoryDto>();

            CreateMap<SupplierDto, SupplierEntity>();
            CreateMap<SupplierEntity, SupplierDto>();
        }
    }
}
