using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewMVCapp.Models
{
    public class Posts
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string TimeStamp { get; set; }
        public string CreatedBy { get; set; }
    }
}
