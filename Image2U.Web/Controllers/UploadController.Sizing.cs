using Image2U.Service.Helper;
using Image2U.Service.Models;
using Image2U.Service.Models.Image;
using Image2U.Service.Models.Zip;
using Image2U.Web.Helper;
using Image2U.Web.Models;
using Image2U.Web.Models.Image;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web;

namespace Image2U.Web.Controllers
{
    public partial class UploadController
    {
        public async Task<ResponseData> ConvertImageAsync(RequestData requestData)
        {
            Dictionary<string, ImageOutput> dict = _ecDict;

            if (requestData.IsCustomSize && requestData.CustomWidth.HasValue && requestData.CustomHeight.HasValue)
            {
                dict = new Dictionary<string, ImageOutput>
                {
                    {"自定義尺寸",new ImageOutput {
                        Width = requestData.CustomWidth.Value,
                        MaxHeight = requestData.CustomHeight.Value,
                        DPI = 72
                    }},
                };
            }

            Stream stream = requestData.Base64.GetStream();

            bool isPortait = requestData.Height > requestData.Width;

            ProcessData processData =
                new ProcessData(requestData.FileName, requestData.Size, requestData.Type, isPortait, requestData.Width, requestData.Height, dict);

            ResponseData rs = await SizingAsync(stream, processData);

            return rs;
        }

        public async Task<ResponseData> ConvertImageBase64Async(RequestData requestData)
        {
            Dictionary<string, ImageOutput> dict = _ecDict;

            if (requestData.IsCustomSize && requestData.CustomWidth.HasValue && requestData.CustomHeight.HasValue)
            {
                dict = new Dictionary<string, ImageOutput>
                {
                    {"自定義尺寸",new ImageOutput {
                        Width = requestData.CustomWidth.Value,
                        MaxHeight = requestData.CustomHeight.Value,
                        DPI = 72
                    }},
                };
            }

            Stream stream = requestData.Base64.GetStream();

            bool isPortait = requestData.Height > requestData.Width;

            ProcessData processData =
                new ProcessData(requestData.FileName, requestData.Size, requestData.Type, isPortait, requestData.Width, requestData.Height, dict);

            ResponseData rs = await SizingAsync(stream, processData);

            return rs;
        }

        private async Task<ResponseData> SizingAsync(Stream stream, ProcessData processData, bool isBase64)
        {
            byte[] zipRs = await _IConvertHandler.ConvertProcessAsync(stream, processData);

            ResponseData response = new ResponseData
            {
                FileName = processData.FileName,
                ContentType = processData.Type,
                Result = zipRs
            };

            return response;
        }


        private async Task<ResponseData> SizingAsync(Stream stream, ProcessData processData)
        {
            byte[] zipRs = await _IConvertHandler.ConvertProcessAsync(stream, processData);

            ResponseData response = new ResponseData
            {
                FileName = processData.FileName,
                ContentType = processData.Type,
                Result = zipRs
            };

            return response;
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
    }
}