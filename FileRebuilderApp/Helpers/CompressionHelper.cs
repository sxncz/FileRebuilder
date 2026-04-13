using System.IO;
using System.IO.Compression;

namespace FileRebuilderApp.Helpers
{
    public static class CompressionHelper
    {
        public static byte[] Compress(byte[] data)
        {
            using var output = new MemoryStream();

            using (var gzip = new GZipStream(output, CompressionMode.Compress))
            {
                gzip.Write(data, 0, data.Length);
            }

            return output.ToArray();
        }

        public static byte[] Decompress(byte[] compressedData)
        {
            using var input = new MemoryStream(compressedData);
            using var gzip = new GZipStream(input, CompressionMode.Decompress);
            using var output = new MemoryStream();

            gzip.CopyTo(output);

            return output.ToArray();
        }
    }
}
