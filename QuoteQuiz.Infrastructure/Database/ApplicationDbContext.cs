using Microsoft.EntityFrameworkCore;
using QuoteQuiz.Application.Domain;
using QuoteQuiz.Application.Ports;
using System.Reflection;

namespace QuoteQuiz.Infrastructure.Database
{
    public class ApplicationDbContext : DbContext, IAsyncDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Quiz> Quizes { get; set; }
        public DbSet<UserAnswer> UserAnswers { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // we need to specify the entity type configurations
            // otherwise the EF will use the domain classes for database schema
            builder.ApplyEntityConfigurationsFromAssembly(Assembly.GetAssembly(typeof(ApplicationDbContext)));
        }
    }
}
