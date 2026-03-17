using Application.Users.Commands.UpdateUserDetails;
using AutoMapper;
using Domain.Entities;

namespace Application.Users.Dtos;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UpdateUserDetailsCommand, User>();
    }
}