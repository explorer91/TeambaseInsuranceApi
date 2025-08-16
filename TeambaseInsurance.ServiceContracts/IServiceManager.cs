using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeambaseInsurance.ServiceContracts
{
    public interface IServiceManager
    {
        IEmployeeService EmployeeService { get; }
        IPremiumCalculatorService PremiumCalculatorService { get; }

    }
}
