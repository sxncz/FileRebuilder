using FileRebuilderApp.Data;
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
            var content = _databaseService.GetFileContent(metadata.ContentId);

            var path = customPath ?? metadata.OriginalPath;

            var directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            File.WriteAllBytes(path, content);
        }
    }
}
