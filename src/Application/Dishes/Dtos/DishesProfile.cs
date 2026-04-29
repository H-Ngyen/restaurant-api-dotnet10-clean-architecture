using Application.Dishes.Commands.CreateDish;
using AutoMapper;
using Domain.Entities;

namespace Application.Dishes.Dtos;

public class DishesProfile : Profile
{
    public DishesProfile()
    {
        CreateMap<Dish, DishDto>();
        CreateMap<CreateDishCommand, Dish>();
    }
}