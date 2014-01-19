using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using TestBank.API.WebHost.Infrastructure.AutoMapper.Profiles;

namespace TestBank.API.WebHost.Infrastructure.AutoMapper
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.AddProfile(new TestModelMapperProfile());
            Mapper.AddProfile(new TestModelReverseMapperProfile());
        }
    }
}