using Ardalis.Result;
using AutoMapper;
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
            var employee = await _repositoryManager.EmployeeRepository.GetEmployeeByIdAsync(request.EmployeeId);
            if (employee == null)
                return Result<PremiumResponseDto>.NotFound();

            var employeeDto = _mapper.Map<EmployeeDto>(employee);

            Enum.TryParse<PricingModel>(request.PricingModel, true, out var pricingModel);
            var fullPremium = CalculateFullPremium(employeeDto, pricingModel);

            Enum.TryParse<ProrationMethod>(request.ProrationMethod, true, out var prorationMethod);
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
                    if (employeeDto.Gender.ToLower() == "female" && age >= 18)
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

        private int GetRatePerAgeGroup(int age) => ((age / 10) + 1) * 100;

        private int GetAge(DateTime birthDate)
        {
            int age = DateTime.Today.Year - birthDate.Year;
            if (DateTime.Today < birthDate.AddYears(age)) age--;
            return age;
        }
    }
}
