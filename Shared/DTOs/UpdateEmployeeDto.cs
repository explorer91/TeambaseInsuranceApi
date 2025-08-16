namespace Shared.DTOs
{
    public class UpdateEmployeeDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public DateTime? JoinDate { get; set; }
        public DateTime? PolicyEndDate { get; set; }
    }
} 