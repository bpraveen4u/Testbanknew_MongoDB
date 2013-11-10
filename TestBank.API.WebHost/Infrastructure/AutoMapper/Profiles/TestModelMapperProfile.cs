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
    public class TestModelMapperProfile : Profile
    {
        //private UrlHelper _urlHelper;
        //public TestModelMapperProfile(HttpRequestMessage request)
        //public TestModelMapperProfile(HttpRequestMessage request = null)
        //{
        //    _urlHelper = null;
        //    if (request !=null)
        //    {
        //        _urlHelper = new UrlHelper(request);
        //    }
            
        //}

        //private object UrlResolver(Assessment assessment)
        //{
        //    if (_urlHelper != null)
        //    {
        //        return _urlHelper.Link("Assessments", new { id = assessment.Id });
        //    }
        //    return "12";
        //}

        protected override void Configure()
        {
            Mapper.CreateMap<Assessment, AssessmentModel>()
                .ForMember(x => x.Id, o => o.MapFrom(m => m.Id))
                .ForMember(x => x.Name, o=> o.MapFrom(m => m.Name))
                .ForMember(x => x.Description, o => o.MapFrom(m => m.Description))
                .ReverseMap();
        }
    }
}