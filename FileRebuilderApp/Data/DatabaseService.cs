using FileRebuilderApp.Models;
using Microsoft.Data.SqlClient;
using System.Configuration;


namespace FileRebuilderApp.Data
{
    public class DatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService()
        {
            _connectionString = ConfigurationManager
                .ConnectionStrings["DefaultConnection"]
                .ConnectionString;
        }

        // Insert file content and return its Id
        public int InsertFileContent(byte[] content)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            var query = "INSERT INTO FileContent (Content) OUTPUT INSERTED.Id VALUES (@Content)";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Content", content);

            return (int)command.ExecuteScalar();
        }

        // Insert metadata
        public void InsertFileMetadata(string fileName, string originalPath, int contentId)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            var query = @"INSERT INTO FileMetadata (FileName, OriginalPath, ContentId)
                          VALUES (@FileName, @OriginalPath, @ContentId)";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@FileName", fileName);
            command.Parameters.AddWithValue("@OriginalPath", originalPath);
            command.Parameters.AddWithValue("@ContentId", contentId);

            command.ExecuteNonQuery();
        }

        //Fetch metadata by Id
        public (string FileName, string OriginalPath, int ContentId) GetFileMetadata(int metadataId)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            var query = "SELECT FileName, OriginalPath, ContentId FROM FileMetadata WHERE Id = @Id";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Id", metadataId);

            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                return (
                    reader.GetString(0),
                    reader.GetString(1),
                    reader.GetInt32(2)
                );
            }

            throw new Exception("File metadata not found");
        }

        //Fetch content by ContentId
        public byte[] GetFileContent(int contentId)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            var query = "SELECT Content FROM FileContent WHERE Id = @Id";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Id", contentId);

            var result = command.ExecuteScalar();

            if (result != null)
                return (byte[])result;

            throw new Exception("File content not found");
        }

        //Fetch all files
        public List<FileRecord> GetAllFiles()
        {
            var files = new List<FileRecord>();

            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            var query = "SELECT Id, FileName, OriginalPath FROM FileMetadata";

            using var command = new SqlCommand(query, connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                files.Add(new FileRecord
                {
                    Id = reader.GetInt32(0),
                    FileName = reader.GetString(1),
                    OriginalPath = reader.GetString(2)
                });
            }

            return files;
        }

        public bool FileExists(string fileName, string originalPath)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            var query = "SELECT COUNT(1) FROM FileMetadata WHERE OriginalPath = @OriginalPath OR FileName = @FileName";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@FileName", fileName);
            command.Parameters.AddWithValue("@OriginalPath", originalPath);

            return (int)command.ExecuteScalar() > 0;
        }

        public void DeleteFile(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            var query = "DELETE FROM FileMetadata WHERE Id = @Id";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Id", id);

            command.ExecuteNonQuery();
        }
    }
}