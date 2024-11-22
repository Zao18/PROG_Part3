using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PROG_Part_2.Models;
using PROG_Part_2.Services;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace PROG_Part_2.Controllers
{
    public class ClaimsController : Controller
    {
        private readonly AzureFileShareService _fileShareService;

        public static List<Claims> _claimsList = new List<Claims>();
        private readonly IValidator<Claims> _claimValidator;
        private static int _nextClaimId = 1;

        public ClaimsController(AzureFileShareService fileShareService, IValidator<Claims> claimValidator)
        {
            _fileShareService = fileShareService;
            _claimValidator = claimValidator;
        }

        [HttpGet]
        public IActionResult SubmitClaim()
        {
            return View("ClaimView", new Claims());
        }

        [HttpPost]
        public async Task<IActionResult> SubmitClaim(Claims model, IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    string directoryName = "uploads";
                    string fileName = file.FileName;
                    await _fileShareService.UploadFileAsync(directoryName, fileName, stream);
                    model.DocumentName = fileName; 
                }
            }

            var validationResult = _claimValidator.Validate(model);

            if (!validationResult.IsValid) // (trevorc, 2011)
            {
                var rejectionReason = string.Join(" | ", validationResult.Errors.Select(e => e.ErrorMessage)); // (trevorc, 2011)
                model.Status = $"Rejected: {rejectionReason}"; // (trevorc, 2011)
                model.TotalPayment = 0; // (trevorc, 2011)
                model.ClaimId = _nextClaimId++; // (trevorc, 2011)
                _claimsList.Add(model); // (trevorc, 2011)

                return RedirectToAction("Index"); // (trevorc, 2011)
            }

            if (ModelState.IsValid)
            {
                model.ClaimId = _nextClaimId++;
                model.TotalPayment = model.HoursWorked * model.HourlyRate;
                model.Status = "Pending";

                _claimsList.Add(model);

                return RedirectToAction("Index");
            }

            return View("ClaimView", model);
        }

        public IActionResult Index()
        {
            return View(_claimsList);
        }
    }
}
