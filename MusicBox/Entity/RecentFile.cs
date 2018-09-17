using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicBox.Entity
{
    class RecentFile
    {
        public String FileName { get; set; }

        public String FileSize { get; set; }

        public int FileIndex { get; set; }

        public RecentFile(int index, string fileName, string fileSize)
        {
            FileIndex = index;
            FileName = fileName;
            FileSize = fileSize;
        }
    }
}
