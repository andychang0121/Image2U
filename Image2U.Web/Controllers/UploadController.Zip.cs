using System;
using Image2U.Web.Helper;
using Image2U.Web.Models;
using Image2U.Web.Models.Image;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using Image2U.Service.Helper;
using Image2U.Service.Models.Image;
using Image2U.Web.Enum;
using Image2U.Service.Models.Zip;

namespace Image2U.Web.Controllers
{
    public partial class UploadController
    {
        private static IEnumerable<ZipData> GetZip(IEnumerable<HttpPostedFile> files,
            IReadOnlyList<bool> isPortaits, Dictionary<string, ImageOutput> ecProfile)
        {
            List<ZipData> entryFiles = new List<ZipData>();

            foreach (KeyValuePair<string, ImageOutput> ec in ecProfile)
            {
                string ecName = ec.Key;
                ImageOutput ecSetting = ec.Value;

                int fileIdx = 0;

                foreach (HttpPostedFile file in files)
                {
                    bool isPortait = isPortaits?[fileIdx] ?? false;

                    ZipData zipData = GetZip(file, isPortait, ecSetting.Width, ecSetting.MaxHeight, ecName);

                    entryFiles.Add(zipData);

                    fileIdx++;
                }
            }

            return entryFiles;
        }

        private static ZipData GetZip(HttpPostedFile file,
            bool isPortait, int width, int height, string ecName)
        {

            ImageFile imageFile = new ImageFile(file.InputStream, file.FileName, isPortait);

            byte[] bytes = imageFile.Resize(width, height, ImageFormat.Jpeg);

            string zipFileName =
                imageFile.GetZipFileName(ecName, width, height);

            ZipData rs = new ZipData(bytes, zipFileName, ecName);

            return rs;
        }
    }
}