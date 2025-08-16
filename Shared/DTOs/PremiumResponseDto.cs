namespace Shared.DTOs
{
    public class PremiumResponseDto
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = null!;
        public decimal FullPremium { get; set; }
        public decimal ProratedPremium { get; set; }
        public string PricingModel { get; set; } = null!;
        public string ProrationMethod { get; set; } = null!;
        public DateTime CalculationDate { get; set; }
    }
} 