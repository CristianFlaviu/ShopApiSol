using System;
using System.Linq;
using AutoMapper;
using ShopApi.Database.Entities.ProductManagement;
using ShopApi.Dto;

namespace ShopApi.Extensions
{
    public class AutoMapperExtension : Profile
    {
        public AutoMapperExtension()
        {
            CreateMap<Product, ProductSearch>()
                .ForMember(d => d.Brand,
                    o => o.MapFrom(s => s.Brand.Name));

            CreateMap<Product, ProductDetails>()
                .ForMember(d => d.Brand,
                    o => o.MapFrom(s => s.Brand.Name))
                .ForMember(p => p.OldPrice, o => o.MapFrom(pp => pp.BasePrice))
                .ForMember(p => p.Category, o => o.MapFrom(pp => pp.Category.Name))
                .ForMember(p => p.NewPrice, o => o.MapFrom(pp => pp.BasePrice - Math.Round(pp.BasePrice * pp.Discount / 100, 2)));

            CreateMap<Product, ProductCarousel>()
                .ForMember(p => p.OldPrice, o => o.MapFrom(pp => pp.BasePrice))
                .ForMember(p => p.NewPrice, o => o.MapFrom(pp => pp.BasePrice - Math.Round(pp.BasePrice * pp.Discount / 100, 2)));

            CreateMap<ShoppingCartProduct, ProductShoppingList>()
                .ForMember(p => p.OldPrice, o => o.MapFrom(pp => pp.Product.BasePrice))
                .ForMember(p => p.NewPrice,
                    o => o.MapFrom(pp => pp.Product.BasePrice - Math.Round(pp.Product.BasePrice * pp.Product.Discount / 100, 2)))
                .ForMember(p => p.Title, o => o.MapFrom(pp => pp.Product.Title))
                .ForMember(p => p.PathToImage, o => o.MapFrom(pp => pp.Product.PathToImage))
                .ForMember(p => p.Discount, o => o.MapFrom(pp => pp.Product.Discount))
                .ForMember(p => p.Barcode, o => o.MapFrom(pp => pp.Product.Barcode))
                .ForMember(p => p.UnitsAvailable, o => o.MapFrom(pp => pp.Product.UnitsAvailable));

            CreateMap<FavoriteProduct, ProductShoppingList>()
                .ForMember(p => p.OldPrice, o => o.MapFrom(pp => pp.Product.BasePrice))
                .ForMember(p => p.NewPrice,
                    o => o.MapFrom(pp => pp.Product.BasePrice - Math.Round(pp.Product.BasePrice * pp.Product.Discount / 100, 2)))
                .ForMember(p => p.Title, o => o.MapFrom(pp => pp.Product.Title))
                .ForMember(p => p.PathToImage, o => o.MapFrom(pp => pp.Product.PathToImage))
                .ForMember(p => p.Discount, o => o.MapFrom(pp => pp.Product.Discount))
                .ForMember(p => p.Barcode, o => o.MapFrom(pp => pp.Product.Barcode))
                .ForMember(p => p.UnitsAvailable, o => o.MapFrom(pp => pp.Product.UnitsAvailable));

            CreateMap<Order, OrderTable>()
                .ForMember(p => p.Interest, o => o.MapFrom(pp => pp.DueDate.Date > DateTime.Now ?
                    0 : DateTime.Now.Subtract(pp.DueDate).Days * 0.5))
                .ForMember(p => p.ProductsCost,
                    o => o.MapFrom(pp =>
                        Math.Round(pp.OrderedProducts.Sum(x => x.PricePerProduct * x.Quantity),2)));

            CreateMap<OrderedProduct, ProductShoppingList>()
                .ForMember(p => p.OldPrice, o => o.MapFrom(pp => pp.Product.BasePrice))
                .ForMember(p => p.NewPrice, o => o.MapFrom(pp => pp.PricePerProduct))
                .ForMember(p => p.Title, o => o.MapFrom(pp => pp.Product.Title))
                .ForMember(p => p.PathToImage, o => o.MapFrom(pp => pp.Product.PathToImage))
                .ForMember(p => p.Discount, o => o.MapFrom(pp => pp.Product.Discount))
                .ForMember(p => p.Barcode, o => o.MapFrom(pp => pp.Product.Barcode));
        }
    }
}
