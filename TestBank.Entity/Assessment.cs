using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestBank.Entity
{
    [CollectionNameAttribute("assessments")]
    public class Assessment : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Duration { get; set; }
        public int Sort { get; set; }
        public string TestName { get; set; }
        public string[] Questions { get; set; }
        [BsonIgnore]
        public List<Question> QuestionDetails { get; set; }
        public string Link { get; set; }
        public string ShortLink { get; set; }
        public TestStatus Status { get; set; }
        public bool Enable { get; set; }
        public int MaxOptions { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedUser { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedUser { get; set; }
    }

    public enum TestStatus
    {
        None = 0,
        Started = 1,
        Completed = 2,
    }
}
