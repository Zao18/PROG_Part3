using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PROG_Part_2.Models;
using PROG_Part_2.Services;
using PROG_RETRY.Controllers;
using System.Linq;
using System.Threading.Tasks;

namespace PROG_Part_2.Controllers
{
    public class AdminController : Controller
    {
        private readonly AzureFileShareService _fileShareService;
        private readonly IValidator<Claims> _claimsValidator; // (dotnet-bot, 2024)

        public AdminController(AzureFileShareService fileShareService, IValidator<Claims> claimsValidator) // (dotnet-bot, 2024)
        {
            _fileShareService = fileShareService;
            _claimsValidator = claimsValidator; // (dotnet-bot, 2024)
        }

        public IActionResult PendingClaims()
        {
            var pendingClaims = ClaimsController._claimsList;
            return View(pendingClaims);
        }

        [HttpPost]
        public IActionResult ApproveClaim(int id) // (Microsoft, 2023)
        {
            var claim = ClaimsController._claimsList.FirstOrDefault(c => c.ClaimId == id);
            if (claim != null)
            {
                if (claim.HoursWorked < 0 || claim.HourlyRate < 0)
                {
                    claim.Status = "Rejected: Hours worked or Hourly rate cannot be negative.";
                }
                else
                {
                    var validationResult = _claimsValidator.Validate(claim); // (dotnet-bot, 2024)

                    if (validationResult.IsValid)
                    {
                        claim.Status = "Approved";
                    }
                    else
                    {
                        claim.Status = "Rejected: " + string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    }
                }
            }
            return RedirectToAction("PendingClaims");
        }

        [HttpPost]
        public IActionResult RejectClaim(int id) // (Microsoft, 2023)
        {
            var claim = ClaimsController._claimsList.FirstOrDefault(c => c.ClaimId == id);
            if (claim != null)
            {
                claim.Status = "Rejected";
            }
            return RedirectToAction("PendingClaims");
        }

        [HttpGet]
        public async Task<IActionResult> DownloadFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return BadRequest("File name cannot be null or empty");
            }
            try
            {
                var fileStream = await _fileShareService.DownLoadFileAsync("uploads", fileName);
                if (fileStream == null)
                {
                    return NotFound($"File '{fileName}' not found");
                }
                return File(fileStream, "application/octet-stream", fileName);
            }
            catch (Exception e)
            {
                return BadRequest($"Error downloading file: {e.Message}");
            }
        }
    }
}
