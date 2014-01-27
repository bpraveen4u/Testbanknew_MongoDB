using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using TestBank.Entity.Models;
using TestBank.Web.ViewModels;
using TestBank.Web.Infrastructure.AutoMapper.Profiles.Resolvers;
using TestBank.Entity;
//using TestBank.Entity.Utilities;

namespace TestBank.Web.Infrastructure.AutoMapper.Profiles
{
    public class QuestionViewModelMapperProfile : Profile
    {
        protected override void Configure()
        {

            Mapper.CreateMap<QuestionModel, QuestionViewModel>()
                .ForMember(x => x.Id, o => o.MapFrom(m => m.Id))
                .ForMember(x => x.Category, o => o.MapFrom(m => m.Category))
                .ForMember(x => x.Description, o => o.MapFrom(m => m.Description))
                .ForMember(x => x.Sort, o => o.MapFrom(m => m.Sort))
                .ForMember(x => x.Nr, o => o.Ignore())
                .ForMember(x => x.Sort, o => o.Ignore())
                .IgnoreAllNonExisting()
                ;

            Mapper.CreateMap<QuestionDetailsModel, QuestionViewModel>()
                .ForMember(x => x.Id, o => o.MapFrom(m => m.Id))
                .ForMember(x => x.Category, o => o.MapFrom(m => m.Category))
                .ForMember(x => x.Description, o => o.MapFrom(m => m.Description))
                .ForMember(x => x.InstructorRemarks, o => o.MapFrom(m => m.InstructorRemarks))
                .ForMember(x => x.Weightage, o => o.MapFrom(m => m.Weightage))
                .ForMember(x => x.CorrectScore, o => o.MapFrom(m => m.CorrectScore))
                .ForMember(x => x.WrongScore, o => o.MapFrom(m => m.WrongScore))
                .ForMember(x => x.Sort, o => o.MapFrom(m => m.Sort))
                .ForMember(x => x.Options, o => o.MapFrom(m => m.Options))
                .ForMember(x => x.Nr, o => o.Ignore())
                .ForMember(x => x.Sort, o => o.Ignore())
                .IgnoreAllNonExisting()
                ;
            //Mapper.CreateMap<Option, QuestionViewModel.OptionViewModel>();

            Mapper.CreateMap<OptionModel, QuestionViewModel.OptionViewModel>()
                .ForMember(x => x.Id, o => o.MapFrom(m => m.Id))
                
                .ForMember(x => x.Text, o => o.MapFrom(m => m.Description))
                //.ForMember(x => x.Sort, o => o.MapFrom(m => m.Sort))
                .ForMember(x => x.IsCorrect, o => o.MapFrom(m => m.IsCorrect))
                .ForMember(x => x.Type, o => o.MapFrom(m => m.Type))
                .ForMember(x => x.Sort, o => o.Ignore())
                .IgnoreAllNonExisting()
                ;
        }
    }
    
    public class QuestionMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<QuestionViewModel, QuestionDetailsModel>()
                .ForMember(x => x.Id, o => o.Ignore())
                .ForMember(x => x.Category, o => o.MapFrom(m => m.Category))
                .ForMember(x => x.Description, o => o.MapFrom(m => m.Description))
                .ForMember(x => x.CorrectScore, o => o.MapFrom(m => m.CorrectScore))
                .ForMember(x => x.InstructorRemarks, o => o.MapFrom(m => m.InstructorRemarks))
                .ForMember(x => x.Weightage, o => o.MapFrom(m => m.Weightage))
                .ForMember(x => x.CorrectScore, o => o.MapFrom(m => m.CorrectScore))
                .ForMember(x => x.WrongScore, o => o.MapFrom(m => m.WrongScore))
                .ForMember(x => x.Sort, o => o.MapFrom(m => m.Sort))
                .ForMember(x => x.Options, o => o.MapFrom(m => m.Options))
                .IgnoreAllNonExisting()
                ;
            //Mapper.CreateMap<QuestionViewModel.OptionViewModel, Option>();

                Mapper.CreateMap<QuestionViewModel.OptionViewModel, OptionModel>()
                    .ForMember(x => x.Id, o => o.MapFrom(m => m.Id))
                    .ForMember(x => x.Description, o => o.MapFrom(m => m.Text))
                    //.ForMember(x => x, o => o.MapFrom(m => m.Id))
                    //.ForMember(x => x.Sort, o => o.MapFrom(m => m.Sort))
                    .ForMember(x => x.IsCorrect, o => o.MapFrom(m => m.IsCorrect))
                    
                    .ForMember(x => x.Type, o => o.MapFrom(m => m.Type)).AfterMap((x, y) => y.Type = OptionType.RadioButton)
                    .IgnoreAllNonExisting()
                ;
        }
    }
}