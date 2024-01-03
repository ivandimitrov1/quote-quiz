using Microsoft.Extensions.DependencyInjection;
using QuoteQuiz.Application.Quizes;
using QuoteQuiz.Application.Quizes.Interfaces;
using QuoteQuiz.Application.QuizManagement;
using QuoteQuiz.Application.QuizManagement.Interfaces;
using QuoteQuiz.Application.UserManagement;
using QuoteQuiz.Application.UserManagement.Interfaces;

namespace QuoteQuiz.Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<IUserManagementService, UserManagementService>();
            services.AddTransient<IQuizManagementService, QuizManagementService>();
            services.AddTransient<IQuizService, QuizService>();

            return services;
        }
    }
}
