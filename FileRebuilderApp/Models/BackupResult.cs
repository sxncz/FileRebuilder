using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileRebuilderApp.Models
{
    public class BackupResult
    {
        public long OriginalSize { get; set; }
        public long CompressedSize { get; set; }
        public bool UsedExistingContent { get; set; }
    }
}
