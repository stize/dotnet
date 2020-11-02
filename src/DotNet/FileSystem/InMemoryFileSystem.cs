using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Stize.DotNet.FileSystem
{
    /// <summary>
    /// In memory storage. For development purposes only!
    /// </summary>
    public class InMemoryFileSystem : IFileSystem
    {
        /// <summary>
        /// In-memory dictionary storing the files
        /// </summary>
        private static readonly IDictionary<string, Stream> Files = new ConcurrentDictionary<string, Stream>();

        /// <summary>
        /// Saves a file to the in-memory storage
        /// </summary>
        /// <param name="path">Pseudo-path to file</param>
        /// <param name="stream">File content</param>
        /// <returns>Async task</returns>
        public virtual Task SaveAsync(string path, Stream stream)
        {
            if (Files.ContainsKey(path))
            {
                Files[path] = stream;
            }
            else
            {
                Files.Add(path, stream);
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// Checks if a file exists in the in-memory storage
        /// </summary>
        /// <param name="path">Pseudo-path to file</param>
        /// <returns>Does the file exists in the in-memory storage?</returns>
        public virtual Task<bool> ExistsAsync(string path)
        {
            return Task.FromResult(Files.ContainsKey(path));
        }

        /// <summary>
        /// Deletes a file from the in-memory storage
        /// </summary>
        /// <param name="path">Path to delete from in-memory storage</param>
        /// <returns>Has the file been deleted?</returns>
        public virtual Task<bool> DeleteAsync(string path)
        {
            if (Files.ContainsKey(path))
            {
                Files.Remove(path);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        /// <summary>
        /// Retrieves a file content from the in-memory storage
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <returns>File content from the in-memory storage</returns>
        public virtual Task<Stream> OpenAsync(string path)
        {
            if (Files.TryGetValue(path, out Stream stream))
            {
                Stream copy = new MemoryStream();
                stream.CopyTo(copy);
                copy.Seek(0, SeekOrigin.Begin);
                return Task.FromResult(copy);
            }

            return Task.FromResult((Stream)null);
        }
    }
}
