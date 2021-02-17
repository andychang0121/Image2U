using Image2U.Service.Helper;
using Image2U.Service.Models;
using Image2U.Service.Models.Image;
using Image2U.Web.Helper;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;

namespace Image2U.Web.Controllers
{
    public partial class UploadController
    {
        public async Task<ResponseData> SizingAsync(RequestData request)
        {
            ResponseData rs = await SizingAsync(request, _ecDict);

            return rs;
        }

        public async Task<ResponseData> SizingAsync(RequestData requestData, Dictionary<string, ImageOutput> dict)
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

        public ResponseData Sizing(HttpRequest request)
        {
            IEnumerable<HttpPostedFile> formFiles = request.GetHttpFiles();

            Dictionary<string, string> form = request.Form
                .GetDictionaryValue();

            string isPortaits = form
                .GetDictionaryValue("isPortaits");

            string width = form
                .GetDictionaryValue("isPortaits");

            string height = form
                .GetDictionaryValue("height");

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