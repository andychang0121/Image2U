using Image2U.Web.Helper;
using Image2U.Web.Models.Image;
using System.Web.Mvc;

namespace Image2U.Web.Controllers
{
    public partial class UploadController
    {
        public ActionResult Get(string tempdataKey)
        {
            string encodeKey = Server.HtmlEncode(tempdataKey);

            string tempRs = (string)TempData[encodeKey];

            ResponseData jsonRs = tempRs.Deserialize<ResponseData>();

            byte[] bytes = jsonRs.Result;

            string fileName = $"{tempdataKey}.zip";

            FileContentResult rs = File(bytes, _zipContentType, fileName);

            return rs;
        }
    }
}