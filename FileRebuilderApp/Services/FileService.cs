using System.IO;
using FileRebuilderApp.Data;
using FileRebuilderApp.Helpers;
using FileRebuilderApp.Models;

namespace FileRebuilderApp.Services
{
    public class FileService
    {
        private readonly DatabaseService _databaseService;

        public FileService()
        {
            _databaseService = new DatabaseService();
        }

        public BackupResult BackupFile(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"File not found: {filePath}");

            var fileName = Path.GetFileName(filePath);

            if (_databaseService.FileExists(fileName, filePath))
                throw new Exception("A file with the same name or path has already been backed up.");

            var rawContent = File.ReadAllBytes(filePath);

            var hash = HashHelper.ComputeHash(rawContent);

            var existingContentId = _databaseService.GetContentIdByHash(hash);

            int contentId;
            long compressedSize;
            bool reused = false;

            if (existingContentId.HasValue)
            {
                contentId = existingContentId.Value;

                var existingContent = _databaseService.GetFileContentById(contentId);
                compressedSize = existingContent.Length;

                reused = true;
            }
            else
            {
                var compressedContent = CompressionHelper.Compress(rawContent);
                compressedSize = compressedContent.Length;

                contentId = _databaseService.InsertFileContent(compressedContent, hash);
            }

            _databaseService.InsertFileMetadata(fileName, filePath, contentId);

            return new BackupResult
            {
                OriginalSize = rawContent.Length,
                CompressedSize = compressedSize,
                UsedExistingContent = reused
            };
        }
    }
}