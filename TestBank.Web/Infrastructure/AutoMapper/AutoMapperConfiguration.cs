using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using TestBank.Web.Infrastructure.AutoMapper.Profiles;

namespace TestBank.Web.Infrastructure.AutoMapper
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            // TODO: It would make sense to add all of those automatically with an IoC.
            Mapper.AddProfile(new QuestionViewModelMapperProfile());
            Mapper.AddProfile(new QuestionMapperProfile());
            Mapper.AddProfile(new AssessmentViewModelMapperProfile());
            Mapper.AddProfile(new AssessmentMapperProfile());
            Mapper.AddProfile(new UserAnswersInputViewModelProfile());
            Mapper.AddProfile(new UserAnswersResultMapperProfile());
            Mapper.AddProfile(new UserAnswersMapperProfile());
            Mapper.AddProfile(new UserViewModelProfile());
        }
    }
}