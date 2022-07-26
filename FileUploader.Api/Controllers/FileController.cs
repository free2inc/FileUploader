using FileUploader.Service;
using Microsoft.AspNetCore.Mvc;
using FileUploader.Domain.ResourceParameters;
using AutoMapper;
using FileUploader.Domain.Models;

namespace FileUploader.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        IFileService _fileService;
        IConfiguration _configuration;
        IMapper _mapper;
        //IHttpContextAccessor _httpContextAccessor;


        string[]? _validExtensions;

        public FileController(IFileService fileService, 
            IConfiguration configuration,
            IMapper mapper)
            /*IHttpContextAccessor httpContextAccessor*/
        {
            _fileService = fileService;
            _configuration = configuration;
            _mapper = mapper;
            //_httpContextAccessor = httpContextAccessor;
            _validExtensions = _configuration["AllowedExtensions"]?.Split(", ");
        }

        // GET: api/<FileController>
        [HttpGet]
        public async Task<IActionResult> GetFilesAsync([FromQuery] RequestParams requestParams)
        {
            if (requestParams.FileExtension is null)
            {
                requestParams.FileExtension = _validExtensions.FirstOrDefault();
            }

            var files = await _fileService.GetFiles(requestParams);
            
            var mappedFiles = _mapper.Map<IList<FileEntity>, IList<FileDto>>(files.ToList());
            var count = _fileService.GetAllFilesCountByExtension(requestParams.FileExtension);

            return Ok(new { Files = mappedFiles, TotalFiles = count })  ;

        }

        [HttpGet(nameof(GetExtensions))]
        public IActionResult GetExtensions()
        {
            return Ok(_validExtensions);
        }

        [HttpGet(nameof(GetFileConfigs))]
        public IActionResult GetFileConfigs()
        {
            var config = new { ValidExtensions = _validExtensions, ValidFileSize = Convert.ToInt32(_configuration["UploadedFileMaxSize"]) };
            return Ok(config);
        }



        [HttpPost(nameof(UploadFileAsync))]
        public async Task<IActionResult> UploadFileAsync(IFormFile file)
        {
            if (file is null)
            {
                ModelState.AddModelError(nameof(file), "File is required");
                return ValidationProblem();
            }

            int fileMaxSize = Convert.ToInt32(_configuration["UploadedFileMaxSize"]);

            var extension = Path.GetExtension(file.FileName);

            if(!extension.IsValidExtension(_validExtensions))
            {
                ModelState.AddModelError(file.FileName, $"Cannot upload {extension} file format");
                return ValidationProblem();
            }

            if (!file.IsValidSize(fileMaxSize))
            {
                ModelState.AddModelError(file.FileName, $"File cannot be bigger than {fileMaxSize}");
                return ValidationProblem();
            }

            await _fileService.SaveFileAsync(file, file.FileName, extension);

            return Ok(new { file.Length });
        }


        
    }
}
