using jovision.Services.Task47;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace jovision.Controllers.Task47
{
    [Route("Task49/[controller]")]
    [ApiController]
    public class ImgFilter : ControllerBase
    {
        private ImgService service;

        public ImgFilter(ImgService service)
        {
            this.service = service;
        }

        [HttpPost]
        public async Task<IActionResult> filter()
        {
            var form = await Request.ReadFormAsync();

            if (!DateTime.TryParse(form["CreationDate"], out DateTime creationDate))
            {
                return BadRequest("Invalid CreationDate format");
            }

            if (!DateTime.TryParse(form["ModificationDate"], out DateTime modificationDate))
            {
                return BadRequest("Invalid ModificationDate format");
            }

            var owner = form["Owner"].ToString();

            if (string.IsNullOrWhiteSpace(owner))
            {
                return BadRequest("Invalid Owner name");
            }

            if (!Enum.TryParse<FilterType>(form["FilterType"], out var filterType))
            {
                return BadRequest("Invalid FilterType value");
            }
            try
            {
                var result = filterType switch
                {
                    FilterType.ByModificationDate => service.getFilesByModificationDate(modificationDate),
                    FilterType.ByCreationDateDescending => service.getFilesByCreationDateDescending(creationDate),
                    FilterType.ByCreationDateAscending => service.getFilesByCreationDateAscending(creationDate),
                    FilterType.ByOwner => service.getFilesByOwner(owner),
                    _ => throw new ArgumentException("Invalid FilterType")
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }

        }

    }
    enum FilterType
    {
        ByModificationDate,
        ByCreationDateDescending,
        ByCreationDateAscending,
        ByOwner
    }
}
