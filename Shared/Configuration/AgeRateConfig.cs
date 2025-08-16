namespace Shared.Configuration
{
    public class AgeRateConfig
    {
        public Dictionary<string, decimal> AgeRates { get; set; } = new Dictionary<string, decimal>();
        public decimal DefaultRate { get; set; } = 1000m;
    }
} 