using System;
using System.Formats.Tar;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace Indexer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            DecompressGzipFile("enron/mikro.tar.gz", "mails.tar");
            if (Directory.Exists("maildir")) Directory.Delete("maildir", true);
            TarFile.ExtractToDirectory("mails.tar", ".", false);

            new Renamer().Crawl(new DirectoryInfo("maildir"));
            await new App().RunAsync();
        }

        static void DecompressGzipFile(string compressedFilePath, string decompressedFilePath)
        {
            using (FileStream compressedFileStream = File.OpenRead(compressedFilePath))
            {
                using (FileStream decompressedFileStream = File.Create(decompressedFilePath))
                {
                    using (GZipStream gzipStream = new GZipStream(compressedFileStream, CompressionMode.Decompress))
                    {
                        gzipStream.CopyTo(decompressedFileStream);
                    }
                }
            }
        }
    }
}