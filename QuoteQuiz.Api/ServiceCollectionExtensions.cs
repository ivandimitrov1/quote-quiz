using QuoteQuiz.Api.Core;

namespace QuoteQuiz.Api
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {
            services.AddTransient<ITokenService, TokenService>();

            return services;
        }
    }
}
