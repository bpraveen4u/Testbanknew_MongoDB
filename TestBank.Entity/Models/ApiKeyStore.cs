using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestBank.Entity.Models
{
    public class ApiKeyStore //: IEntity
    {
        public string Id { get; set; }
        public List<UserIdentity> LoginUsers { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class UserIdentity
    {
        public string UserId { get; set; }
        public Guid ApiKey { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsExpired { get; set; }
        public Roles Role { get; set; }
    }
}
