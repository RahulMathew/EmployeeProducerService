using EmployeeProducerService.Interface;
using Common.ServiceBus.Interfaces;

namespace EmployeeProducerService.HostedServices
{
    public class ProducerService : IHostedService, IDisposable
    {
        #region Declaration

        private Timer _timer = null!;
        private int _topicGenerationTimeIntervalInMinutes = 1;
        private readonly ITopicPublisher _topicPublisher;
        private IEmployeeQueueService _employeeQueueService;

        #endregion Declaration

        #region Constructor

        public ProducerService(IEmployeeQueueService employeeQueueService,
            ITopicPublisher topicPublisher,
            IConfiguration configuration) 
        {
            _topicPublisher = topicPublisher;
            _employeeQueueService = employeeQueueService;

            _topicGenerationTimeIntervalInMinutes = configuration.GetValue<int>("TopicGenerationTimeIntervalInMinutes");
        }

        #endregion Constructor

        #region Public Methods

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(GenerateMessages, null, TimeSpan.Zero, TimeSpan.FromMinutes(_topicGenerationTimeIntervalInMinutes));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        #endregion Public Methods

        #region Private Methods

        private async void GenerateMessages(object? message)
        {
            try
            {
                message = await _employeeQueueService.GetNextEmployee();

                if (message != null)
                {
                    await _topicPublisher.ProduceAsync(message);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion Private Methods
    }
}
