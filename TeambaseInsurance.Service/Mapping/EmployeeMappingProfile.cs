using AutoMapper;
using Entities.Models;
using Shared.DTOs;

namespace TeambaseInsurance.Service.Mapping
{
    public class EmployeeMappingProfile : Profile
    {
        public EmployeeMappingProfile()
        {
            CreateMap<Employee, EmployeeDto>();

            CreateMap<CreateEmployeeDto, Employee>();
        }
    }
}