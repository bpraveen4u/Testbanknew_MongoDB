using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestBank.Entity.Models;
using TestBank.Business.Exceptions;
using System.Linq.Expressions;
using TestBank.Data.Repositories;
using TestBank.Entity;
using TestBank.Entity.Sys;
using TestBank.Business.Validators;

namespace TestBank.Business.Manager
{
    public class UsersManager
    {
        private readonly IUserRepository repository;
        private readonly IKeyStoreRepository keyStoreRepository;

        //public UsersManager() 
        //    : this(new UserMongoRepository(), new KeyStoreMongoRepository())
        //{   
        //}

        public UsersManager(IUserRepository repository, IKeyStoreRepository keyStoreRepository)
        {
            this.repository = repository;
            this.keyStoreRepository = keyStoreRepository;
        }

        #region User Methods

        public IEnumerable<User> GetAll()
        {
            var users = repository.Get();
            if (users != null)
            {
                return users.ToList();
            }
            return null;
        }


        public User Get(string userId)
        {
            return repository.GetByID(userId);
            //var user = Get(x => x.Id == userId.ToLower());
            //if (user != null)
            //{
                //return GetUser(userId);
            //}
            //return null;
        }

        //private User Get(Func<User, bool> filter)
        //{
        //    var user = repository.SingleOrDefault<User>(filter);
        //    return user;
        //}

        public TestBankIdentity ValidateUserLogin(Credentials credentials)
        {
            if (string.IsNullOrWhiteSpace(credentials.User))
            {
                throw new BusinessException("userid cannot be empty.");
            }
            if (string.IsNullOrWhiteSpace(credentials.Password))
            {
                throw new BusinessException("password cannot be empty.");
            }

            var user = repository.GetByID(credentials.User); //<User>(RavenIdConverter.Convert(RavenIdPrefix.Users, credentials.User));
            if (user != null)
            {
                var identity = new UserIdentity()
                {
                    ApiKey = Guid.NewGuid().ToString(),
                    UserId = user.Id,
                    CreatedDate = DateTime.UtcNow,
                    IsExpired = false,
                    Role = user.Role
                };

                if (user.Role == Roles.Instructor)
                {
                    if (!string.IsNullOrWhiteSpace(credentials.Password))
                    {
                        if (!user.ValidatePassword(credentials.Password))
                        {
                            return null;
                        }
                    }
                }
                else if (user.Role == Roles.Student)
                {
                    //TODO : Get the actual test time insted of 120 or hide the userid
                    if (user.IsLocked)
                    {
                        return null;
                    }
                    else if (DateTime.UtcNow.Subtract(user.CreatedDate.Value).TotalMinutes > 65)
                    {
                        return null;
                    }
                }
                
                AddApiKey(identity);

                var tbIdentity = new TestBankIdentity()
                {
                    UserIdentity = identity
                };

                return tbIdentity;
            }
            return null;
        }

        private void AddApiKey(UserIdentity identity)
        {
            //var uniqueId = RavenIdConverter.Convert(RavenIdPrefix.ApiKeyStores, string.Format("{0:yyyyMMdd}", DateTime.UtcNow));
            var uniqueId = string.Format("{0:yyyyMMdd}", DateTime.UtcNow);
            var apiKeyStore = keyStoreRepository.GetByID(uniqueId);
            if (apiKeyStore == null)
            {
                apiKeyStore = new ApiKeyStore() { Id = uniqueId, CreatedDate = DateTime.UtcNow, LoginUsers = new List<UserIdentity>() };
                apiKeyStore.LoginUsers.Add(identity);
                keyStoreRepository.Insert(apiKeyStore);
            }
            else
            {
                foreach (var key in apiKeyStore.LoginUsers)
                {
                    if (key.UserId == identity.UserId)
                    {
                        //if (key.CreatedDate.AddMinutes(5) < DateTime.UtcNow)
                        //{
                            key.IsExpired = true;
                        //}
                    }
                }
                apiKeyStore.LoginUsers.Add(identity);
                keyStoreRepository.Update(apiKeyStore);
            }
            
        }

        public User Post(User user)
        {
            UserValidator validator = new UserValidator();
            var results = validator.Validate(user);
            if (results.IsValid)
            {
                //user.UserId = user.UserId.ToLower();
                
                if (Get(user.Id) == null)
                {
                    var userId = user.Id.ToLower();
                    user.Id = userId;
                    user.CreatedDate = user.ModifiedDate = DateTime.UtcNow;
                    user.CreatedUser = user.ModifiedUser = userId;
                    if (string.IsNullOrWhiteSpace(user.Password))
                    {
                        user.SetPassword(user.Id.ToLower() + "@111");
                    }
                    else
                    {
                        user.SetPassword(user.Password);
                        //user.IsSerializePassword = false;
                        user.Role = Roles.Instructor;
                    }
                    repository.Insert(user);
                    var newUser = repository.GetByID(user.Id);
                    return newUser;
                }
                else
                {
                    throw new BusinessException("Invalid Request Format", new List<string>() {"'User Id' already exists please choose another."});
                }
            }
            else
            {
                var errors = results.Errors.Select(e => e.ErrorMessage).ToList();
                throw new BusinessException(errors);
            }
        }

        public User Update(User user)
        {
            var userOriginal = repository.GetByID(user.Id);
            if (userOriginal != null)
            {
                UserValidator validator = new UserValidator();
                var results = validator.Validate(user);
                if (results.IsValid)
                {
                    if (userOriginal.Id.Equals(user.Id, StringComparison.InvariantCultureIgnoreCase))
                    {
                        userOriginal.Title = user.Title;
                        userOriginal.FirstName = user.FirstName;
                        userOriginal.LastName = user.LastName;
                        userOriginal.PhoneNumber = user.PhoneNumber;
                        userOriginal.ModifiedDate = DateTime.UtcNow;
                        userOriginal.Qualification = user.Qualification;
                        //userOriginal.Sort = user.Sort;
                        userOriginal.Email = user.Email;
                        userOriginal.Role = user.Role;
                        //repository.Save();
                    }
                    else
                    {
                        throw new BusinessException("Invalid Request Format", new List<string>() { "'UserId' can not be modified." });
                    }
                }
                else
                {
                    var errors = results.Errors.Select(e => e.ErrorMessage).ToList();
                    throw new BusinessException(errors);
                }
            }

            return user;
        }

        /// <summary>
        /// Get User without Prefix
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Returns User without id prefix</returns>
        //private User GetUser(User user)
        //{
        //    user.Id = user.Id.RemoveRavenIdPrefix();
        //    user.CreatedUser = user.CreatedUser.RemoveRavenIdPrefix();
        //    user.ModifiedUser = user.ModifiedUser.RemoveRavenIdPrefix();
        //    return user;
        //}

        //private User GetUser(string id)
        //{
        //    var user = repository.Load<User>(RavenIdConverter.Convert(RavenIdPrefix.Users, id));
        //    if (user == null)
        //    {
        //        return null;
        //    }
        //    return GetUser(user);
        //}

        #endregion
    }
}
