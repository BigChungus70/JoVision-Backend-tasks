namespace jovision.Models.Task48
{
    public class ImgDTO
    {
        public ImgDTO()
        {
        }

        public ImgDTO(byte[]? imageData, string? contentType, string? message)
        {
            ImageData = imageData;
            ContentType = contentType;
            Message = message;
        }

        public byte[]? ImageData { get; set; }
        public string? ContentType { get; set; }
        public string ? Message { get; set; }
    }
}
