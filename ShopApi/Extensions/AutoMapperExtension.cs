using AutoMapper;
using ShopApi.Database.Entities.ProductManagement;
using ShopApi.Dto;

namespace ShopApi.Extensions
{
    public class AutoMapperExtension : Profile
    {

        public AutoMapperExtension()
        {
            CreateMap<Product, ProductsGeneralDto>()
                .ForMember(d => d.Brand, o => o.MapFrom(s => s.Brand.Name));
        }
    }
}
