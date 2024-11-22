using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROG_Part_2.Controllers;
using System.Text;

public class GenerateInvoiceModel : PageModel
{
    public string ErrorMessage { get; set; }

    public IActionResult OnGet(int id)
    {
        // Simulate fetching claim details
        var claim = ClaimsController._claimsList.FirstOrDefault(c => c.ClaimId == id);

        if (claim == null)
        {
            ErrorMessage = "Claim not found.";
            return Page();
        }

        // Simulate invoice generation
        var invoiceData = $@"
            Invoice for Claim ID: {claim.ClaimId}
            Name: {claim.Name}
            Hours Worked: {claim.HoursWorked}
            Hourly Rate: {claim.HourlyRate}
            Total Payment: {claim.TotalPayment}
            Status: {claim.Status}";

        // Return the invoice as a downloadable text file
        var fileBytes = Encoding.UTF8.GetBytes(invoiceData);
        var fileName = $"Invoice_Claim_{id}.txt";
        return File(fileBytes, "text/plain", fileName);
    }
}

