using Image2U.Web.Enum;
using Image2U.Web.Helper;
using Image2U.Web.Models;
using Image2U.Web.Models.Image;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Image2U.Web.Controllers
{
    public partial class UploadController
    {
        private readonly Dictionary<string, ImageOutput> dict =
            new Dictionary<string, ImageOutput>
        {
            {"PCHOME-尺寸1",new ImageOutput {
                Width = 800,
                MaxHeight = 120,
                DPI = 72
            }},
            {"PCHOME-尺寸2",new ImageOutput {
                Width = 360,
                MaxHeight = 360,
                DPI = 72
            }},
            {"PCHOME-尺寸3",new ImageOutput {
                Width = 475,
                MaxHeight = 270,
                DPI = 72
            }},
            {"PCHOME-尺寸4",new ImageOutput {
                Width = 414,
                MaxHeight = 270,
                DPI = 72
            }},
            {"PCHOME-尺寸5",new ImageOutput {
                Width = 314,
                MaxHeight = 282,
                DPI = 72
            }},
            {"Momo",new ImageOutput {
                Width = 1000,
                MaxHeight = 300,
                DPI = 200
            }},
            {"Momo-館內輪播看板",new ImageOutput {
                Width = 818,
                MaxHeight = 370,
                DPI = 200
            }},
            {"Momo-輪播看板",new ImageOutput {
                Width = 960,
                MaxHeight = 480,
                DPI = 200
            }},
            {"Shopee-輪播看板",new ImageOutput {
                Width = 2000,
                MinHeight = 100,
                MaxHeight = 2200
            }},
            {"Shopee-簡易看板",new ImageOutput {
                Width = 600,
                MaxHeight = 600
            }},
            {"Shopee-簡易圖片-圖片點擊區",new ImageOutput {
                Width = 1200,
                MinHeight = 100,
                MaxHeight = 2200
            }}
        };

        private static bool[] GetIsPortaits(string stringValue)
            => stringValue
                .GetStringArray()
                .GetStingArrayToBoolean()
                ?.ToArray();

        public ActionResult Post()
        {
            HttpRequest request = System.Web.HttpContext.Current.Request;

            if (request.Files.Count <= 0) return Json(new ResponseResult
            {
                IsOk = false
            },JsonRequestBehavior.AllowGet);

            ResponseData response = PostProcess(request);

            string key = Guid.NewGuid().ToString();

            TempData[key] = response.Serialize();

            ResponseResult rs = new ResponseResult
            {
                IsOk = true,
                Data = key
            };
            return Json(rs,JsonRequestBehavior.AllowGet);
        }

        public ResponseData PostProcess(HttpRequest request)
        {
            IEnumerable<HttpPostedFile> formFiles = request.GetHttpFiles();

            string isPortaits = request.Form
                .GetDictionaryValue()
                .GetDictionaryValue("isPortaits");

            RequestFormData req = new RequestFormData
            {
                IsPortaits = isPortaits,
                File = formFiles
            };

            ResponseData rs = PostProcess(req);

            return rs;
        }

        public ResponseData PostProcess(RequestFormData formData)
        {
            bool[] isPortaits = GetIsPortaits(formData.IsPortaits);

            IEnumerable<HttpPostedFile> files = formData.File;

            IEnumerable<ZipData> entryFiles = GetFileResult(files, isPortaits, dict);

            byte[] zipRs = ZipHelper.ZipData(entryFiles);

            ResponseData response = new ResponseData
            {
                Result = zipRs
            };

            return response;
        }

        private static IEnumerable<ZipData> GetFileResult(IEnumerable<HttpPostedFile> files,
            IReadOnlyList<bool> isPortaits, Dictionary<string, ImageOutput> ecProfile)
        {
            List<ZipData> entryFiles = new List<ZipData>();

            foreach (KeyValuePair<string, ImageOutput> ec in ecProfile)
            {
                string ecName = ec.Key;
                ImageOutput ecSetting = ec.Value;

                files.ToList().Select((e, i) => new { e, i })
                    .ForEach(f =>
                    {
                        bool isPortait = isPortaits?[f.i] ?? false;

                        HttpPostedFile file = f.e;

                        ImageResult rs = GetFileResult(file, isPortait, ecSetting.Width);

                        ZipData zipData = new ZipData
                        {
                            FileName = $"{ecName}\\{rs.FileName}",
                            Bytes = rs.Bytes,
                            FolderName = string.Empty
                        };

                        entryFiles.Add(zipData);
                    });
            }

            return entryFiles;
        }

        private static ImageResult GetFileResult(HttpPostedFile formFile, bool isPortait, int limitPx)
        {
            string ext = formFile.FileName.Split('.').LastOrDefault();

            string originalFileName = formFile.FileName.Split('.').FirstOrDefault();

            Stream fileStream = formFile.InputStream;

            fileStream.Position = 0;

            //using (MagickImage image = new MagickImage(fileStream))
            //{
            //    IExifProfile exif = image.GetExifProfile();

            //    double ratio = GetRatio(isPortait, limitPx, image.Width, image.Height);

            //    ImageDirection direction = isPortait ? ImageDirection.Portait : ImageDirection.LandScape;

            //    ImageResult rs = await image.Resize(ratio, direction);

            //    rs.FileName = GetFileName(originalFileName, rs.Width, rs.Height, ext);

            //    return rs;
            //}


            using (Bitmap image = new Bitmap(Image.FromStream(fileStream)))
            {
                double ratio = GetRatio(isPortait, limitPx, image.Width, image.Height);

                ImageDirection direction = isPortait ? ImageDirection.Portait : ImageDirection.LandScape;

                Bitmap newImage = image.Resize(ratio, direction);

                string fileName = GetFileName(originalFileName, newImage.Width, newImage.Height, ext);

                byte[] bytes = newImage.ImageToByteArray(image.RawFormat);

                var rs = new ImageResult
                {
                    FileName = fileName,
                    Bytes = bytes
                };

                return rs;
            }
        }

        private static double GetRatio(bool isPortait, int limitPx, int width, int height)
            => isPortait ? limitPx / (double)height : limitPx / (double)width;

        private static string GetFileName(string prefix, int w, int h, string extName)
            => $"{prefix}-{w}x{h}.{extName}";
    }
}