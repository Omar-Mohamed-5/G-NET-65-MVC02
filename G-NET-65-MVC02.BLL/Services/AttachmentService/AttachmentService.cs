using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Pipelines;

namespace G_NET_65_MVC02.BLL.Services.AttachmentService
{
    public class AttachmentService : IAttachmentService
    {
        private readonly long _MaxFileSize = 5 * 1024 * 1024;
        private readonly string[] _AllowedExtensions = { ".jpg", ".jpeg", ".png" };
        private readonly ILogger<AttachmentService> _logger;
        private readonly IWebHostEnvironment _env;

        public AttachmentService(IWebHostEnvironment env, ILogger<AttachmentService> logger)
        {
            _logger = logger;
            _env = env;
        }


        public async Task<string?> UploadFileAsync(Stream FileStream, string FileName, string FolderName, CancellationToken ct = default)
        {
            if (FileStream is null || !FileStream.CanRead) return null;
            if (FileStream.Length == 0) return null;
            if(FileStream.Length > _MaxFileSize)
            {
                _logger.LogWarning("Rejected Upload:{File Too Large Size} bytes", FileStream.Length);
                return null;
            }
            var Extension = Path.GetExtension(FileName);
            if(string.IsNullOrEmpty(Extension) || !_AllowedExtensions.Contains(Extension))
            {
                _logger.LogWarning("Rejected Upload:Extension{Ext} Not Allowed", Extension);
                return null;
            }

            var UploadFolder = Path.Combine(_env.ContentRootPath, FolderName);
            Directory.CreateDirectory(UploadFolder);
            var StoredFileName = $"{Guid.NewGuid()}{Extension}";
            var FilePath = Path.Combine(UploadFolder, StoredFileName);

            try
            {
                await using var FS = new FileStream(FilePath,FileMode.CreateNew,FileAccess.Write,FileShare.None);
                await FileStream.CopyToAsync(FS, ct);
                return StoredFileName;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed To Update File {FileName}", FileName);
                return null;
            }
        }


        public bool Delete(string FileName, string FolderName)
        {
            if(string.IsNullOrEmpty(FileName) || string.IsNullOrEmpty(FolderName)) return false;
            try
            {
                var FullPath = Path.Combine(_env.WebRootPath, FileName, FolderName);
                if (!File.Exists(FullPath)) return false;
                File.Delete(FullPath);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed To Delete File {FileName}", FileName);
                return false;
            }
        }


        public (Stream stream, string ContentType)? GetFile(string FileName, string FolderName)
        {
            if (string.IsNullOrEmpty(FileName) || string.IsNullOrEmpty(FolderName)) return null;
            var FullPath = Path.Combine(_env.ContentRootPath,FolderName,FileName);
            if(!File.Exists(FullPath)) return null;
            var ContentType = Path.GetExtension(FullPath).ToLowerInvariant() switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                _ => "application/octet-stream"
            };
            var stream = new FileStream(FullPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return (stream, ContentType);
        }
    }
}
