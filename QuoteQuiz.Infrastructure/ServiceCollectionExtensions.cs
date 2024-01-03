using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuoteQuiz.Application.Ports;
using QuoteQuiz.Infrastructure.Database.Users;
using QuoteQuiz.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using QuoteQuiz.Infrastructure.Database.Quizes;

namespace QuoteQuiz.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DbContext")))
                .AddScoped<IAsyncDbContext>(x => x.GetRequiredService<ApplicationDbContext>());

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IQuizRepository, QuizRepository>();

            return services;
        }
    }
}
