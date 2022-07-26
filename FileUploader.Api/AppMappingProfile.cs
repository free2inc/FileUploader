using AutoMapper;
using FileUploader.Domain.Models;

namespace FileUploader.Api
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<FileEntity, FileDto>();

        }

    }
}
