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
using Image2U.Service.Models.Image;
using Image2U.Web.Enum;

namespace Image2U.Web.Controllers
{
    public partial class UploadController
    {
        private async Task<IEnumerable<ZipData>> GetZipAsync(
            Stream stream, string fileName, bool isPortait, Dictionary<string, ImageOutput> ecProfile)
        {
            List<ZipData> entryFiles = new List<ZipData>();

            foreach (KeyValuePair<string, ImageOutput> ec in ecProfile)
            {
                string ecName = ec.Key;
                ImageOutput ecSetting = ec.Value;

                byte[] bytes =
                    await GetZipAsync(stream, fileName, isPortait, ecSetting.Width, ecSetting.MaxHeight, ecName);

                string zipFileName = ZipHelper.GetZipFileName(fileName, ecSetting.Width, ecSetting.MaxHeight, ecName);

                ZipData zipData = new ZipData(bytes, zipFileName, string.Empty);

                entryFiles.Add(zipData);
            }

            return entryFiles;
        }

        private async Task<byte[]> GetZipAsync(Stream stream, string fileName, bool isPortait, int width, int height, string ecName)
        {
            ImageFile imageFile = new ImageFile(stream, fileName, isPortait);

            byte[] bytes = await Task.Run(() =>
                Resize(imageFile, width, height, ImageFormat.Jpeg));

            return bytes;
        }

        private static async Task<IEnumerable<ZipData>> GetZip(RequestData requestData, Dictionary<string, ImageOutput> ecProfile)
        {
            List<ZipData> entryFiles = new List<ZipData>();

            foreach (KeyValuePair<string, ImageOutput> ec in ecProfile)
            {
                string ecName = ec.Key;
                ImageOutput ecSetting = ec.Value;

                bool isPortait = requestData.IsPortsait();

                Stream stream = requestData.Base64.GetStream();

                ZipData zipData =
                    await GetZip(stream, requestData.FileName, isPortait, ecSetting.Width, ecSetting.MaxHeight, ecName);

                entryFiles.Add(zipData);
            }

            return entryFiles;
        }

        private static async Task<ZipData> GetZip(Stream stream, string fileName, bool isPortait, int width, int height, string ecName)
        {
            ImageFile imageFile = new ImageFile(stream, fileName, isPortait);

            byte[] bytes = await Task.Run(() =>
                imageFile.Resize(width, height, ImageFormat.Jpeg));

            string zipFileName =
                imageFile.GetZipFileName(ecName, width, height);

            ZipData rs = new ZipData(bytes, zipFileName, ecName);

            return rs;
        }

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