using jovision.Services.Task47;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace jovision.Controllers.Task47
{
    [Route("Task49/[controller]")]
    [ApiController]
    public class ImgTransferOwnership : ControllerBase
    {
        private ImgService service;

        public ImgTransferOwnership(ImgService service)
        {
            this.service = service;
        }

        [HttpPut]
        public IActionResult transferOwnership([FromQuery] string? oldOwner, [FromQuery] string? newOwner)
        {
            if(string.IsNullOrWhiteSpace(newOwner) || string.IsNullOrWhiteSpace(oldOwner))
            {
                return BadRequest("Missing Data");
            }
            try
            {
                var updatedFiles = service.transferOwner(oldOwner, newOwner);

                if (updatedFiles == null || updatedFiles.Count == 0)
                {
                    return NotFound("No files found for the given owner.");
                }
                return Ok(updatedFiles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
