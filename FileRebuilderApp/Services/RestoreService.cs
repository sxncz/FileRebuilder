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

        public void RestoreFile(int metadataId)
        {
            var metadata = _databaseService.GetFileMetadata(metadataId);
            var content = _databaseService.GetFileContent(metadata.ContentId);

            File.WriteAllBytes(metadata.OriginalPath, content);
        }
    }
}
