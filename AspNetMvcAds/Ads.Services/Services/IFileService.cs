using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Ads.Services.Services
{
    public interface IFileService
    {
        List<string> GetFiles();
        Task UploadFileAsync(IFormFile formFile);
        Task<FileResponse?> DownloadFileAsync(string fileName);
        Task DeleteAsync(string fileName);
    }
}
