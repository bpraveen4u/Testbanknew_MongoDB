using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using TestBank.Entity.Models;
using TestBank.Web.ViewModels;
//using TestBank.Entity.Utilities;
//using TestBank.Infrastructure.Extensions;

namespace TestBank.Web.Infrastructure.AutoMapper.Profiles
{
    public class UserAnswersResultMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<UserAnswerModel, UserAnswersResultViewModel>()
                .ForMember(x => x.AssessmentId, o => o.MapFrom(m => m.AssessmentId))
                .ForMember(x => x.UserId, o => o.MapFrom(m => m.UserId))
                .ForMember(x => x.Id, o => o.MapFrom(m => m.Id))
                .ForMember(x => x.TimeToComplete, o => o.MapFrom(m => m.TimeToComplete)).AfterMap((o, x) => x.TimeToComplete = string.Format("{0:%m} min. {0:%s} sec.", TimeSpan.FromSeconds(o.TimeToComplete)))
                .ForMember(x => x.StartDateTime, o => o.MapFrom(m => m.CreatedDate))
                .ForMember(x => x.IsTestCompleted, o => o.MapFrom(m => m.IsTestCompleted))
                .ForMember(x => x.Percentage, o => o.MapFrom(m => m.Percentage))
                .ForMember(x => x.Status, o => o.MapFrom(m => m.Status.ToString()))
                .ForMember(x => x.UserUniqueName, o => o.MapFrom(m => m.UserUniqueName))
                .ForMember(x => x.UserName, o => o.MapFrom(m => m.UserName))
                .ForMember(x => x.AssessmentName, o => o.MapFrom(m => m.AssessmentName))

                ;

            //Mapper.CreateMap<UserAnswer.Answer,UserAnswersInputViewModel.AnswerInputViewModel>()
            //    .ForMember(x => x.QuestionId, o => o.MapFrom(m => m.Question.QuestionId))
            //    .ForMember(x => x.SelectedOption, o => o.MapFrom(m => m.SelectedOption))
            //    ;
        }
    }
}