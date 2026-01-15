using AutoMapper;
using ColdDream.Api.DTOs;
using ColdDream.Api.Models;

namespace ColdDream.Api.Helpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Product, ProductDto>();
        CreateMap<CreateProductDto, Product>();

        CreateMap<Banner, BannerDto>();
        
        CreateMap<Butler, ButlerDto>();
        
        CreateMap<Coupon, CouponDto>();
        
        CreateMap<CustomTour, CustomTourDto>();
        CreateMap<CreateCustomTourDto, CustomTour>();
        
        CreateMap<Guide, GuideDto>();
        CreateMap<CreateGuideDto, Guide>();
        
        CreateMap<Inspiration, InspirationDto>();
        
        CreateMap<TourRoute, TourRouteDto>();
        CreateMap<CreateTourRouteDto, TourRoute>();
        
        CreateMap<Booking, BookingDto>();
        CreateMap<CreateBookingDto, Booking>();
    }
}
