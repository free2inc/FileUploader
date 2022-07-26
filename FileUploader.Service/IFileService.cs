using FileUploader.Domain.Models;
using FileUploader.Domain.ResourceParameters;
using Microsoft.AspNetCore.Http;

namespace FileUploader.Service
{
    public interface IFileService
    {
        public Task<IEnumerable<FileEntity>> GetFiles(RequestParams requestParams);
        public Task SaveFileAsync(IFormFile file, string fileName, string extension);

        public int GetAllFilesCountByExtension(string extension);
    }
}
