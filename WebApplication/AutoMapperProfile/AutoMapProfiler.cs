using WebApplication.ModelsUI;

namespace WebApplication.AutoMapperProfile
{
    using AutoMapper;

    using DataAccessLayer.Models;

    using WebApplication.Models;

    public class AutoMapProfiler: Profile
    {
        public AutoMapProfiler()
        {
            CreateProfile();
            CreateProfileUI();
        }

        private void CreateProfile()
        {
            CreateMap<ProductEntity, Product>();
            CreateMap<Product, ProductEntity>();

            CreateMap<Сategory, СategoryEntity>();
            CreateMap<СategoryEntity, Сategory>();

            CreateMap<Supplier, SupplierEntity>();
            CreateMap<SupplierEntity, Supplier>();
        }

        private void CreateProfileUI()
        {
            CreateMap<ProductUI, Product>();
            CreateMap<Product, ProductUI>()
                .ForMember(x => x.CategoryName, y => y.Ignore())
                .ForMember(x => x.SupplierName, y => y.Ignore());
        }
    }
}
