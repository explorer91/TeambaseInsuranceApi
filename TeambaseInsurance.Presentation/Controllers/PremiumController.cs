using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.DTOs;
using TeambaseInsurance.ServiceContracts;

namespace TeambaseInsurance.Presentation.Controllers
{
    [Route("api/v1/premium")]
    [ApiController]
    public class PremiumController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public PremiumController(IServiceManager service)
        {
            _serviceManager = service;
        }

        [HttpGet("calculate")]
        public async Task<IActionResult> GetPremium([FromQuery] PremiumRequestDto request)
        {
            var result = await _serviceManager.PremiumCalculatorService.GetPremiumAsync(request);

            if (result.IsSuccess)
            {
                return Ok(new CoreResponseModel().GetSuccessResponse(
                    "Premium calculated successfully",
                    result.Value
                ));
            }
            else if (result.IsNotFound())
            {
                return Ok(new CoreResponseModel().getFailResponse(
                    ResponseMessages.employeNotFound,
                    "Employee not found"
                ));
            }
            
            return Ok(new CoreResponseModel().getFailResponse(
                "Failed to calculate premium",
                result.Errors
            ));
        }
    }
}
