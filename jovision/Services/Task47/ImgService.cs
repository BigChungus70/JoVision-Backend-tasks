
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
namespace jovision.Services.Task47
{
    public class ImgService
    {
        private string getImgPath(string filename)
        {
            return Path.Combine(Directory.GetCurrentDirectory(),"Resources", filename);
        }

        public string saveImage(IFormFile img, string owner)
        {
            
                string imgPath = getImgPath(img.FileName);
                string metadataPath = Path.ChangeExtension(imgPath, ".json");

                if (File.Exists(imgPath))
                {
                    return "File Already Created";
                }

                var fileStream = new FileStream(imgPath, FileMode.CreateNew);
                img.CopyTo(fileStream);

                var metadata = new { Owner = owner, CreatedAt = DateTime.Now, LastModified = DateTime.Now };
                File.WriteAllText(metadataPath, JsonSerializer.Serialize(metadata));

                return "Success";
           

        }

        public string deleteImage(string fileName, string ownerName)
        {
            string imgPath = getImgPath(fileName);
            string metadataPath = Path.ChangeExtension(imgPath, ".json");

            if (!File.Exists(imgPath))
            {
                return "File Does Not Exist";
            }

            var metadata = JsonSerializer.Deserialize<Dictionary<string, Object>>(File.ReadAllText(metadataPath));
            
            if(metadata == null)
            {
                return "Metadata File Does Not Exist";
            }


            if (!ownerName.Equals(metadata["Owner"].ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return "Forbidden";
            }
            File.Delete(imgPath);
            File.Delete(metadataPath);
            return "Deleted";

        }
    }
}
