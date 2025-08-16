using Entities.Models;
using Microsoft.EntityFrameworkCore;
using TeambaseInsurance.RepositoryContracts;

namespace TeambaseInsurance.Repository
{
    public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public void CreateEmployeeAsync(Employee employee)
        {
            Create(employee);
        }

        public void DeleteEmployeeAsync(Employee employee)
        {
            Delete(employee);
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            try
            {
                return await FindAll(trackChanges: false).ToListAsync();

            }
            catch (Exception ex)
            {

                throw;
            }        }

        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            return await FindByCondition(e => e.Id.Equals(id), trackChanges: false)
                .FirstOrDefaultAsync();
        }

        public void UpdateEmployeeAsync(Employee employee)
        {
            Update(employee);
        }
    }
}
