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
        }

        private void CreateProfile()
        {
            CreateMap<ProductEntity, Product>();
            CreateMap<Product, ProductEntity>();

            CreateMap<Сategory, СategoryEntity>();
            CreateMap<СategoryEntity, Сategory>();
        }
    }
}
