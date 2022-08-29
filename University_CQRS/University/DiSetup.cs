using System.Reflection;
using University.Persistence.Repositories;

namespace University
{
    public static class DiSetup
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            foreach (var repo in GetRepositories())
            {
                services.AddScoped(repo);
            }

            return services;
        }

        private static IEnumerable<Type> GetRepositories()
        {
            var genericRepo = typeof(GenericRepository<>);
            // ReSharper disable once PossibleNullReferenceException
            return Assembly.GetAssembly(genericRepo).GetTypes().Where(myType => myType.IsClass &&
                myType.BaseType is
                {
                    IsGenericType: true
                } && myType.BaseType.GetGenericTypeDefinition() == genericRepo).ToArray();
        }
    }
}