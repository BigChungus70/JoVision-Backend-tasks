using jovision.Services.Task47;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace jovision.Controllers.Task47
{
    [Route("Task47/[controller]")]
    [ApiController]
    public class ImgDelete : ControllerBase
    {
        private ImgService service;

        public ImgDelete(ImgService service)
        {
            this.service = service;
        }

        [HttpDelete]
        public IActionResult delete([FromQuery] string? fileName, [FromQuery] string? ownerName)
        {
            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(ownerName))
            {
                return BadRequest("Missing Data");
            }
            try
            {
                return service.deleteImage(fileName, ownerName) switch
                {
                    "File Does Not Exist" => BadRequest("File Does Not Exist"),
                    "Metadata File Does Not Exist" => BadRequest("Metadata File Does Not Exist"),
                    "Forbidden" => Forbid("Forbidden, you are not the owner"),
                    "Deleted" => Ok("Deleted Successfully"),
                    _ => StatusCode(500, "InternalServerError")
                };
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}
