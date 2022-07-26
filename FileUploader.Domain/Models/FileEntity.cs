namespace FileUploader.Domain.Models
{
    public class FileEntity : BaseEntity
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string Type { get; set; }
        public long FileSize { get; set; }

    }
}
