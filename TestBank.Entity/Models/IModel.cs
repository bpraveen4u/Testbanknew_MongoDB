using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestBank.Entity.Models
{
    public interface IModel : IEntity
    {
        string Id { get; set; }
        int Sort { get; set; }
        DateTime? CreatedDate { get; set; }
        string CreatedUser { get; set; }
        DateTime? ModifiedDate { get; set; }
        string ModifiedUser { get; set; }
    }

    //public interface IEntity
    //{
    //    string Id { get; set; }
    //}
}
