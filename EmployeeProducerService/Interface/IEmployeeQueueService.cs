using BusinessObjects.DTO;

namespace EmployeeProducerService.Interface
{
    public interface IEmployeeQueueService
    {
        Task<Employee> GetNextEmployee();
    }
}
