using FileRebuilderApp.Data;
using FileRebuilderApp.Helpers;
using System.IO;

namespace FileRebuilderApp.Services
{
    public class RestoreService
    {
        private readonly DatabaseService _databaseService;

        public RestoreService()
        {
            _databaseService = new DatabaseService();
        }

        public void RestoreFile(int metadataId, string customPath = null)
        {
            var metadata = _databaseService.GetFileMetadata(metadataId);
            var compressedContent = _databaseService.GetFileContentById(metadata.ContentId);

            var content = CompressionHelper.Decompress(compressedContent);

            var path = customPath ?? metadata.OriginalPath;

            var directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            File.WriteAllBytes(path, content);
        }
    }
}
