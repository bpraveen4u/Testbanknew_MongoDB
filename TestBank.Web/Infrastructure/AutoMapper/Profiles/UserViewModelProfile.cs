using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using TestBank.Entity.Models;
using TestBank.Web.ViewModels;
//using TestBank.Entity.Utilities;

namespace TestBank.Web.Infrastructure.AutoMapper.Profiles
{
    public class UserViewModelProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<UserViewModel, UserModel>()
                .ForMember(x => x.Id, o => o.MapFrom(m => m.UserId))
                .ForMember(x => x.UserId, o => o.MapFrom(m => m.UserId))
                .ForMember(x => x.FirstName, o => o.MapFrom(m => m.FirstName))
                .ForMember(x => x.LastName, o => o.MapFrom(m => m.LastName))
                .ForMember(x => x.Email, o => o.MapFrom(m => m.Email))
                .ForMember(x => x.Qualification, o => o.MapFrom(m => m.Qualification))
                .ForMember(x => x.Title, o => o.MapFrom(m => m.Title))
                ;
        }
    }
}