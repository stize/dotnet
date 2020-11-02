using System.IO;
using System.Threading.Tasks;

namespace Stize.DotNet.FileSystem
{
    /// <summary>
    /// Interface providing abstraction over the different file storage providers
    /// </summary>
    public interface IFileSystem
    {
        /// <summary>
        /// Saves a file to the storage provider
        /// </summary>
        /// <param name="path">Path to save the file</param>
        /// <param name="stream">File content</param>
        /// <returns>Async task</returns>
        Task SaveAsync(string path, Stream stream);

        /// <summary>
        /// Checks if a file exists in the storage provider
        /// </summary>
        /// <param name="path">Path to check</param>
        /// <returns>Does the file exist in the storage provider?</returns>
        Task<bool> ExistsAsync(string path);

        /// <summary>
        /// Deletes a file from the storage provider
        /// </summary>
        /// <param name="path">Path to delete</param>
        /// <returns>Has the file been deleted from the storage provider?</returns>
        Task<bool> DeleteAsync(string path);

        /// <summary>
        /// Opens a file
        /// </summary>
        /// <param name="path">Path to file in the storage provider</param>
        /// <returns>File content</returns>
        Task<Stream> OpenAsync(string path);
    }

}
