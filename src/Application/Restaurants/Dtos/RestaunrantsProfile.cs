using Application.Restaurants.Commands.CreateRestaurant;
using Application.Restaurants.Commands.UpdateRestaurant;
using AutoMapper;
using Domain.Entities;

namespace Application.Restaurants.Dtos;

public class RestaunrantsProfile : Profile
{
    public RestaunrantsProfile()
    {
        CreateMap<Restaurant, RestaurantDto>()
            .ForMember(d => d.City, opt => 
                opt.MapFrom(src => src.Address == null ? null : src.Address.City))
            .ForMember(d => d.Street, opt => 
                opt.MapFrom(src => src.Address == null ? null : src.Address.Street))
            .ForMember(d => d.PostalCode, opt => 
                opt.MapFrom(src => src.Address == null ? null : src.Address.PostalCode))
            .ForMember(d => d.Dishes, opt => opt.MapFrom(src => src.Dishes));

        CreateMap<CreateRestaurantCommand, Restaurant>()
            .ForMember(d => d.Address, opt => opt.MapFrom(
                src => new Address
                {
                    City = src.City,
                    PostalCode = src.PostalCode,
                    Street = src.Street   
                }));

        CreateMap<UpdateRestaurantCommand, Restaurant>();
    }
}