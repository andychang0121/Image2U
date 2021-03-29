using Image2U.Service.Handler;
using Image2U.Service.Models.Image;
using Image2U.Service.Repository;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Image2U.Web.Controllers
{
    public partial class UploadController : Controller
    {
        public const string _zipContentType = "application/x-zip-compressed";
        public const string _downloadExtName = "zip";
        public readonly IConvertHandler _IConvertHandler;

        public UploadController()
        {
            _IConvertHandler = new ConvertHandler();
        }

        private readonly Dictionary<string, ImageOutput> _ecDict =
            new Dictionary<string, ImageOutput>
            {
                {"PCHOME-尺寸-800-120",new ImageOutput {
                    Width = 800,
                    MinHeight = 0,
                    MaxHeight = 120,
                    DPI = 72
                }},
                {"PCHOME-尺寸-306-360",new ImageOutput {
                    Width = 360,
                    MinHeight = 0,
                    MaxHeight = 360,
                    DPI = 72
                }},
                {"PCHOME-尺寸-475-270",new ImageOutput {
                    Width = 475,
                    MinHeight = 0,
                    MaxHeight = 270,
                    DPI = 72
                }},
                {"PCHOME-尺寸-414-270",new ImageOutput {
                    Width = 414,
                    MinHeight = 0,
                    MaxHeight = 270,
                    DPI = 72
                }},
                {"PCHOME-尺寸-314-282",new ImageOutput {
                    Width = 314,
                    MinHeight = 0,
                    MaxHeight = 282,
                    DPI = 72
                }},
                {"Momo",new ImageOutput {
                    Width = 1000,
                    MinHeight = 0,
                    MaxHeight = 300,
                    DPI = 200
                }},
                {"Momo-館內輪播看板",new ImageOutput {
                    Width = 818,
                    MinHeight = 0,
                    MaxHeight = 370,
                    DPI = 200
                }},
                {"Momo-輪播看板",new ImageOutput {
                    Width = 960,
                    MinHeight = 0,
                    MaxHeight = 480,
                    DPI = 200
                }},
                {"Shopee-輪播看板",new ImageOutput {
                    Width = 2000,
                    MinHeight = 100,
                    MaxHeight = 2200
                }},
                {"Shopee-簡易看板",new ImageOutput {
                    Width = 600,
                    MinHeight = 0,
                    MaxHeight = 600
                }},
                {"Shopee-簡易圖片-圖片點擊區",new ImageOutput {
                    Width = 1200,
                    MinHeight = 100,
                    MaxHeight = 2200
                }}
            };

        public ActionResult Index() => View();


    }
}