using BusinessLayer.Core;
using BusinessLayer.Interfaces;
using Common.AdoUtils.Core;
using Common.AdoUtils.Interfaces;
using Common.ServiceBus.Core;
using Common.ServiceBus.Interfaces;
using DBContextLayer.Core;
using DBContextLayer.Interfaces;
using EmployeeProducerService.Core;
using EmployeeProducerService.HostedServices;
using EmployeeProducerService.Interface;

namespace EmployeeProducerService
{
    public static class DependencyRegistration
    {
        #region Public Methods

        public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHostedService<ProducerService>();

            string confluenceKafkaServiceBusUrl = configuration.GetValue<string>("ConfluenceKafkaServiceBusUrl");
            string employeeServiceTopicName = configuration.GetValue<string>("EmployeeServiceTopicName");
            services.AddSingleton<ITopicPublisher>(new ConfluentKafkaTopicPublisher(confluenceKafkaServiceBusUrl, employeeServiceTopicName));

            services.AddSingleton<IEmployeeRepository, EmployeeRepository>();
            services.AddSingleton<IEmployeeDBContext, EmployeeDBContext>();
            services.AddSingleton<IEmployeeQueueService, EmployeeQueueService>();

            services.AddSingleton<IAdoDal>(new SqlServerDal());

            return services;
        }

        #endregion Public Methods
    }
}
