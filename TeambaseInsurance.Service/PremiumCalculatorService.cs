using Ardalis.Result;
using AutoMapper;
using Entities.Models;
using Shared;
using Shared.DTOs;
using TeambaseInsurance.RepositoryContracts;
using TeambaseInsurance.ServiceContracts;

namespace TeambaseInsurance.Service
{
    public class PremiumCalculatorService : IPremiumCalculatorService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;

        public PremiumCalculatorService(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }

        public async Task<Result<PremiumResponseDto>> GetPremiumAsync(PremiumRequestDto request)
        {
            try
            {
                var pricingModel = ParsePricingModel(request.PricingModel);
                var prorationMethod = ParseProrationMethod(request.ProrationMethod);

                var employee = await _repositoryManager.EmployeeRepository.GetEmployeeByIdAsync(request.EmployeeId);
                if (employee == null)
                    return Result<PremiumResponseDto>.NotFound();

                var employeeDto = _mapper.Map<EmployeeDto>(employee);

                var fullPremium = CalculateFullPremium(employeeDto, pricingModel);

                var proratedPremium = ApplyProration(fullPremium, employeeDto, prorationMethod);
                 

                return Result<PremiumResponseDto>.Success(new PremiumResponseDto
                {
                    EmployeeId = employee.Id,
                    EmployeeName = $"{employee.FirstName} {employee.LastName}",
                    FullPremium = fullPremium,
                    ProratedPremium = proratedPremium,
                    PricingModel = request.PricingModel,
                    ProrationMethod = request.ProrationMethod,
                    CalculationDate = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return Result<PremiumResponseDto>.Error($"An error occurred while calculating premium: {ex.Message}");
            }
        }

        public decimal CalculateFullPremium(EmployeeDto employeeDto, PricingModel pricingModel)
        {
            int age = GetAge(employeeDto.DateOfBirth);

            switch (pricingModel)
            {
                case PricingModel.FlatRate:
                    return 1000m;

                case PricingModel.AgeRated:
                    return age * GetRatePerAgeGroup(age);

                case PricingModel.GenderAgeRated:
                    decimal basePremium = age * GetRatePerAgeGroup(age);
                    if (employeeDto.Gender.Equals("Female", StringComparison.OrdinalIgnoreCase) && age >= 18)
                        basePremium *= 1.5m;
                    return basePremium;

                default:
                    throw new ArgumentException("Invalid pricing model");
            }
        }

        public decimal ApplyProration(decimal fullPremium, EmployeeDto employeeDto, ProrationMethod method)
        {
            switch (method)
            {
                case ProrationMethod.ByDays:
                    int daysRemaining = (employeeDto.PolicyEndDate - employeeDto.JoinDate).Days;
                    return Math.Round(fullPremium / 365 * daysRemaining, 2);

                case ProrationMethod.ByMonths:
                    int monthsRemaining = ((employeeDto.PolicyEndDate.Year - employeeDto.JoinDate.Year) * 12)
                                          + employeeDto.PolicyEndDate.Month - employeeDto.JoinDate.Month;
                    return Math.Round(fullPremium / 12 * monthsRemaining, 2);

                default:
                    throw new ArgumentException("Invalid proration method");
            }
        }

        private Result<PremiumResponseDto> ValidatePremiumRequest(PremiumRequestDto request)
        {
            if (request == null)
                return Result<PremiumResponseDto>.Error("Request cannot be null");

            if (request.EmployeeId <= 0)
                return Result<PremiumResponseDto>.Error("EmployeeId must be greater than 0");

            if (string.IsNullOrWhiteSpace(request.PricingModel))
                return Result<PremiumResponseDto>.Error("PricingModel is required");

            if (string.IsNullOrWhiteSpace(request.ProrationMethod))
                return Result<PremiumResponseDto>.Error("ProrationMethod is required");

            return Result<PremiumResponseDto>.Success(null!);
        }

        private PricingModel ParsePricingModel(string pricingModel)
        {
            if (!Enum.TryParse<PricingModel>(pricingModel, true, out var result))
                throw new ArgumentException($"Invalid PricingModel: {pricingModel}");
            return result;
        }

        private ProrationMethod ParseProrationMethod(string prorationMethod)
        {
            if (!Enum.TryParse<ProrationMethod>(prorationMethod, true, out var result))
                throw new ArgumentException($"Invalid ProrationMethod: {prorationMethod}");
            return result;
        }

        private int GetRatePerAgeGroup(int age) => ((age / 10) + 1) * 100;
        
        private int GetAge(DateTime birthDate)
        {
            int age = DateTime.Today.Year - birthDate.Year;
            if (DateTime.Today < birthDate.AddYears(age)) age--;
            return age;
        }
    }
}
