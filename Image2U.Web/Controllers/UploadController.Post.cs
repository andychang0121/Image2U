using Image2U.Web.Enum;
using Image2U.Web.Helper;
using Image2U.Web.Models;
using Image2U.Web.Models.Image;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Image2U.Web.Controllers
{
    public partial class UploadController
    {
        private readonly Dictionary<string, ImageOutput> _ecDict =
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

        public async Task<ActionResult> Post()
        {
            HttpRequest request = System.Web.HttpContext.Current.Request;

            if (request.Files.Count <= 0) return Json(new ResponseResult
            {
                IsOk = false
            }, JsonRequestBehavior.AllowGet);

            ResponseData response = await Task.Run(() => Sizing(request));

            //ResponseData response = Sizing(request);

            string key = Guid.NewGuid().ToString();

            TempData[key] = response.Serialize();

            ResponseResult rs = new ResponseResult
            {
                IsOk = true,
                Data = key
            };

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public ResponseData Sizing(HttpRequest request)
        {
            IEnumerable<HttpPostedFile> formFiles = request.GetHttpFiles();

            Dictionary<string, string> form = request.Form
                .GetDictionaryValue();

            string isPortaits = form
                .GetDictionaryValue("isPortaits");

            RequestFormData req = new RequestFormData(form.GetDictionaryValue("customWidth"), form.GetDictionaryValue("customHeight"))
            {
                IsPortaits = isPortaits,
                FormData = form,
                File = formFiles
            };

            ResponseData rs = Sizing(req, _ecDict);

            return rs;
        }

        public ResponseData Sizing(RequestFormData formData, Dictionary<string, ImageOutput> dict)
        {
            bool[] isPortaits = StringHelper.GetStringToArray(formData.IsPortaits);

            IEnumerable<HttpPostedFile> files = (IEnumerable<HttpPostedFile>)formData.File;

            Dictionary<string, ImageOutput> refDict = dict;

            if (formData.IsCustomeSpec)
            {
                refDict = new Dictionary<string, ImageOutput>
                {
                    {"自定義尺寸",new ImageOutput {
                        Width = formData.CustomWidth,
                        MaxHeight = formData.CustomHeight,
                        DPI = 72
                    }},
                };
            }

            IEnumerable<ZipData> entryFiles = GetZip(files, isPortaits, refDict);

            byte[] zipRs = ZipHelper.ZipData(entryFiles);

            ResponseData response = new ResponseData
            {
                Result = zipRs
            };

            return response;
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

                foreach (HttpPostedFile f in files)
                {
                    bool isPortait = isPortaits?[fileIdx] ?? false;

                    ImageFile imageFile = new ImageFile(f, isPortait);

                    ImageResult rs = GetZip(imageFile, ecSetting.Width, ecSetting.MaxHeight);

                    ZipData zipData = new ZipData
                    {
                        FileName = $"{ecName}\\{rs.FileName}",
                        Bytes = rs.Bytes,
                        FolderName = string.Empty
                    };

                    entryFiles.Add(zipData);
                    fileIdx++;
                }


                //files.ToList().Select((e, i) => new { e, i })
                //    .ForEach(async f =>
                //    {
                //        bool isPortait = isPortaits?[f.i] ?? false;

                //        HttpPostedFile file = f.e;

                //        ImageResult rs = await GetFileResult(file, isPortait, ecSetting.Width);

                //        ZipData zipData = new ZipData
                //        {
                //            FileName = $"{ecName}\\{rs.FileName}",
                //            Bytes = rs.Bytes,
                //            FolderName = string.Empty
                //        };

                //        entryFiles.Add(zipData);
                //    });
            }

            return entryFiles;
        }

        private static ImageResult GetZip(ImageFile imageFile, int ecWidth, int ecHeigth)
        {
            //string ext = formFile.Ext;

            //string originalFileName = formFile.FileName.Split('.').FirstOrDefault();

            //Stream fileStream = formFile.Stream;

            //imageFile.Stream.Position = 0;

            //using (MagickImage image = new MagickImage(fileStream))
            //{
            //    IExifProfile exif = image.GetExifProfile();

            //    double ratio = GetRatio(isPortait, limitPx, image.Width, image.Height);

            //    ImageDirection direction = isPortait ? ImageDirection.Portait : ImageDirection.LandScape;

            //    ImageResult rs = await image.Resize(ratio, direction);

            //    rs.FileName = GetFileName(originalFileName, rs.Width, rs.Height, ext);

            //    return rs;
            //}


            using (Bitmap image = new Bitmap(Image.FromStream(imageFile.Stream)))
            {
                double ratio = GetRatio(imageFile.Direction, ecWidth, image.Width, image.Height);

                Bitmap newImage = image.Resize(ratio, imageFile.Direction, ecWidth, ecHeigth);

                string fileName = ImageHelper.GetFileName(imageFile.FileName, newImage.Width, newImage.Height, imageFile.Ext);

                ImageFormat imageFormat = ImageFormat.Jpeg;

                byte[] bytes = newImage.ImageToByteArray(ImageFormat.Jpeg);

                ImageResult rs = new ImageResult
                {
                    FileName = fileName,
                    Bytes = bytes
                };

                return rs;
            }
        }

        private static double GetRatio(ImageDirection direction, int limitWidth, int width, int height)
            => limitWidth == width ?
                1 :
                direction == ImageDirection.Portait ? limitWidth / (double)height : limitWidth / (double)width;
    }
}