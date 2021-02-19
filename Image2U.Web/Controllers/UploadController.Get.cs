using System.Linq;
using System.Threading.Tasks;
using Image2U.Web.Helper;
using Image2U.Web.Models.Image;
using System.Web.Mvc;

namespace Image2U.Web.Controllers
{
    public partial class UploadController
    {
        public async Task<ActionResult> Get(string tempdataKey)
            => await Task.Run(() => GetTempData(Server.HtmlEncode(tempdataKey)));

        private ActionResult GetTempData(string encodeKey)
        {
            string tempRs = (string)TempData[encodeKey];

            ResponseData jsonRs = tempRs.Deserialize<ResponseData>();

            byte[] bytes = jsonRs.Result;

            string originalFileName = jsonRs.FileName?.Split('.').FirstOrDefault() ?? encodeKey;

            string fileName = $"{originalFileName}.{_downloadExtName}";

            string encodeFileName = Url.Encode(fileName);

            FileContentResult rs = File(bytes, _zipContentType, encodeFileName);

            return rs;
        }
    }
}