using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestBank.Entity;
using TestBank.Entity.Models;

namespace TestBank.Data.MongoDB
{
    public static class EntityBsonClassMap
    {
        public static void Register()
        {
            var myConventions = new ConventionPack();
            myConventions.Add(new MemberSerializationOptionsConvention(typeof(Roles), new RepresentationSerializationOptions(BsonType.String)));

            var roleConvention = new ConventionProfile();
            roleConvention.SetSerializationOptionsConvention(new TypeRepresentationSerializationOptionsConvention(typeof(Roles), BsonType.String));

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

            if (!BsonClassMap.IsClassMapRegistered(typeof(User)))
            {
                BsonClassMap.RegisterClassMap<User>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIdMember(cm.GetMemberMap(c => c.Id));
                    cm.GetMemberMap(u => u.Role).SetRepresentation(global::MongoDB.Bson.BsonType.String);
                });
            }

            //ConventionRegistry.Register("test123", roleConvention, t => t == typeof(User));
            ConventionRegistry.Register("test", new TestBankConventions(), t => t == typeof(UserIdentity));
            ConventionRegistry.Register("test1", new TestBankConventions(), t => t == typeof(UserAnswer.Answer));

            if (!BsonClassMap.IsClassMapRegistered(typeof(UserAnswer)))
            {
                BsonClassMap.RegisterClassMap<UserAnswer>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIdMember(cm.GetMemberMap(c => c.Id));
                    cm.IdMemberMap.SetIdGenerator(new Int32IdGenerator());
                    cm.GetMemberMap(u => u.Status).SetRepresentation(global::MongoDB.Bson.BsonType.String);
                });
            }
        }

        
    }

    public class TestBankConventions : IConventionPack
    {
        public IEnumerable<IConvention> Conventions
        {
            get { return new List<IConvention> { new IgnoreIfNullConvention(true)
                , new MemberSerializationOptionsConvention(typeof(Roles), new RepresentationSerializationOptions(BsonType.String)) }; }
        }
    }
}
