using Image2U.Web.Models;
using Image2U.Web.Models.Image;
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

            ActionResult rs = SetTempData(response);

            return rs;
        }
    }
}