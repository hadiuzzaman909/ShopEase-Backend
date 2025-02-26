using AutoMapper;
using ShopEase.DTOs.Request;
using ShopEase.DTOs.Response;
using ShopEase.Models;

namespace ShopEase.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Product Mappings
            CreateMap<ProductRequest, Product>();
            CreateMap<Product, ProductResponse>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category));

            // Category Mappings
            CreateMap<CategoryRequest, Category>();
            CreateMap<Category, CategoryResponse>()
                .ForMember(dest => dest.SubCategories, opt => opt.MapFrom(src => src.SubCategories));

            // SubCategory Mappings
            CreateMap<Category, SubCategoryResponse>();

            // Cart → CartResponse
            CreateMap<Cart, CartResponse>()
                .ForMember(dest => dest.CartItems, opt => opt.MapFrom(src => src.CartItems));

            // CartItem → CartItemResponse
            CreateMap<CartItem, CartItemResponse>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.Product.Price));

            // CartItemRequest → CartItem (For Adding Items)
            CreateMap<CartItemRequest, CartItem>();

            CreateMap<Order, OrderResponse>();
            CreateMap<OrderItem, OrderItemResponse>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name));

        }
    }
}