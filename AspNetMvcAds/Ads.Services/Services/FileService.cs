using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;

namespace Ads.Services.Services
{
    public class FileService : IFileService
    {
        private readonly string _rootPath;

        public FileService(IConfiguration configuration)
        {
            Configuration = configuration;
            _rootPath = Directory.GetCurrentDirectory(); // wwwroot içindeki dosyaları almak için mevcut çalışma dizini kullanıyoruz.
        }

        private IConfiguration Configuration { get; }

        public async Task DeleteAsync(string fileName)
        {
            await Task.Run(() =>
            {
                var uploadFolder = GetUploadFolder();
                var fullFilePath = Path.Combine(uploadFolder, fileName);
                if (File.Exists(fullFilePath))
                {
                    File.Delete(fullFilePath);
                }
            });
        }

        public async Task<FileResponse?> DownloadFileAsync(string fileName)
        {
            var uploadFolder = GetUploadFolder();
            var fullFilePath = Path.Combine(uploadFolder, fileName);
            if (!File.Exists(fullFilePath))
            {
                return null;
            }
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(fullFilePath, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            return new FileResponse
            {
                FileName = fileName,
                ContentType = contentType,
                FileContent = await File.ReadAllBytesAsync(fullFilePath)
            };
        }

        public List<string> GetFiles()
        {
            var uploadFolder = GetUploadFolder();
            var files = Directory.GetFiles(uploadFolder);
            return files.Select(f => Path.GetFileName(f)).ToList();
        }

        public async Task UploadFileAsync(IFormFile formFile)
        {
            var uploadFolder = GetUploadFolder();
            var fullFilePath = Path.Combine(uploadFolder, formFile.FileName);
            using var fileStream = new FileStream(fullFilePath, FileMode.Create);
            await formFile.CopyToAsync(fileStream);
        }

        private string GetUploadFolder()
        {
            var localUploadFolder = Configuration.GetValue<string>("FileUploadLocation");
            if (string.IsNullOrWhiteSpace(localUploadFolder))
            {
                throw new InvalidOperationException("FileUploadLocation is not configured.");
            }
            var fullPath = Path.Combine(_rootPath, "wwwroot", localUploadFolder);
            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }
            return fullPath;
        }
    }
}
