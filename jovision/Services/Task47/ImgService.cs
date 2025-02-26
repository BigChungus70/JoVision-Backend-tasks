
using jovision.Models.Task48;
using jovision.Models.Task49;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
namespace jovision.Services.Task47
{
    public class ImgService
    {
        private string getImgPath(string filename)
        {
            return Path.Combine(Directory.GetCurrentDirectory(),"Resources", filename);
        }

        public string saveImage(IFormFile img, string ownerName)
        {
            
                string imgPath = getImgPath(img.FileName);
                string metadataPath = Path.ChangeExtension(imgPath, ".json");

                if (File.Exists(imgPath))
                {
                    return "File Already Created";
                }

                var fileStream = new FileStream(imgPath, FileMode.CreateNew);
                img.CopyTo(fileStream);

                var metadata = new { Owner = ownerName, CreatedAt = DateTime.Now, LastModified = DateTime.Now };
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
        public string updateImage(IFormFile img, string ownerName)
        {
            string imgPath = getImgPath(img.FileName);
            string metadataPath = Path.ChangeExtension(imgPath, ".json");

            if (!File.Exists(imgPath))
            {
                return "File Does Not Exist";
            }

            var metadata = JsonSerializer.Deserialize<Dictionary<string, Object>>(File.ReadAllText(metadataPath));

            if (metadata == null)
            {
                return "Metadata File Does Not Exist";
            }


            if (!ownerName.Equals(metadata["Owner"].ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return "Forbidden";
            }

            var fileStream = new FileStream(imgPath, FileMode.Create);
            img.CopyTo(fileStream);

            metadata["LastModified"] = DateTime.Now;
            File.WriteAllText(metadataPath, JsonSerializer.Serialize(metadata));

            return "Updated";
        }
        public ImgDTO retrieveImage(string fileName, string ownerName)
        {
            string imgPath = getImgPath(fileName);
            string metadataPath = Path.ChangeExtension(imgPath, ".json");

            if (!File.Exists(imgPath))
            {
                return new ImgDTO
                {
                    Message = "File Does Not Exist"
                };
            }

            var metadata = JsonSerializer.Deserialize<Dictionary<string, Object>>(File.ReadAllText(metadataPath));

            if (metadata == null)
            {
                return new ImgDTO
                {
                    Message = "Metadata File Does Not Exist"
                };
            }


            if (!ownerName.Equals(metadata["Owner"].ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return new ImgDTO
                {
                    Message = "Forbidden"
                };
            }
            return new ImgDTO
            {
                ImageData = File.ReadAllBytes(imgPath),
                ContentType = "image/jpg",
                Message = "Ok"

            };
        }
        public List<FilteredDTO> getFilesByModificationDate(DateTime modificationDate)
        {
            var file = from x in getAllMetadata()
                       where x.ModificationDate < modificationDate
                       select new FilteredDTO
                       {
                           FileName = x.FileName,
                           Owner = x.Owner
                       } ;

            return file.ToList();
        }
        public List<FilteredDTO> getFilesByCreationDateDescending(DateTime creationDate)
        {
            var file = from x in getAllMetadata()
                       where x.CreationDate > creationDate
                       orderby x.CreationDate descending
                       select new FilteredDTO
                       {
                           FileName = x.FileName,
                           Owner = x.Owner
                       };

            return file.ToList();
        }
        public List<FilteredDTO> getFilesByCreationDateAscending(DateTime creationDate)
        {
            var file = from x in getAllMetadata()
                       where x.CreationDate > creationDate
                       orderby x.CreationDate ascending
                       select new FilteredDTO
                       {
                           FileName = x.FileName,
                           Owner = x.Owner
                       };

            return file.ToList();
        }
        public List<FilteredDTO> getFilesByOwner(string owner)
        {
            var file = from x in getAllMetadata()
                       where x.Owner.Equals(owner, StringComparison.OrdinalIgnoreCase)
                       select new FilteredDTO
                       {
                           FileName = x.FileName,
                           Owner = x.Owner
                       };

            return file.ToList();
        }

        public List<String> transferOwner(string oldOwner, string newOwner)
        {
            var filesToTransfer = from file in getAllMetadata()
                                  where file.Owner.Equals(oldOwner, StringComparison.OrdinalIgnoreCase)
                                  select file;
            foreach (var file in filesToTransfer)
            {
                string metadataPath = Path.Combine("Resources", file.FileName + ".json");

                var metadata = new { Owner = newOwner, CreatedAt = file.CreationDate, LastModified = DateTime.Now };
                File.WriteAllText(metadataPath, JsonSerializer.Serialize(metadata));
            }

            var updatedFiles = from file in getAllMetadata()
                               where file.Owner.Equals(newOwner, StringComparison.OrdinalIgnoreCase)
                               select file.FileName;

            return updatedFiles.ToList();
        }


        private List<MetaDataDTO> getAllMetadata()
        {
            var metaFiles = Directory.GetFiles("Resources", "*.json");

            var files = from metadataFile in metaFiles
                        let metadata = JsonSerializer.Deserialize<Dictionary<string, object>>(File.ReadAllText(metadataFile))
                        where metadata != null && metadata.ContainsKey("Owner") && metadata.ContainsKey("CreatedAt") && metadata.ContainsKey("LastModified")
                        select new MetaDataDTO
                        {
                            FileName = Path.GetFileNameWithoutExtension(metadataFile) ?? throw new Exception("File has no name"),
                            Owner = metadata["Owner"].ToString() ?? throw new Exception("Empty Owner"),
                            CreationDate = DateTime.TryParse(metadata["CreatedAt"].ToString(), out var created) ? created: throw new Exception("Invalid or missing CreationDate"),
                            ModificationDate = DateTime.TryParse(metadata["LastModified"].ToString(), out var modified)? modified: throw new Exception("Invalid or missing ModificationDate")
                        };

            return files.ToList();
        }
    }
}
