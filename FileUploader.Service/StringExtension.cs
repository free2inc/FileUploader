using Microsoft.AspNetCore.Http;

namespace FileUploader.Service
{
    public static class Extension
    {
        public static bool IsValidExtension(this string extension, string[]? validExtensions)
        {
            return validExtensions.Any(x => x == extension);
        }


        public static bool IsValidSize(this IFormFile file, int fileMaxSize)
        {
            return (file.Length <= fileMaxSize);
        }
    }
}
