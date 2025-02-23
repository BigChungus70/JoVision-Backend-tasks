using jovision.Services.Task47;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace jovision.Controllers.Task47
{
    [Route("Task47/[controller]")]
    [ApiController]
    public class ImgCreate : ControllerBase
    {

        private ImgService service;

        public ImgCreate(ImgService service)
        {
            this.service = service;
        }

        [HttpPost]
        public async Task<IActionResult> post()
        {
            var form = await Request.ReadFormAsync();
            var owner = form["owner"].ToString();
            var img = form.Files["image"];



            if (img == null || img.Length == 0 || string.IsNullOrWhiteSpace(owner))
            {
                return BadRequest("Missing Data");
            }
            if (Path.GetExtension(img.FileName).ToLower() != ".jpg")
            {
                return BadRequest("Invalid image type.");
            }


            try
            {
                return service.saveImage(img, owner) switch
                {
                    "File Already Created" => BadRequest("File Already Created"),
                    "Success" => Created("", "File Created"),
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
