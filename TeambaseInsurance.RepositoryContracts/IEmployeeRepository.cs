using Entities.Models;

namespace TeambaseInsurance.RepositoryContracts
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllEmployeesAsync();
        Task<Employee> GetEmployeeByIdAsync(int id);
        void CreateEmployeeAsync(Employee employee);
        void UpdateEmployeeAsync(Employee employee);
        void DeleteEmployeeAsync(Employee employee);
    }
}
