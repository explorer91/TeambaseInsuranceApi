using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs
{
    public class PremiumRequestDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "EmployeeId must be greater than 0")]
        public int EmployeeId { get; set; }

        [Required]
        [RegularExpression("^(FlatRate|AgeRated|GenderAgeRated)$", ErrorMessage = "PricingModel must be one of: FlatRate, AgeRated, GenderAgeRated")]
        public string PricingModel { get; set; } = null!;

        [Required]
        [RegularExpression("^(ByDays|ByMonths)$", ErrorMessage = "ProrationMethod must be one of: ByDays, ByMonths")]
        public string ProrationMethod { get; set; } = null!;
    }
} 