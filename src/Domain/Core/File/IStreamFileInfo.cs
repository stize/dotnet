using System.IO;

namespace Stize.Domain.File
{
    public interface IStreamFileInfo : IFileInfo
    {
        Stream FileStream { get; set;}
    }

}