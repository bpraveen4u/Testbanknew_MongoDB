using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using TestBank.Entity;
using TestBank.API.WebHost.Models;
using System.Web.Http.Routing;
using System.Net.Http;

namespace TestBank.API.WebHost.Infrastructure.AutoMapper.Profiles
{
    public class QuestionModelMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<QuestionDetailsModel, Question>()
                .ForMember(x => x.Id, o => o.MapFrom(m => m.Id))
                .ForMember(x => x.Description, o => o.MapFrom(m => m.Description))
                .ForMember(x => x.ModifiedDate, o => o.MapFrom(m => m.ModifiedDate))
                .ForMember(x => x.ModifiedUser, o => o.MapFrom(m => m.ModifiedUser))
                .ForMember(x => x.CreatedDate, o => o.MapFrom(m => m.CreatedDate))
                .ForMember(x => x.CreatedUser, o => o.MapFrom(m => m.CreatedUser))
                .ForMember(x => x.Category, o => o.MapFrom(m => m.Category))
                .ForMember(x => x.Sort, o => o.MapFrom(m => m.Sort))
                .ForMember(x => x.Weightage, o => o.MapFrom(m => m.Weightage))
                .ForMember(x => x.CorrectScore, o => o.MapFrom(m => m.CorrectScore))
                .ForMember(x => x.WrongScore, o => o.MapFrom(m => m.WrongScore))
                .ForMember(x => x.Options, o => o.MapFrom(m => m.Options))
                .ForMember(x => x.Sort, o => o.MapFrom(m => m.Sort))
                ;

            Mapper.CreateMap<Question, QuestionModel>()
                .Include<Question, QuestionDetailsModel>()
                .ForMember(x => x.Id, o => o.MapFrom(m => m.Id))
                .ForMember(x => x.Description, o => o.MapFrom(m => m.Description))
                .ForMember(x => x.ModifiedDate, o => o.MapFrom(m => m.ModifiedDate))
                .ForMember(x => x.ModifiedUser, o => o.MapFrom(m => m.ModifiedUser))
                .ForMember(x => x.Links, o => o.Ignore())
                ;
            ///.ReverseMap();
            ///
            Mapper.CreateMap<Question, QuestionDetailsModel>()
                .ForMember(x => x.Options, o => o.MapFrom(m => m.Options))
                .ForMember(x => x.CreatedDate, o => o.MapFrom(m => m.CreatedDate))
                .ForMember(x => x.CreatedUser, o => o.MapFrom(m => m.CreatedUser))
                .ForMember(x => x.Category, o => o.MapFrom(m => m.Category))
                .ForMember(x => x.Sort, o => o.MapFrom(m => m.Sort))
                .ForMember(x => x.Weightage, o => o.MapFrom(m => m.Weightage))
                .ForMember(x => x.CorrectScore, o => o.MapFrom(m => m.CorrectScore))
                .ForMember(x => x.WrongScore, o => o.MapFrom(m => m.WrongScore))
                .ForMember(x => x.Sort, o => o.MapFrom(m => m.Sort))
                ;
                
        }
    }
}