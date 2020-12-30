using System.Collections.Generic;
using Stize.Domain.File;

namespace Stize.Domain.Model
{
    public class MultiUploadedFileModel : IMultiUploadedFile
    {
        public IEnumerable<IStreamFileInfo> Files { get; set; }

    }

    public class MultiUploadedFileModel<T> : MultiUploadedFileModel, IMultiUploadedFile<T>
    {
        public T Model { get; set; }
    }

}