using System.ComponentModel.DataAnnotations;

namespace PROG_Part_2.Models
{
    public class Claims
    {
        [Key]
        public int ClaimId { get; set; }
        public string? Name { get; set; }
        public double? HoursWorked { get; set; }
        public double? HourlyRate { get; set; }
        public double? TotalPayment { get; set; }
        public string? AdditionalNotes { get; set; }
        public string? DocumentName { get; set; }
        public long Size { get; set; }
        public DateTimeOffset? LastModified { get; set; }
        public string? Status { get; set; } = "Pending";
    }
}
