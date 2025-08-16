using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.DTOs;
using TeambaseInsurance.Presentation.ActionFilters;
using TeambaseInsurance.ServiceContracts;

namespace TeambaseInsurance.Presentation.Controllers
{
    [Route("api/v1/employee")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public EmployeeController(IServiceManager service)
        {
            _serviceManager = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            var result = await _serviceManager.EmployeeService.GetAllEmployeesAsync();
            
            if (result.IsSuccess)
            {
                return Ok(new CoreResponseModel().GetSuccessResponse(
                    ResponseMessages.success,
                    result.Value
                ));
            }

            return Ok(new CoreResponseModel().getFailResponse(
                "Failed to retrieve employees",
                result.Errors
            ));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            var result = await _serviceManager.EmployeeService.GetEmployeeByIdAsync(id);
            
            if (result.IsSuccess)
            {
                return Ok(new CoreResponseModel().GetSuccessResponse(
                    ResponseMessages.success,
                    result.Value
                ));
            }

            return Ok(new CoreResponseModel().getFailResponse(
                ResponseMessages.employeNotFound,
                result.Errors
            ));
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeDto createEmployeeDto)
        {
            var result = await _serviceManager.EmployeeService.CreateEmployeeAsync(createEmployeeDto);
            
            if (result.IsSuccess)
            {
                return Ok(new CoreResponseModel().GetSuccessResponse(
                    ResponseMessages.employeInserted,
                    result.Value
                ));
            }

            return Ok(new CoreResponseModel().getFailResponse(
                ResponseMessages.employeCreationFailed,
                result.Errors
            ));
        }
    }
}
