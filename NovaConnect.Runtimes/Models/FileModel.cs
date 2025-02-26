using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaConnect.Runtimes.Models
{
    public class FileInfoType
    {
        public string FileName { get; set; }
        public string CreatTime { get; set; }
        public string LastWrite { get; set; }
        public string FileSize { get; set; }
    }

    public class DriveInfoType
    {
        public string Name { get; set; }
        public string Label { get; set; }
        public string Total { get; set; }
        public string Used { get; set; }
    }
}