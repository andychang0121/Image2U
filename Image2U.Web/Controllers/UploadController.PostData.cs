using Image2U.Service.Models;
using Image2U.Service.Models.Image;
using Image2U.Web.Helper;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Image2U.Web.Controllers
{
    public partial class UploadController
    {
        [HttpPost]
        public async Task<ActionResult> PostDataAsync(RequestData requestData)
        {
            if (!requestData.ValidRequestData()) return Json(new ResponseResult
            {
                IsOk = false
            }, JsonRequestBehavior.AllowGet);

            ResponseData response = await SizingAsync(requestData);

            ActionResult rs =  await Task.Run(() => SetTempData(response));

            return rs;
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