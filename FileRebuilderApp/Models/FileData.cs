namespace FileRebuilderApp.Models
{
    public class FileData
    {
        public string FileName { get; set; }
        public string OriginalPath { get; set; }
        public byte[] Content { get; set; }
    }
}