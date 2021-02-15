using Image2U.Web.Helper;
using Image2U.Web.Models;
using Image2U.Web.Models.Image;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Image2U.Web.Controllers
{
    public partial class UploadController
    {
        [HttpPost]
        public async Task<ActionResult> PostData(RequestData requestData)
        {
            if (!requestData.ValidRequestData()) return Json(new ResponseResult
            {
                IsOk = false
            }, JsonRequestBehavior.AllowGet);

            ResponseData response = await Sizing(requestData);

            string key = Guid.NewGuid().ToString();

            TempData[key] = response.Serialize();

            ResponseResult rs = new ResponseResult
            {
                IsOk = true,
                Data = key
            };
            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public async Task<ResponseData> Sizing(RequestData request)
        {
            ResponseData rs = await Sizing(request, _ecDict);

            return rs;
        }

        public async Task<ResponseData> Sizing(RequestData requestData, Dictionary<string, ImageOutput> dict)
        {
            Dictionary<string, ImageOutput> refDict = dict;

            if (requestData.IsCustomSize)
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

            IEnumerable<ZipData> entryFiles = await GetZip(requestData, refDict);

            byte[] zipRs = ZipHelper.ZipData(entryFiles);

            ResponseData response = new ResponseData
            {
                FileName = requestData.FileName,
                ContentType = requestData.Type,
                Result = zipRs
            };

            return response;
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

                ImageFile imageFile = new ImageFile(stream, requestData.FileName, isPortait);

                byte[] bytes = await Task.Run(() =>
                        imageFile.Resize(ecSetting.Width, ecSetting.MaxHeight, ImageFormat.Jpeg));

                string zipFileName =
                    imageFile.GetZipFileName(ecSetting.Width, ecSetting.MaxHeight);

                ZipData zipData = new ZipData(bytes, zipFileName, ecName);

                entryFiles.Add(zipData);
            }

            return entryFiles;
        }
    }
}