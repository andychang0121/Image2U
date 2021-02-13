using Image2U.Web.Enum;
using System.IO;
using System.Linq;
using System.Web;

namespace Image2U.Web.Models.Image
{
    public struct ImageFile
    {
        private bool _isPortait { get; set; }
        private string _ext { get; set; }
        private string _fileName { get; set; }
        private Stream _stream { get; set; }

        public ImageFile(HttpPostedFile formFile, bool isPortait)
        {
            _isPortait = isPortait;
            _ext = formFile.FileName.Split('.').LastOrDefault();
            _fileName = formFile.FileName.Split('.').FirstOrDefault();
            _stream = formFile.InputStream;
            _stream.Position = 0;
        }

        public ImageFile(Stream stream, string fileName, bool isPortait)
        {
            _isPortait = isPortait;
            _ext = fileName.Split('.').LastOrDefault();
            _fileName = fileName.Split('.').FirstOrDefault();
            _stream = stream;
            _stream.Position = 0;
        }

        public ImageDirection Direction => _isPortait ? ImageDirection.Portait : ImageDirection.LandScape;

        public string FileName => _fileName;

        public string Ext => _ext;

        public Stream Stream => _stream;
    }
}