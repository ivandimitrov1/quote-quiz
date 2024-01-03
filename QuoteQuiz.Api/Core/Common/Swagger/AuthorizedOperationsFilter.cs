using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace QuoteQuiz.Core.Common.Swagger
{
    public class AuthorizedOperationsFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.ApiDescription.ActionDescriptor is ControllerActionDescriptor controller)
            {
                AuthorizeAttribute attribute = controller.MethodInfo.GetCustomAttribute<AuthorizeAttribute>() ?? controller.ControllerTypeInfo.GetCustomAttribute<AuthorizeAttribute>();
                if (attribute == null)
                {
                    return;
                }

                if (operation.Security == null)
                {
                    operation.Security = new List<OpenApiSecurityRequirement>();
                }

                var securityDefinitions = new OpenApiSecurityRequirement
                {
                    { GetSecurityScheme(attribute.AuthenticationSchemes ?? "jwt"), new string[] { } },
                };

                operation.Security.Add(securityDefinitions);
            }
        }

        private OpenApiSecurityScheme GetSecurityScheme(string name)
        {
            return new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = name,
                    Type = ReferenceType.SecurityScheme,
                },
            };
        }
    }
}
