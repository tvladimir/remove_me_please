using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project1.Models.Settings
{
    public class ApplicationSettings : IApplicationSettings
    {
        public string AppDataPath { get; set; }
        public string FilesDirectory { get; set; }
        public string DataDirectory { get; set; }
        public string DataFile { get; set; }
        public int ImageSize { get; set; }
    }
}

