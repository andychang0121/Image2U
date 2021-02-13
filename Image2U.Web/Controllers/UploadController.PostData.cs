using Image2U.Web.Helper;
using Image2U.Web.Models;
using Image2U.Web.Models.Image;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Image2U.Web.Controllers
{
    public static class UploadExtension
    {
        public static bool ValidRequestData(this RequestData requestData)
            => !string.IsNullOrEmpty(requestData.Base64)
               && !string.IsNullOrEmpty(requestData.FileName);
    }


    public partial class UploadController
    {
        [HttpPost]
        public ActionResult PostData(RequestData requestData)
        {
            if (!requestData.ValidRequestData()) return Json(new ResponseResult
            {
                IsOk = false
            }, JsonRequestBehavior.AllowGet);

            ResponseData response = Sizing(requestData);

            string key = Guid.NewGuid().ToString();

            TempData[key] = response.Serialize();

            ResponseResult rs = new ResponseResult
            {
                IsOk = true,
                Data = key
            };
            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public ResponseData Sizing(RequestData request)
        {
            ResponseData rs = Sizing(request, _ecDict);

            return rs;
        }

        public ResponseData Sizing(RequestData requestData, Dictionary<string, ImageOutput> dict)
        {
            Dictionary<string, ImageOutput> refDict = dict;

            if (requestData.CustomHeight.HasValue && requestData.CustomWidth.HasValue)
            {
                refDict = new Dictionary<string, ImageOutput>
                {
                    {"自定義尺寸",new ImageOutput {
                        Width = requestData.CustomWidth.Value,
                        MaxHeight = requestData.CustomHeight.Value,
                        DPI = 72
                    }},
                };
            }

            IEnumerable<ZipData> entryFiles = GetZip(requestData, refDict);

            byte[] zipRs = ZipHelper.ZipData(entryFiles);

            ResponseData response = new ResponseData
            {
                FileName = requestData.FileName,
                ContentType = requestData.Type,
                Result = zipRs
            };

            return response;
        }

        private static IEnumerable<ZipData> GetZip(RequestData requestData, Dictionary<string, ImageOutput> ecProfile)
        {
            List<ZipData> entryFiles = new List<ZipData>();

            foreach (KeyValuePair<string, ImageOutput> ec in ecProfile)
            {
                string ecName = ec.Key;
                ImageOutput ecSetting = ec.Value;

                bool isPortait = requestData.IsPortsait();

                ImageFile imageFile = new ImageFile(requestData.Base64.GetStream(), requestData.FileName, isPortait);

                ImageResult rs = GetZip(imageFile, ecSetting.Width, ecSetting.MaxHeight);

                ZipData zipData = new ZipData
                {
                    FileName = $"{ecName}\\{rs.FileName}",
                    Bytes = rs.Bytes,
                    FolderName = string.Empty
                };

                entryFiles.Add(zipData);


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
    }
}