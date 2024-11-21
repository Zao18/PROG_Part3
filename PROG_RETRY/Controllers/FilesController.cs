using Microsoft.AspNetCore.Mvc;
using PROG_Part_2.Models;
using PROG_Part_2.Services;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PROG_Part_2.Controllers
{
    public class FilesController : Controller
    {
        private readonly AzureFileShareService _fileShareService;
        private const long FileSize = 1 * 1024 * 1024; // (Nilsson, 2008)
        private readonly string[] FileTypes = { ".pdf", ".png", ".jpg" }; // (Parshuram Kalvikatte, 2017)

        public FilesController(AzureFileShareService fileShareService)
        {
            _fileShareService = fileShareService;
        }

        public async Task<IActionResult> Index()
        {
            List<Claims> files;
            try
            {
                files = await _fileShareService.ListFileAsync("uploads");
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"Failed to load files: {ex.Message}";
                files = new List<Claims>();
            }
            return View(files);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("file", "Please select a file to upload.");
                return View("Index");
            }

            if (file.Length > FileSize)
            {
                ModelState.AddModelError("file", "The file size exceeds the limit of 1 MB."); // (Nilsson, 2008)
                return View("Index");
            }

            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant(); // (Parshuram Kalvikatte, 2017)
            if (!FileTypes.Contains(fileExtension))
            {
                ModelState.AddModelError("file", "Invalid file type");
                return View("Index");
            }

            try
            {
                using (var stream = file.OpenReadStream())
                {
                    string directoryName = "uploads";
                    string fileName = file.FileName;
                    await _fileShareService.UploadFileAsync(directoryName, fileName, stream);

                    TempData["UploadedFileName"] = fileName;
                }
                TempData["Message"] = $"File '{file.FileName}' uploaded successfully.";
            }
            catch (Exception e)
            {
                TempData["Message"] = $"File upload failed: {e.Message}";
            }

            return RedirectToAction("Index");
        }
    }
}
