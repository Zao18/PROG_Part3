using Microsoft.AspNetCore.Http;
using Moq;
using PROG_Part_2.Controllers;
using PROG_Part_2.Models;
using PROG_Part_2.Services;
using System.Security.Claims;
using Xunit;

namespace TestPROG
{
    public class UnitTest1
    {
        [Fact]
        public async Task Test1()
        {
            var claim = new Claims
            {
                Name = "Zao",
                HoursWorked = 40,
                HourlyRate = 120,
                AdditionalNotes = "I hope I did a good job",
                DocumentName = "Document.pdf"
            };

            Assert.Equal("Zao", claim.Name);
            Assert.Equal(40, claim.HoursWorked);
            Assert.Equal(120, claim.HourlyRate);
            Assert.Equal("I hope I did a good job", claim.AdditionalNotes);
            Assert.Equal("Document.pdf", claim.DocumentName);
        }
    }
}