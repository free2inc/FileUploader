using Microsoft.EntityFrameworkCore;
using FileUploader.Domain.Models;

namespace FileUploader.Domain.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<FileEntity> FileEntities { get; set; }
    }
}
