using Image2U.Service.Models.Image;
using System.Collections.Generic;

namespace Image2U.Service.Models.Process
{
    public struct ProcessData
    {
        private string _fileName { get; set; }

        private string _contentType { get; set; }

        private Dictionary<string, ImageOutput> _refDict { get; set; }

        public ProcessData(string fileName, string contentType, Dictionary<string, ImageOutput> refDict)
        {
            _fileName = fileName;
            _contentType = contentType;
            _refDict = refDict;
        }

        public string FileName => _fileName;

        public string ContentType => _contentType;

        public Dictionary<string, ImageOutput> RefDict => _refDict;
    }
}
