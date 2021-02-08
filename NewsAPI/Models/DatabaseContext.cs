using Microsoft.EntityFrameworkCore;
using NewsAppData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsAPI.Models
{
    public class DatabaseContext : DbContext
    {
        private readonly DbContextOptions _options;
        public DatabaseContext(DbContextOptions options) : base(options)
        {
            _options = options;
        }
        public DbSet<Article> Articles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
