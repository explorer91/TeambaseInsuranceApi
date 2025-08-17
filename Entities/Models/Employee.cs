using Entities.Contracts;

namespace Entities.Models
{
    public class Employee : ITimeStampedModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateOnly DateOfBirth { get; set; }
        public string Gender { get; set; } = null!;
        public DateTime JoinDate { get; set; }
        public DateTime PolicyEndDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastModified { get; set; }
    }
}
