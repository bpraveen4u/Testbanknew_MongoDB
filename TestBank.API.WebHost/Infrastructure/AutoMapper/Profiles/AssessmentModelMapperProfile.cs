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
    public class AssessmentModelMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<AssessmentDetailsModel, Assessment>()
                .ForMember(x => x.Id, o => o.MapFrom(m => m.Id))
                .ForMember(x => x.Name, o => o.MapFrom(m => m.Name))
                .ForMember(x => x.Description, o => o.MapFrom(m => m.Description))
                .ForMember(x => x.Questions, o => o.MapFrom(m => m.Questions != null? m.Questions.Select(q => q.Id) : null))
                .ForMember(x => x.ModifiedDate, o => o.MapFrom(m => m.ModifiedDate))
                .ForMember(x => x.ModifiedUser, o => o.MapFrom(m => m.ModifiedUser))
                .ForMember(x => x.CreatedDate, o => o.MapFrom(m => m.CreatedDate))
                .ForMember(x => x.CreatedUser, o => o.MapFrom(m => m.CreatedUser))
                .ForMember(x => x.Duration, o => o.MapFrom(m => m.Duration))
                .ForMember(x => x.Status, o => o.MapFrom(m => m.Status))
                .ForMember(x => x.Sort, o => o.MapFrom(m => m.Sort))
                ;

            Mapper.CreateMap<Assessment, AssessmentModel>()
                .Include<Assessment, AssessmentDetailsModel>()
                .ForMember(x => x.Id, o => o.MapFrom(m => m.Id))
                .ForMember(x => x.Name, o => o.MapFrom(m => m.Name))
                .ForMember(x => x.Description, o => o.MapFrom(m => m.Description))
                .ForMember(x => x.ModifiedDate, o => o.MapFrom(m => m.ModifiedDate))
                .ForMember(x => x.ModifiedUser, o => o.MapFrom(m => m.ModifiedUser))
                .ForMember(x => x.Duration, o => o.MapFrom(m => m.Duration))
                .ForMember(x => x.Links, o => o.Ignore())
                ;
            ///.ReverseMap();

            Mapper.CreateMap<Assessment, AssessmentDetailsModel>()
                .ForMember(x => x.Questions, o => o.MapFrom(m => m.Questions != null ? m.Questions.Select(q => new QuestionModel() { Id = q }) : null))
                //.ForMember(x => x.Questions, o => o.Ignore())
                .ForMember(x => x.CreatedDate, o => o.MapFrom(m => m.CreatedDate))
                .ForMember(x => x.CreatedUser, o => o.MapFrom(m => m.CreatedUser))
                .ForMember(x => x.Status, o => o.MapFrom(m => m.Status))
                ;
        }
    }
}