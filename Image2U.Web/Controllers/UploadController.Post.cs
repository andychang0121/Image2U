using Image2U.Web.Enum;
using Image2U.Web.Helper;
using Image2U.Web.Models;
using Image2U.Web.Models.Image;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

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