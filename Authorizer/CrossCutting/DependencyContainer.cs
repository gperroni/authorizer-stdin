using Authorizer.Repositories;
using Authorizer.Repositories.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace Authorizer.CrossCutting
{
    public class DependencyContainer
    {
        protected static ServiceCollection ServiceCollection { get; set; }

        public static ServiceProvider ServiceProvider { get; set; }

        public static void Initialize()
        {
            ServiceCollection = new ServiceCollection();
            ConfigureServices();
            ServiceCollection.AddMemoryCache();
            ServiceProvider = ServiceCollection.BuildServiceProvider();

        }

        public static T GetService<T>() where T : class
        {
            return ServiceProvider.GetService<T>();
        }

        private static void ConfigureServices()
        {
            ServiceCollection.AddSingleton<IAccountRepository, AccountRepository>();
            ServiceCollection.AddSingleton<IMemoryCache, MemoryCache>();
        }
    }
}
