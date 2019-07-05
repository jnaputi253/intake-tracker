using IntakeTracker.Database.Configuration;
using IntakeTracker.Repositories;
using IntakeTracker.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace IntakeTracker
{
    public class Container
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceCollection _serviceCollection;

        public Container(IConfiguration configuration, IServiceCollection serviceCollection)
        {
            _configuration = configuration;
            _serviceCollection = serviceCollection;
        }

        public void RegisterDependencies()
        {
            RegisterMongo();
            RegisterGeneralDependencies();
        }

        private void RegisterMongo()
        {
            _serviceCollection.Configure<IntakeTrackerConfiguration>(
                _configuration.GetSection(nameof(IntakeTrackerConfiguration)));

            _serviceCollection.AddSingleton(serviceProvider =>
                serviceProvider.GetRequiredService<IOptions<IntakeTrackerConfiguration>>().Value);
        }

        private void RegisterGeneralDependencies()
        {
            _serviceCollection.AddSingleton<ItemService>();
            _serviceCollection.AddSingleton<ItemRepository>();
        }
    }
}
