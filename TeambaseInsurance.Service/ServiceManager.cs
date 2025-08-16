using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeambaseInsurance.RepositoryContracts;
using TeambaseInsurance.ServiceContracts;

namespace TeambaseInsurance.Service
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<IEmployeeService> _employeeService;
        private readonly Lazy<IPremiumCalculatorService> _premiumCalculatorService;

        public IEmployeeService EmployeeService => _employeeService.Value;
        public IPremiumCalculatorService PremiumCalculatorService => _premiumCalculatorService.Value;
        
        public ServiceManager(IRepositoryManager repository, AutoMapper.IMapper mapper)
        {
            _employeeService = new Lazy<IEmployeeService>(() => new EmployeeService(repository, mapper));
            _premiumCalculatorService = new Lazy<IPremiumCalculatorService>(() => new PremiumCalculatorService(repository, mapper));
        }
    }
}
