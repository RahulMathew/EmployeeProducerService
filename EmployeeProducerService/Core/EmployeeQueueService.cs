using BusinessObjects.DTO;
using BusinessLayer.Interfaces;
using EmployeeProducerService.Interface;

namespace EmployeeProducerService.Core
{
    public class EmployeeQueueService : IEmployeeQueueService
    {
        #region Declaration

        private IEmployeeRepository _employeeRepository;
        private List<Employee>? _employees;
        private Queue<Employee> _employeeQueue = new Queue<Employee>();

        #endregion Declaration

        #region Constructor

        public EmployeeQueueService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;

            GetEmployeeList();
        }

        #endregion Constructor

        #region Public Methods

        public Task<Employee> GetNextEmployee()
        {
            Employee employee = null!;

            if (_employeeQueue?.Count > 0)
            {
                employee = _employeeQueue.Dequeue();
            }

            return Task.FromResult(employee);
        }

        #endregion Public Methods

        #region Private Methods

        private async void GetEmployeeList()
        {
            _employees = await _employeeRepository.GenerateEmployees();

            if (_employees?.Count > 0)
            {
                foreach (Employee employee in _employees)
                {
                    _employeeQueue.Enqueue(employee);
                }
            }
        }

        #endregion Private Methods
    }
}
