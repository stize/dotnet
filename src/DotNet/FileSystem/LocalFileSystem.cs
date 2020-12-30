using System.IO;
using System.Threading.Tasks;

namespace Stize.DotNet.FileSystem
{
    /// <summary>
    /// Default storage file system.
    /// Class not recommended to use in Container/Cloud scenarios
    /// </summary>
    public class LocalFileSystem : IFileSystem
    {

        /// <summary>
        /// Saves a file to the local file system
        /// </summary>
        /// <param name="path">Path to save the file</param>
        /// <param name="stream">File content to save</param>
        /// <returns></returns>
        public virtual Task SaveAsync(string path, Stream stream)
        {
            this.CreateDirectoryIfNotExists(path);

            using (var fileStream = File.Create(path))
            {
                if (stream.Length > 0)
                {
                    var bytesInStream = new byte[stream.Length];
                    stream.Read(bytesInStream, 0, bytesInStream.Length);
                    fileStream.Write(bytesInStream, 0, bytesInStream.Length);
                }
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// Check if a file exists in the local file system
        /// </summary>
        /// <param name="path">Local file to check</param>
        /// <returns>Does the file exist in the local file system?</returns>
        public virtual Task<bool> ExistsAsync(string path)
        {
            return Task.FromResult(File.Exists(path));
        }

        /// <summary>
        /// Checks if a file exists in the file system and deletes it
        /// </summary>
        /// <param name="path">Local file to delete</param>
        /// <returns>Has the file been deleted?</returns>
        public virtual Task<bool> DeleteAsync(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        /// <summary>
        /// Reads a file content from the local file system
        /// </summary>
        /// <param name="path">Path to file to read</param>
        /// <returns>Stream with file content</returns>
        public virtual Task<Stream> OpenAsync(string path)
        {
            return Task.FromResult((Stream)File.OpenRead(path));
        }

        /// <summary>
        /// Creates a directory if does not exist
        /// </summary>
        /// <param name="path">Path to create if does not exist</param>
        protected virtual void CreateDirectoryIfNotExists(string path)
        {
            var directory = Path.GetDirectoryName(path);

            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }
    }
}