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
        private static int _nextClaimId = 1;

        public ClaimsController(AzureFileShareService fileShareService)
        {
            _fileShareService = fileShareService;
        }

        [HttpGet]
        public IActionResult SubmitClaim()
        {
            return View("ClaimView", new Claims());
        }

        [HttpPost]
        public async Task<IActionResult> SubmitClaim(Claims model, IFormFile file)
        {
            if (ModelState.IsValid)
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
