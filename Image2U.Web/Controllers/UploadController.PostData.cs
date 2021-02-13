using Image2U.Web.Enum;
using Image2U.Web.Helper;
using Image2U.Web.Models;
using Image2U.Web.Models.Image;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Image2U.Web.Controllers
{
    public partial class UploadController
    {
        [HttpPost]
        public async Task<ActionResult> PostData(RequestData data)
        {
            ResponseData response = await Task.Run(() => Sizing(data));

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

            if (requestData.IsCustomeSpec)
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
                FileName = requestData.FileName.Split('.').FirstOrDefault(),
                ContentType = requestData.Type,
                Result = zipRs
            };

            return response;
        }

        private static IEnumerable<ZipData> GetZip(RequestData file,
            Dictionary<string, ImageOutput> ecProfile)
        {
            List<ZipData> entryFiles = new List<ZipData>();

            foreach (KeyValuePair<string, ImageOutput> ec in ecProfile)
            {
                string ecName = ec.Key;
                ImageOutput ecSetting = ec.Value;

                bool isPortait = file.IsPortait.ToLower() == "true";

                ImageFile imageFile = new ImageFile(file.Base64.GetStream(), file.FileName, isPortait);

                ImageResult rs = GetZip(imageFile, ecSetting.Width, ecSetting.MaxHeight);

                ZipData zipData = new ZipData
                {
                    FileName = $"{ecName}\\{rs.FileName}",
                    Bytes = rs.Bytes,
                    FolderName = string.Empty
                };

                entryFiles.Add(zipData);
            }

            return entryFiles;
        }

        private static IEnumerable<ZipData> GetZip(IEnumerable<RequestData> files,
            Dictionary<string, ImageOutput> ecProfile)
        {
            List<ZipData> entryFiles = new List<ZipData>();

            foreach (KeyValuePair<string, ImageOutput> ec in ecProfile)
            {
                string ecName = ec.Key;
                ImageOutput ecSetting = ec.Value;

                foreach (RequestData f in files)
                {
                    bool isPortait = f.IsPortait.ToLower() == "true";

                    ImageFile imageFile = new ImageFile(f.Base64.GetStream(), f.FileName, isPortait);

                    ImageResult rs = GetZip(imageFile, ecSetting.Width, ecSetting.MaxHeight);

                    ZipData zipData = new ZipData
                    {
                        FileName = $"{ecName}\\{rs.FileName}",
                        Bytes = rs.Bytes,
                        FolderName = string.Empty
                    };

                    entryFiles.Add(zipData);
                }
            }

            return entryFiles;
        }
    }
}