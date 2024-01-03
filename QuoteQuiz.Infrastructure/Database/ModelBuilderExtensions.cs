using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QuoteQuiz.Infrastructure.Database
{
    public static class ModelBuilderExtensions
    {
        // source: https://github.com/aspnet/EntityFrameworkCore/blob/master/src/EFCore/ModelBuilder.cs
        public static ModelBuilder ApplyEntityConfigurationsFromAssembly(this ModelBuilder builder, Assembly assembly, Func<Type, bool> predicate = null)
        {
            MethodInfo applyEntityConfigurationMethod = typeof(ModelBuilder)
                .GetMethods()
                .Single(
                    e => e.Name == nameof(ModelBuilder.ApplyConfiguration)
                         && e.ContainsGenericParameters
                         && e.GetParameters().SingleOrDefault()?.ParameterType.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>));

            foreach (Type type in assembly.GetTypes())
            {
                if (!predicate?.Invoke(type) ?? false)
                {
                    continue;
                }

                foreach (Type @interface in type.GetInterfaces())
                {
                    if (!@interface.IsGenericType)
                    {
                        continue;
                    }

                    if (@interface.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>))
                    {
                        object instance;

                        try
                        {
                            instance = Activator.CreateInstance(type, builder);
                        }
                        catch (Exception)
                        {
                            instance = Activator.CreateInstance(type);
                        }

                        MethodInfo target = applyEntityConfigurationMethod.MakeGenericMethod(@interface.GenericTypeArguments[0]);
                        target.Invoke(builder, new[] { instance });
                    }
                }
            }

            return builder;
        }
    }
}
