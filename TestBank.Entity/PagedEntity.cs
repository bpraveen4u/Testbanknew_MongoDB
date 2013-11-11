using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestBank.Entity
{
    public class PagedEntity<T> where T : class
    {
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public List<T> PagedData { get; set; }
    }
}
