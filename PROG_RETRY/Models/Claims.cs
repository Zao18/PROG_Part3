using System.ComponentModel.DataAnnotations;

namespace PROG_Part_2.Models
{
    public class Claims
    {
        [Key]
        public int ClaimId { get; set; }
        public string? Name { get; set; }
        public decimal HoursWorked { get; set; }
        public decimal HourlyRate { get; set; }
        public string? AdditionalNotes { get; set; }
        public string? DocumentName { get; set; }
        public long Size { get; set; }
        public DateTimeOffset? LastModified { get; set; }
        public string? Status { get; set; } = "Pending";
    }
}
