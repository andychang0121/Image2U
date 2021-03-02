using Image2U.Web.Helper;
using Image2U.Web.Models;
using Image2U.Web.Models.Image;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Image2U.Web.Controllers
{
    public partial class UploadController
    {
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PostDataAsync(RequestData requestData)
        {
            if (!requestData.ValidRequestData()) return Json(new ResponseResult
            {
                IsOk = false
            }, JsonRequestBehavior.AllowGet);

            ResponseData response = await ConvertImageAsync(requestData);

            ResponseData rs = new ResponseData
            {
                Result = await response.Result.GetBase64(),
                FileName = response.FileName.SetDownloadFileName("zip"),
                ContentType = response.ContentType
            };

            return Json(rs);
        }

        public ActionResult SetTempData(ResponseData data)
        {
            string key = Guid.NewGuid().ToString();

            TempData[key] = data.Serialize();

            ResponseResult rs = new ResponseResult
            {
                IsOk = true,
                Data = key
            };
            return Json(rs, JsonRequestBehavior.AllowGet);
        }

    }
}