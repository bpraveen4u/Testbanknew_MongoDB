using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using TestBank.Entity.Models;
using TestBank.Web.ViewModels;
//using TestBank.Entity.Utilities;
using TestBank.Infrastructure.Extensions;

namespace TestBank.Web.Infrastructure.AutoMapper.Profiles
{
    public class AssessmentViewModelMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<AssessmentModel, AssessmentViewModel>()
                .ForMember(x => x.Id, o => o.MapFrom(m => m.Id))
                .ForMember(x => x.TestName, o => o.MapFrom(m => m.Name))
                .ForMember(x => x.ModifiedDate, o => o.MapFrom(m => m.ModifiedDate))
                .ForMember(x => x.ModifiedUser, o => o.MapFrom(m => m.ModifiedUser))
                .ForMember(x => x.Enable, o => o.MapFrom(m => m.Enable))
                .IgnoreAllNonExisting();
            ;


            Mapper.CreateMap<AssessmentDetailsModel, AssessmentViewModel>()
                .ForMember(x => x.Id, o => o.MapFrom(m => m.Id))
                .ForMember(x => x.TestName, o => o.MapFrom(m => m.Name))
                .ForMember(x => x.MaxOptions, o => o.MapFrom(m => m.MaxOptions))
                .ForMember(x => x.CreatedDate, o => o.MapFrom(m => m.CreatedDate))
                .ForMember(x => x.CreatedUser, o => o.MapFrom(m => m.CreatedUser))
                .ForMember(x => x.ModifiedDate, o => o.MapFrom(m => m.ModifiedDate))
                .ForMember(x => x.ModifiedUser, o => o.MapFrom(m => m.ModifiedUser))
                .ForMember(x => x.Enable, o => o.MapFrom(m => m.Enable))
                .ForMember(x => x.Questions, o => o.MapFrom(m => m.Questions.Select(q => new QuestionReferenceViewModel() { QuestionId = q.Id, Description = q.Description, Sort = q.Sort })))
                .ForMember(x => x.QuestionIds, o => o.MapFrom(m => m.Questions.Select(q => q.Id)))
                .ForMember(x => x.ShortLink, o => o.MapFrom(m => m.ShortLink))
                .ForMember(x => x.Link, o => o.MapFrom(m => m.Link))
                .IgnoreAllNonExisting();
                ;
            ;

            Mapper.CreateMap<QuestionModel, QuestionReferenceViewModel>()
                .ForMember(x => x.QuestionId, o => o.MapFrom(m => m.Id))
                .ForMember(x => x.Description , o => o.MapFrom(m => m.Description))
                .ForMember(x => x.Sort, o => o.MapFrom(m => m.Sort))
                ;
        }
    }

    public class AssessmentMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<AssessmentViewModel, AssessmentDetailsModel>()
                .ForMember(x => x.Id, o => o.Ignore())
                .ForMember(x => x.Name, o => o.MapFrom(m => m.TestName))
                .ForMember(x => x.MaxOptions, o => o.MapFrom(m => m.MaxOptions))
                .ForMember(x => x.CreatedDate, o => o.MapFrom(m => m.CreatedDate))
                .ForMember(x => x.CreatedUser, o => o.MapFrom(m => m.CreatedUser))
                .ForMember(x => x.ModifiedDate, o => o.MapFrom(m => m.ModifiedDate))
                .ForMember(x => x.ModifiedUser, o => o.MapFrom(m => m.ModifiedUser))
                .ForMember(x => x.Enable, o => o.MapFrom(m => m.Enable))
                .ForMember(x => x.Questions, o => o.MapFrom(m => m.Questions))
                .ForMember(x => x.ShortLink, o => o.MapFrom(m => m.ShortLink))
                .ForMember(x => x.Link, o => o.MapFrom(m => m.Link))
                .IgnoreAllNonExisting();
            ;

            Mapper.CreateMap<QuestionReferenceViewModel, QuestionModel>()
                .ForMember(x => x.Id, o => o.MapFrom(m => m.QuestionId))
                .ForMember(x => x.Sort, o => o.MapFrom(m => m.Sort))
                .IgnoreAllNonExisting();
                ;
        }
    }
}