using jovision.Models.Task46;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace jovision.Controllers.Task46
{
    [Route("task46/[controller]")]
    [ApiController]
    public class BirthDate : ControllerBase
    {
        [HttpPost] 
        public string Post([FromForm] BirthDateModel req)
        {
            if (!int.TryParse(req.Years, out _) ||
                !int.TryParse(req.Months, out _) ||
                !int.TryParse(req.Days, out _))
            {
                return $"Hello {req.Name}, I can't calculate your age without knowing your birthdate!";
            }
            int age=0;
            if (DateTime.TryParse($"{req.Years}/{req.Months}/{req.Days}", out DateTime birthDate))
               { 
                age = calcAge(birthDate);
               }
            return $"Hello {req.Name}, This is your age: {age}";
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
