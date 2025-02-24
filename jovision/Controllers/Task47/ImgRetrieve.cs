using jovision.Services.Task47;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace jovision.Controllers.Task47
{
    [Route("Task48/[controller]")]
    [ApiController]
    public class ImgRetrieve : ControllerBase
    {
        private ImgService service;

        public ImgRetrieve(ImgService service)
        {
            this.service = service;
        }

        [HttpGet]
        public IActionResult get([FromQuery] string? fileName, [FromQuery] string? ownerName)
        {
            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(ownerName))
            {
                return BadRequest("Missing Data");
            }
            try
            {
                var result = service.retrieveImage(fileName, ownerName);

                return result.Message switch
                {
                    "File Does Not Exist" => NotFound("File Does Not Exist"),
                    "Metadata File Does Not Exist" => NotFound("Metadata File Does Not Exist"),
                    "Forbidden" => StatusCode(StatusCodes.Status403Forbidden, "Forbidden, you are not the owner"),
                    "Ok" => File(result.ImageData, result.ContentType),
                    _ => StatusCode(500, "Internal Server Error")
                };
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
        
    }
}
