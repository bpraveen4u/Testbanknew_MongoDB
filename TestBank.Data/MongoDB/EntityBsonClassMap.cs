using MongoDB.Bson.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestBank.Entity;

namespace TestBank.Data.MongoDB
{
    public static class EntityBsonClassMap
    {
        public static void Register()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(Assessment)))
            {
                BsonClassMap.RegisterClassMap<Assessment>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIdMember(cm.GetMemberMap(c => c.Id));
                    cm.IdMemberMap.SetIdGenerator(new Int32IdGenerator());
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(Question)))
            {
                BsonClassMap.RegisterClassMap<Question>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIdMember(cm.GetMemberMap(c => c.Id));
                    cm.IdMemberMap.SetIdGenerator(new Int32IdGenerator());
                });
            }
        }
    }
}
