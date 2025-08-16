using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeambaseInsurance.RepositoryContracts
{
    public interface IRepositoryManager
    {
        IEmployeeRepository EmployeeRepository { get; }
        Task SaveAsync();

    }
}
