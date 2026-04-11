using System.Security.Cryptography;
using System.Text;

namespace FileRebuilderApp.Helpers
{
    public static class HashHelper
    {
        public static string ComputeHash(byte[] data)
        {
            using var sha256 = SHA256.Create();
            var hashBytes = sha256.ComputeHash(data);

            var sb = new StringBuilder();
            foreach (var b in hashBytes)
                sb.Append(b.ToString("x2"));

            return sb.ToString();
        }
    }
}