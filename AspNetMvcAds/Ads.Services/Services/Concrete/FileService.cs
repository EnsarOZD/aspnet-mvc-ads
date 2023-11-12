using Ads.Services.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;

namespace Ads.Services.Services.Concrete
{
    public class FileService : IFileService
    {
        private readonly string _uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

        public List<string> GetFiles()
        {
            return Directory.GetFiles(_uploadDirectory).ToList();
        }

        public async Task UploadFileAsync(IFormFile formFile)
        {
            if (formFile == null || formFile.Length == 0)
            {
                throw new ArgumentNullException(nameof(formFile), "File is empty or null");
            }

            var filePath = Path.Combine(_uploadDirectory, formFile.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            }
        }

        public async Task<FileResponse?> DownloadFileAsync(string fileName)
        {
            var filePath = Path.Combine(_uploadDirectory, fileName);

            if (!File.Exists(filePath))
            {
                return null;
            }

            var fileBytes = await File.ReadAllBytesAsync(filePath);

            return new FileResponse
            {
                FileName = fileName,
                ContentType = GetContentType(filePath),
                FileContent = fileBytes
            };
        }

        public async Task DeleteAsync(string fileName)
        {
            var filePath = Path.Combine(_uploadDirectory, fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
        {
            {".txt", "text/plain"},
            {".pdf", "application/pdf"},
            {".doc", "application/vnd.ms-word"},
            {".docx", "application/vnd.ms-word"},
            {".png", "image/png"},
            {".jpg", "image/jpeg"},
            {".jpeg", "image/jpeg"},
            {".gif", "image/gif"},
            {".csv", "text/csv"}
            // Diğer dosya türlerini ekleyebilirsiniz
        };
        }

    }
}
