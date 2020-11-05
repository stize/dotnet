namespace Stize.Domain.File
{
    public interface IFileEntity
    {

        /// <summary>
        /// The original name when the user uploads a file
        /// </summary>
        public string OriginalName { get; set; }

        /// <summary>
        ///     Size of the file in bytes
        /// </summary>
        public long ContentLength { get; set; }

        /// <summary>
        /// Type of the file
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// The extension given by the Server
        /// </summary>
        public string Extension { get; set; }

        /// <summary>
        /// The file type classification
        /// </summary>
        public FileType Type { get; set; }
    }
}