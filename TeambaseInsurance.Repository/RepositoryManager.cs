using TeambaseInsurance.RepositoryContracts;

namespace TeambaseInsurance.Repository
{
    public class RepositoryManager  : IRepositoryManager
    {
        private readonly RepositoryContext _repositoryContext;
        private readonly Lazy<IEmployeeRepository> _employeeRepository;

        public IEmployeeRepository EmployeeRepository => _employeeRepository.Value;

        public RepositoryManager(RepositoryContext repositoryContext)
        {

            _repositoryContext = repositoryContext;
            _employeeRepository = new Lazy<IEmployeeRepository>(() => new EmployeeRepository(_repositoryContext));
        }

        public async Task SaveAsync()
        {
            await _repositoryContext.SaveChangesAsync();
        }
    }
}
