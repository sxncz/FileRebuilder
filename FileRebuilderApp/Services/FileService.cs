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

            // 1. Read file
            var content = File.ReadAllBytes(filePath);

            // 2. Store content
            int contentId = _databaseService.InsertFileContent(content);

            // 3. Store metadata
            var fileName = Path.GetFileName(filePath);

            _databaseService.InsertFileMetadata(fileName, filePath, contentId);

            // 4. Delete original file
            File.Delete(filePath);
        }
    }
}