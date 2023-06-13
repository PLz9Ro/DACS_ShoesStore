using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DACS_ShoesStore.Models
{
    public class News
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public string Description { get; set; }
        public string Detail { get; set; }
        public string image { get; set; }   
    }
}