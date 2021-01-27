using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Image2U.Web.Models.Image;

namespace Image2U.Web.Controllers
{
    public partial class UploadController : Controller
    {
        Dictionary<string, ImageOutput> dict = new Dictionary<string, ImageOutput>
        {
            {"PCHOME",new ImageOutput {
                Width = 800,
                MaxHeight = 120,
                DPI = 100
            }},
            {"Momo",new ImageOutput {
                Width = 1000,
                MaxHeight = 300,
                DPI = 200
            }},
            {"Shopee",new ImageOutput {
                Width = 600,
                MaxHeight = 600
            }}
        };




    }
}