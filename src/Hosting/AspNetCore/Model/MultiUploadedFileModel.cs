using System.Collections.Generic;

namespace Stize.Hosting.AspNetCore.Model
{
    public class MultiUploadedFileModel
    {
        public IEnumerable<StreamFileModel> Files { get; set; }
       
    }

    public class MultiUploadedFileModel<T>
    {
        public IEnumerable<StreamFileModel> Files { get; set; }
        public T Model { get; set; }
    }
    
}