using System.Drawing.Imaging;
using System.Web.Mvc;

namespace Image2U.Web.Controllers
{
    public partial class UploadController : Controller
    {
        public const string _zipContentType = "application/zip";

        public ActionResult Index()
        {
            return View();
        }
    }
}