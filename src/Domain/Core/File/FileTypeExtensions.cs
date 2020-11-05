using System.Collections.Generic;
using System.Linq;

namespace Stize.Domain.File
{
    public static class FileTypeExtensions
    {
        /// <summary>
        /// http://www.youneeditall.com/web-design-and-development/file-extensions.html
        /// </summary>
        private static readonly Dictionary<FileType, List<string>> FileTypeExtensionsDictionary = new Dictionary
            <FileType, List<string>>
        {
            {
                FileType.Audio,
                new List<string>
                {
                    ".m4a",
                    ".mid",
                    ".mp3",
                    ".mpa",
                    ".ra",
                    ".wav",
                    ".wma",
                }
            },
            {
                FileType.Image,
                new List<string>
                {
                    ".gif",
                    ".jpg",
                    ".jpeg",
                    ".png",
                }
            },
            {
                FileType.Video,
                new List<string>
                {
                    ".3g2",
                    ".3gp",
                    ".asf",
                    ".asx",
                    ".avi",
                    ".flv",
                    ".mov",
                    ".mp4",
                    ".mpg",
                    ".rm",
                    ".swf",
                    ".vob",
                    ".wmv"
                }
            },
            {
                FileType.TextDocument,
                new List<string>
                {
                    ".doc",
                    ".docx",
                    ".log",
                    ".msg",
                    ".pages",
                    ".rtf",
                    ".txt",
                    ".wpd",
                    ".wps",
                }
            },
            {
                FileType.CompressedFile,
                new List<string>
                {
                    ".7z",
                    ".arc",
                    ".arj",
                    ".as",
                    ".b64",
                    ".btoa",
                    ".bz",
                    ".cab",
                    ".cpt",
                    ".gz",
                    ".hqx",
                    ".iso",
                    ".lha",
                    ".lzh",
                    ".mim",
                    ".mme",
                    ".pak",
                    ".pf",
                    ".rar",
                    ".sea",
                    ".sit",
                    ".sitx",
                    ".tar",
                    ".gz",
                    ".tbz",
                    ".tbz2",
                    ".tgz",
                    ".uu",
                    ".uue",
                    ".z",
                    ".zip",
                    ".zoo"
                }
            }
        };

        public static List<string> ToExtensions(this FileType fileType)
        {
            var allowedExtensions = new List<string>();

            if (FileTypeExtensionsDictionary.ContainsKey(fileType))
            {
                allowedExtensions.AddRange(FileTypeExtensionsDictionary[fileType]);
            }

            return allowedExtensions;
        }

        public static FileType ToFileType(this string extension)
        {
            return FileTypeExtensionsDictionary.FirstOrDefault(e => e.Value.Contains(extension.ToLower())).Key;
        }
    }
}