using System.ComponentModel.DataAnnotations;

namespace FileUploader.Domain.Models
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        
    }
}
