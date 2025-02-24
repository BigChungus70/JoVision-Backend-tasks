using jovision.Services.Task47;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace jovision.Controllers.Task47
{
    [Route("Task47/[controller]")]
    [ApiController]
    public class ImgUpdate : ControllerBase
    {
        private ImgService service;

        public ImgUpdate(ImgService service)
        {
            this.service = service;
        }
        [HttpPut]
        public async Task<IActionResult> update()
        {

            var form = await Request.ReadFormAsync();
            var owner = form["owner"].ToString();
            var img = form.Files["image"];



            if (img == null || img.Length == 0 || string.IsNullOrWhiteSpace(owner))
            {
                return BadRequest("Missing Data");
            }
            if (!Path.GetExtension(img.FileName).Equals(".jpg", StringComparison.CurrentCultureIgnoreCase))
            {
                return BadRequest("Invalid image type.");
            }

            try
            {
                return service.updateImage(img, owner) switch
                {
                    "File Does Not Exist" => BadRequest("File Does Not Exist"),
                    "Metadata File Does Not Exist" => BadRequest("Metadata File Does Not Exist"),
                    "Forbidden" => StatusCode(StatusCodes.Status403Forbidden, "Forbidden, you are not the owner"),
                    "Updated" => Ok("Updated Successfully"),
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
