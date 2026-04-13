using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileRebuilderApp.Helpers
{
    public class CommonHelpers
    {
        public string FormatSize(long bytes)
        {
            if (bytes >= 1024 * 1024)
                return $"{bytes / (1024.0 * 1024):F2} MB";
            if (bytes >= 1024)
                return $"{bytes / 1024.0:F2} KB";

            return $"{bytes} B";
        }
    }
}
