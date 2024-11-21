using Microsoft.AspNetCore.Mvc;
using PROG_Part_2.Models;
using PROG_Part_2.Services;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace PROG_Part_2.Controllers
{
    public class ClaimsController : Controller
    {
        private readonly AzureFileShareService _fileShareService;
        public ClaimsController(AzureFileShareService fileShareService)
        {
            _fileShareService = fileShareService;
        }

        public static List<Claims> _claimsList = new List<Claims>(); // (Stack Overflow, 2016)

        [HttpGet]
        public IActionResult SubmitClaim()
        {
            return View("ClaimView", new Claims());
        }

        [HttpPost]
        public async Task<IActionResult> SubmitClaim(Claims model, IFormFile file) // (Stack Overflow, 2016)
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
