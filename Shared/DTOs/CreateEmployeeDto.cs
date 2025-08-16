using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs
{
    public class CreateEmployeeDto
    {
        [Required]
        public string FirstName { get; set; } = null!;
        [Required]
        public string LastName { get; set; } = null!;
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string Gender { get; set; } = null!;
        [Required]
        public DateTime JoinDate { get; set; }
        [Required]
        public DateTime PolicyEndDate { get; set; }
    }
} 