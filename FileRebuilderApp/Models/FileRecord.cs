namespace FileRebuilderApp.Models
{
    public class FileRecord
    {
        public int Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string OriginalPath { get; set; } = string.Empty;
    }
}
