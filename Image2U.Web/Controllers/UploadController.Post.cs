using Image2U.Service.Models;
using Image2U.Web.Helper;
using Image2U.Web.Models.Image;
using System;
using System.Web;
using System.Web.Mvc;
using Image2U.Service.Models.Image;

namespace Image2U.Web.Controllers
{
    public partial class UploadController
    {
        public ActionResult Post()
        {
            HttpRequest request = System.Web.HttpContext.Current.Request;

            if (request.Files.Count <= 0) return Json(new ResponseResult
            {
                IsOk = false
            }, JsonRequestBehavior.AllowGet);

            ResponseData response = Sizing(request);

            string key = Guid.NewGuid().ToString();

            TempData[key] = response.Serialize();

            ResponseResult rs = new ResponseResult
            {
                IsOk = true,
                Data = key
            };
            return Json(rs, JsonRequestBehavior.AllowGet);
        }
    }
}