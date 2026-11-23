using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_NET_65_MVC02.BLL.Services.AttachmentService
{
    public class AttachmentService : IAttachmentService
    {
        private readonly long _maxFileSize = 5 * 1024 * 1024;
        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png" };
        private readonly ILogger<AttachmentService> _logger;
        private readonly IWebHostEnvironment _env;

        public AttachmentService(IWebHostEnvironment env, ILogger<AttachmentService> logger)
        {
            _logger = logger;
            _env = env;
        }
        public bool Delete(string Filename, string FolderName)
        {
            throw new NotImplementedException();
        }

        public (Stream stream, string ContentType)? GetFile(string Filename, string FolderName)
        {
            throw new NotImplementedException();
        }

        public Task<string?> UploadFileAsync(Stream FileStream, string Filename, string FolderName, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
