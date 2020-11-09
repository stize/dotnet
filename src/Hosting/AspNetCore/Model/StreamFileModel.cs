using System.IO;
using Stize.Domain.File;

namespace Stize.Hosting.AspNetCore.Model
{
    public class StreamFileModel
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


        public Stream FileStream { get; }

        public string OriginalName { get;  }

        public string Extension { get;  }

        public long ContentLength { get;  }

        public string ContentType { get;  }

        public FileType Type { get;  }
    }
}