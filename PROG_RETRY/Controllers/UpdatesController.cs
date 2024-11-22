using Microsoft.AspNetCore.Mvc;

namespace PROG_Part_2.Controllers
{
    public class HRController : Controller
    {
        public IActionResult LecturerHistory()
        {
            var claimHistory = ClaimsController._claimsList; // (ardalis, 2023)
            return View(claimHistory);
        }
    }
}

