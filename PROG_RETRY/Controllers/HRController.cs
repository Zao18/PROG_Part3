using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PROG_Part_2.Models;
using System.Collections.Generic;
using System.Linq;

namespace PROG_Part_2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HRController : ControllerBase
    {
        [Authorize(Roles = "HR")]
        [HttpGet("ApprovedClaims")]
        public IActionResult GetApprovedClaims()
        {
            // Fetch approved claims from the in-memory list
            var approvedClaims = ClaimsController._claimsList.Where(c => c.Status == "Approved").ToList();

            if (!approvedClaims.Any())
                return NotFound("No approved claims found.");

            return Ok(approvedClaims);
        }

        [Authorize(Roles = "HR")]
        [HttpPost("UpdateLecturer")]
        public IActionResult UpdateLecturer([FromBody] Claims updatedClaim)
        {
            // Fetch the claim/lecturer by name (assuming 'Name' identifies the lecturer)
            var claim = ClaimsController._claimsList.FirstOrDefault(c => c.Name == updatedClaim.Name);

            if (claim == null)
                return NotFound("Lecturer not found.");

            // Update the claim/lecturer details
            claim.Name = updatedClaim.Name;
            claim.AdditionalNotes = updatedClaim.AdditionalNotes;
            claim.IsVerified = updatedClaim.IsVerified;

            return Ok("Lecturer details updated successfully.");
        }

        [Authorize(Roles = "HR")]
        [HttpGet("GenerateInvoice/{claimId}")]
        public IActionResult GenerateInvoice(int claimId)
        {
            // Fetch the claim by ID
            var claim = ClaimsController._claimsList.FirstOrDefault(c => c.ClaimId == claimId);

            if (claim == null)
                return NotFound("Claim not found.");

            // Generate a simple mock invoice (use SSRS for actual implementation)
            var invoiceData = $@"
                Invoice for Claim ID: {claim.ClaimId}
                Name: {claim.Name}
                Hours Worked: {claim.HoursWorked}
                Hourly Rate: {claim.HourlyRate}
                Total Payment: {claim.TotalPayment}
                Status: {claim.Status}";

            // Return the invoice as a plain text file
            var fileName = $"Invoice_{claim.ClaimId}.txt";
            var fileBytes = System.Text.Encoding.UTF8.GetBytes(invoiceData);
            return File(fileBytes, "text/plain", fileName);
        }

        [Authorize(Roles = "HR")]
        [HttpPost("ProcessClaims")]
        public IActionResult ProcessClaims()
        {
            // Example logic to auto-process claims (e.g., mark all pending claims as reviewed)
            var pendingClaims = ClaimsController._claimsList.Where(c => c.Status == "Pending").ToList();

            if (!pendingClaims.Any())
                return NotFound("No pending claims found.");

            foreach (var claim in pendingClaims)
            {
                claim.Status = "Processed";
            }

            return Ok($"{pendingClaims.Count} claims processed successfully.");
        }
    }
}

