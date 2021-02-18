using System.Linq;
using Image2U.Web.Helper;
using Image2U.Web.Models.Image;
using System.Web.Mvc;

namespace Image2U.Web.Controllers
{
    public partial class UploadController
    {
        public ActionResult Get(string tempdataKey)
        => GetTempData(Server.HtmlEncode(tempdataKey));

        private ActionResult GetTempData(string encodeKey)
        {
            string tempRs = (string)TempData[encodeKey];

            ResponseData jsonRs = tempRs.Deserialize<ResponseData>();

            byte[] bytes = jsonRs.Result;

            string originalFileName = jsonRs.FileName?.Split('.').FirstOrDefault() ?? encodeKey;

            string fileName = $"{originalFileName}.zip";

            FileContentResult rs = File(bytes, _zipContentType, fileName);

            return rs;
        }
    }
}