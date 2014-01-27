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
    public class UserAnswersInputViewModelProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<UserAnswersInputViewModel, UserAnswerModel>()
                .ForMember(x => x.AssessmentId, o => o.MapFrom(m => m.AssessmentId.ToString()))
                .ForMember(x => x.UserId, o => o.MapFrom(m => m.UserId))
                .ForMember(x => x.Id, o => o.Ignore())
                .ForMember(x => x.TimeToComplete, o => o.MapFrom(m => m.TimeToComplete))
                .ForMember(x => x.Answers, o => o.MapFrom(m => m.QuestionAnswers))
                .ForMember(x => x.IsTestCompleted, o => o.MapFrom(m => m.IsTestCompleted))
                ;
            ;
            //Mapper.CreateMap<UserAnswersInputViewModel.AnswerInputViewModel, UserAnswer.Answer>();

            //Mapper.CreateMap<UserAnswersInputViewModel.AnswerInputViewModel, UserAnswerModel.Answer>()
            //    .ForMember(x => x.Question, o => o.MapFrom(m => new QuestionReference { QuestionId = m.QuestionId.ToString(), Sort = m.Nr }))
            //    .ForMember(x => x.SelectedOption, o => o.MapFrom(m => m.SelectedOption))
            //    ;
            
        }
    }

    public class UserAnswersMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<UserAnswerModel, UserAnswersInputViewModel>()
                .ForMember(x => x.AssessmentId, o => o.MapFrom(m => m.AssessmentId.RemoveRavenIdPrefix()))
                .ForMember(x => x.UserId, o => o.MapFrom(m => m.UserId.RemoveRavenIdPrefix()))
                .ForMember(x => x.Id, o => o.MapFrom(m => m.Id.RemoveRavenIdPrefix()))
                .ForMember(x => x.TimeToComplete, o => o.MapFrom(m => m.TimeToComplete))
                .ForMember(x => x.StartDateTime, o => o.MapFrom(m => m.CreatedDate))
                .ForMember(x => x.IsTestCompleted, o => o.MapFrom(m => m.IsTestCompleted))
                ;

            //Mapper.CreateMap<UserAnswerModel.Answer,UserAnswersInputViewModel.AnswerInputViewModel>()
            //    .ForMember(x => x.QuestionId, o => o.MapFrom(m => m.Question.QuestionId.ConvertToInt32()))
            //    .ForMember(x => x.SelectedOption, o => o.MapFrom(m => m.SelectedOption))
            //    ;
        }
    }
}