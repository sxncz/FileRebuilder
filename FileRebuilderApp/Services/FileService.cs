using System.IO;
using FileRebuilderApp.Data;

namespace FileRebuilderApp.Services
{
    public class FileService
    {
        private readonly DatabaseService _databaseService;

        public FileService()
        {
            _databaseService = new DatabaseService();
        }

        public void BackupFile(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"File not found: {filePath}");

            var fileName = Path.GetFileName(filePath);

            if (_databaseService.FileExists(fileName, filePath))
                throw new Exception("A file with the same name or path has already been backed up.");

            var content = File.ReadAllBytes(filePath);

            int contentId = _databaseService.InsertFileContent(content);

            _databaseService.InsertFileMetadata(fileName, filePath, contentId);
        }
    }
}