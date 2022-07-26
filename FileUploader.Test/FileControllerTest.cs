namespace FileUploader.Test
{
    public class FileControllerTest
    {
        FileController _fileController;

        Mock<IFileService> _fileServiceMock;
        Mock<IConfiguration> _configurationMock;
        Mock<IMapper> _mapperMock;

        string[]? _validExtension;

        public FileControllerTest()
        {
            _fileServiceMock = new Mock<IFileService>();
            _configurationMock = new Mock<IConfiguration>();
            _configurationMock.Setup(c => c["AllowedExtensions"])
                .Returns(GetValidExtensions());
            _mapperMock = new Mock<IMapper>();
            _validExtension = new string[] { "png" };
            _fileController = new FileController(_fileServiceMock.Object,
                _configurationMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async void GetFilesAsync_Return_File_Count()
        {
            // Arrange
            _fileServiceMock.Setup(s => s.GetFiles(new RequestParams()))
                .ReturnsAsync(GetFileEntities());
            _mapperMock.Setup(m => m.Map<IList<FileEntity>, IList<FileDto>>(new List<FileEntity>()))
                .Returns(GetFileDto());

            Mock<IFormFile> formFile = new Mock<IFormFile>();

            // Act
            var result = await _fileController.GetFilesAsync(new RequestParams());

            // Assert
            var viewResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<FileDto>>(viewResult.Value);
            Assert.Equal(GetFileEntities().Count(), model.Count());
        }


        [Fact]
        public async void OnPostUploadAsync_Return_ValidationExtension_Error()
        {
            // Arrange
            _fileServiceMock.Setup(s => s.GetFiles(new RequestParams()))
                .ReturnsAsync(GetFileEntities());
            _configurationMock.Setup(c => c["UploadedFileMaxSize"])
                .Returns("1000");

            _mapperMock.Setup(m => m.Map<IList<FileEntity>, IList<FileDto>>(new List<FileEntity>()))
                .Returns(GetFileDto());

            var content = "Hello World from a Fake File";
            var fileName = "test.pdf";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;

            IFormFile file = new FormFile(stream, 0, stream.Length, "id", fileName);

            // Act
            var result2 = _fileController.UploadFileAsync(file);
            var result = await _fileController.GetFilesAsync(new RequestParams());

            // Assert
            var ValidationProblemResult = Assert.IsAssignableFrom<ObjectResult>(result2.Result);
            Assert.Equal("Microsoft.AspNetCore.Mvc.ValidationProblemDetails", ValidationProblemResult.Value.ToString());

        }


        private string GetValidExtensions()
        {
            return "png" ;
        }

        IEnumerable<FileEntity> GetFileEntities()
        {
            var files = new List<FileEntity>()
            {
                new FileEntity{ Id = 1, Name = "Entity1", Path = "Path1", Type = ".pdf", CreatedDate = new DateTime()},
                new FileEntity{ Id = 1, Name = "Entity2", Path = "Path2", Type = ".pdf", CreatedDate = new DateTime()},
                new FileEntity{ Id = 1, Name = "Entity3", Path = "Path3", Type = ".pdf", CreatedDate = new DateTime()},
                new FileEntity{ Id = 1, Name = "Entity4", Path = "Path4", Type = ".pdf", CreatedDate = new DateTime()},
                new FileEntity{ Id = 1, Name = "Entity5", Path = "Path5", Type = ".pdf", CreatedDate = new DateTime()}
            };

            return files;
        }

        List<FileDto> GetFileDto()
        {
            var files = new List<FileDto>()
            {
                new FileDto{ Name = "Entity1", Path = "Path1", Type = ".pdf", CreatedDate = new DateTime()},
                new FileDto{ Name = "Entity2", Path = "Path2", Type = ".pdf", CreatedDate = new DateTime()},
                new FileDto{ Name = "Entity3", Path = "Path3", Type = ".pdf", CreatedDate = new DateTime()},
                new FileDto{ Name = "Entity4", Path = "Path4", Type = ".pdf", CreatedDate = new DateTime()},
                new FileDto{ Name = "Entity5", Path = "Path5", Type = ".pdf", CreatedDate = new DateTime()}
            };

            return files;
        }

    }
}