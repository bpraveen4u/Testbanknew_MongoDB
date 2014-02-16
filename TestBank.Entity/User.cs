using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace TestBank.Entity
{
    public enum Roles
    {
        Student = 0,
        Instructor = 1,
        Administrator = 2
    }

    [CollectionNameAttribute("users")]
    public class User : IEntity<string>
    {
        [BsonId]
        public string Id { get; set; }
        public bool IsLocked { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Qualification { get; set; }
        public Roles Role { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedUser { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedUser { get; set; }
        //public bool IsSerializePassword { get; set; }
        const string ConstantSalt = "xi07cevs01q4#";
        [BsonElement]
        protected string HashedPassword { get; private set; }
        private string passwordSalt;
        [BsonElement]
        private string PasswordSalt
        {
            get
            {
                return passwordSalt ?? (passwordSalt = Guid.NewGuid().ToString("N"));
            }
            set { passwordSalt = value; }
        }
        [JsonIgnore]
        public string Password { get { return HashedPassword; } }

        public User SetPassword(string pwd)
        {
            HashedPassword = GetHashedPassword(pwd);
            return this;
        }

        private string GetHashedPassword(string pwd)
        {
            using (var sha = SHA256.Create())
            {
                var computedHash = sha.ComputeHash(Encoding.Unicode.GetBytes(PasswordSalt + pwd + ConstantSalt));
                return Convert.ToBase64String(computedHash);
            }
        }

        public bool ValidatePassword(string maybePwd)
        {
            if (HashedPassword == null)
                return true;
            return HashedPassword == GetHashedPassword(maybePwd);
        }
    }
}
