﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace TestBank.Web.Infrastructure.AutoMapper.Profiles.Resolvers
{
    public class RavenIdResolver
    {
        public static int Resolve(string ravenId)
        {
            var match = Regex.Match(ravenId, @"\d+");
            var idStr = match.Value;
            int id = int.Parse(idStr);
            if (id == 0)
                throw new System.InvalidOperationException("Id cannot be zero."); // TODO: use code contracts.
            return id;
        }
    }
}