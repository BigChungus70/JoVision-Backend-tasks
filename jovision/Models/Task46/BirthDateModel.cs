using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace jovision.Models.Task46
{
    public class BirthDateModel
    {
        [FromForm(Name = "year")]
        public string? Years { get; set; }
        [FromForm(Name = "month")]
        public string? Months { get; set; }
        [FromForm(Name = "day")]
        public string? Days { get; set; }
        [FromForm(Name = "name")]
        public string? Name { get; set; } = "anonymous";
    }
}
