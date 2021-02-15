using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Image2U.Web.Models.Image
{
    public struct ZipData
    {
        private string _fileName { get; set; }

        private byte[] _bytes { get; set; }

        private string _folderName { get; set; }

        public ZipData(byte[] bytes, string fileName, string folderName)
        {
            _bytes = bytes;
            _fileName = fileName;
            _folderName = folderName;
        }

        public string FileName => _fileName;

        public byte[] Bytes => _bytes;

        public string FolderName => _folderName;

        public string ZipFileName => string.IsNullOrEmpty(FolderName)
            ? _fileName : $"{_folderName}\\{_fileName}";
    }
}