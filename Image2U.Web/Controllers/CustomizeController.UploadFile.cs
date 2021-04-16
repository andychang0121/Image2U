using System.Web.Mvc;
using Image2U.Web.Models;

namespace Image2U.Web.Controllers
{
    public partial class CustomizeController
    {
        [HttpPost]
        public ActionResult UploadFile(ImportRequestData request)
        {
            return View();
        }
    }
}