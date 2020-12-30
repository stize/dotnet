using System.IO;
using Stize.Domain.File;

namespace Stize.Domain.Model
{
    public class StreamFileModel : IStreamFileInfo
    {
        public StreamFileModel(Stream fileStream, string fileName, string fileContentType)
        {
            this.FileStream = fileStream;
            this.OriginalName = fileName;
            this.Extension = Path.GetExtension(fileName);
            this.ContentLength = fileStream.Length;
            this.ContentType = fileContentType;
            this.Type = this.Extension.ToFileType();
        }


        public Stream FileStream { get; set;}

        public string OriginalName { get;  set;}

        public string Extension { get;  set;}

        public long ContentLength { get; set; }

        public string ContentType { get; set; }

        public FileType Type { get;  set;}
    }
}