namespace FileRebuilderApp.Models
{
    public class FileData
    {
        public string FileName { get; set; } = string.Empty;
        public string OriginalPath { get; set; } = string.Empty;
        public byte[] Content { get; set; } = [];
    }
}