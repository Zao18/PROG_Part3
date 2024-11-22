using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace PROG_Part_2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HRController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public HRController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "HR")]
        [HttpGet("Lecturers")]
        public async Task<IActionResult> GetLecturers()
        {
            var lecturers = await _context.Lecturers.ToListAsync();
            return Ok(lecturers);
        }

        [HttpPost("UpdateLecturer")]
        public async Task<IActionResult> UpdateLecturer([FromBody] Lecturer updatedLecturer)
        {
            var lecturer = await _context.Lecturers.FindAsync(updatedLecturer.Id);
            if (lecturer == null) return NotFound();

            lecturer.Name = updatedLecturer.Name;
            lecturer.Email = updatedLecturer.Email;
            lecturer.ContactNumber = updatedLecturer.ContactNumber;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("GenerateInvoice/{claimId}")]
        public async Task<IActionResult> GenerateInvoice(int claimId)
        {
            // Fetch claim details and generate invoice
            var claim = await _context.Claims.FindAsync(claimId);
            if (claim == null) return NotFound();

            // Logic for generating invoice (using SSRS or Crystal Reports)
            var report = GenerateReport(claim);
            return File(report, "application/pdf", "Invoice.pdf");
        }

        private byte[] GenerateReport(Claim claim)
        {
            // Implement report generation logic here
            return new byte[0];
        }
    }

}
