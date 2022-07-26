using FileUploader.Service;
using System.ComponentModel.DataAnnotations;

namespace FileUploader.Api
{
    public class FileDto
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string Type { get; set; }
        public long FileSize { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
