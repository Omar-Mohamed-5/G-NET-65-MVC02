using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_NET_65_MVC02.BLL.Services.AttachmentService
{
    public interface IAttachmentService
    {
        Task<string?> UploadFileAsync(Stream FileStream, string FileName, string FolderName, CancellationToken ct = default);
        bool Delete(string FileName, string FolderName);
        (Stream stream, string ContentType)? GetFile(string FileName, string FolderName);
    }
}
