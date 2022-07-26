using FileUploader.Domain.Models;
using Microsoft.AspNetCore.Http;
using FileUploader.Domain.ResourceParameters;
using FileUploader.Domain.Data;
using X.PagedList;
using Microsoft.AspNetCore.Hosting;

namespace FileUploader.Service
{
    public class FileService : IFileService
    {
        private ApplicationDbContext _applicationDbContext;
        IWebHostEnvironment _appEnvironment;

        public FileService(ApplicationDbContext applicationDbContext, 
            IWebHostEnvironment appEnvironment)
        {
            _applicationDbContext = applicationDbContext;
            _appEnvironment = appEnvironment;
        }

        public async Task SaveFileAsync(IFormFile file, string fileName, string extension)
        {
            var path = "\\Files\\" + fileName;
            using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            FileEntity fileEntity = new FileEntity { Name = file.FileName, Path = path, Type = extension, FileSize = file.Length, CreatedDate = DateTime.Now };
            AddFileToDb(fileEntity);
        }

        private void AddFileToDb(FileEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _applicationDbContext.FileEntities.Add(entity);
            _applicationDbContext.SaveChanges();
        }

        
        public async Task<IEnumerable<FileEntity>> GetFiles(RequestParams requestParams)
        {
            try
            {
                IQueryable<FileEntity> query = _applicationDbContext.FileEntities.AsQueryable();

                var obj = await query.Where(e => e.Type == requestParams.FileExtension)
                    .ToPagedListAsync(requestParams.PageNumber, requestParams.PageSize);
                
                return obj;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetAllFilesCountByExtension (string extension)
        {
            try
            {
                IQueryable<FileEntity> query = _applicationDbContext.FileEntities.AsQueryable();
                var count = query.Count(x => x.Type == extension);
                return count;
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
    

}