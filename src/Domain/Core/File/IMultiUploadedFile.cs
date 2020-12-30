using System.Collections.Generic;

namespace Stize.Domain.File
{
    public interface IMultiUploadedFile
    {
        public IEnumerable<IStreamFileInfo> Files { get; set; }
    }

    public interface IMultiUploadedFile<T> : IMultiUploadedFile
    {
        public T Model { get; set;}
    }
}