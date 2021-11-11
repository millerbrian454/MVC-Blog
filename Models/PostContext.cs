using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace NewMVCapp.Models
{
    public class PostContext : DbContext
    {
        public PostContext(DbContextOptions<PostContext> options): base(options) { }
        public DbSet<Posts> Posts { get; set; }
    }
}
