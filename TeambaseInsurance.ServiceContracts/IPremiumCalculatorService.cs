using Ardalis.Result;
using Shared;
using Shared.DTOs;

namespace TeambaseInsurance.ServiceContracts
{
    public interface IPremiumCalculatorService
    {
        decimal CalculateFullPremium(EmployeeDto employee, PricingModel pricingModel);
        decimal ApplyProration(decimal fullPremium, EmployeeDto employee, ProrationMethod prorationMethod);
        Task<Result<PremiumResponseDto>> GetPremiumAsync(PremiumRequestDto request);
    }
}
