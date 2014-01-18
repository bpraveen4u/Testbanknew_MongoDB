using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestBank.Entity
{
    public interface IEntity<TKey>
    {
        /// <summary>
        /// Gets or sets the Id of the Entity.
        /// </summary>
        /// <value>Id of the Entity.</value>
        [BsonId]
        TKey Id { get; set; }
    }

    public interface IEntity : IEntity<int>
    {
        //int Id { get; set; }
    }
}
