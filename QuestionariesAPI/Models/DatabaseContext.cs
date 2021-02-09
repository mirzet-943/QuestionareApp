using Microsoft.EntityFrameworkCore;
using QuestionariesAppData;
using QuestionariesAppData.Models;
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
        public DbSet<Questionare> Questionares { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionResult> QuestionResults { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
