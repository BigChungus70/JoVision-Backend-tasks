using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace jovision.Controllers.Task45
{
    [Route("task45/[controller]")]
    [ApiController]
    public class BirthDate : ControllerBase
    {
        [HttpGet]
        public string Get([FromQuery] string? years,
                          [FromQuery] string? months,
                          [FromQuery] string? days,
                          [FromQuery] string name = "anonymous")
        {
            if (!int.TryParse(years, out _) ||
                !int.TryParse(months, out _) ||
                !int.TryParse(days, out _))
            {
                return $"Hello {name}, I can't calculate your age without knowing your birthdate!";
            }
            int age=0;
            if (DateTime.TryParse($"{years}/{months}/{days}", out DateTime birthDate))
               { 
                age = calcAge(birthDate);
               }
            return $"Hello {name}, This is your age: {age}";
        }

        private int calcAge(DateTime birthDate)
        {
            int age = DateTime.Today.Year - birthDate.Year;

            if(birthDate.Date > DateTime.Today.AddYears(-age))
            {
                age -= 1;
            }

            return age;
        }
    }
}
