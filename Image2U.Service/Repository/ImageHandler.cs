using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Image2U.Service.ImageHandler;
using Image2U.Service.Interface;
using Image2U.Service.Models;
using Image2U.Service.Models.Image;

namespace Image2U.Service.Repository
{
    public class ImageHandler : IImageHandler
    {
        private readonly Dictionary<string, ImageOutput> _ecDict =
            new Dictionary<string, ImageOutput>
            {
                {"PCHOME-尺寸1",new ImageOutput {
                    Width = 800,
                    MaxHeight = 120,
                    DPI = 72
                }},
                {"PCHOME-尺寸2",new ImageOutput {
                    Width = 360,
                    MaxHeight = 360,
                    DPI = 72
                }},
                {"PCHOME-尺寸3",new ImageOutput {
                    Width = 475,
                    MaxHeight = 270,
                    DPI = 72
                }},
                {"PCHOME-尺寸4",new ImageOutput {
                    Width = 414,
                    MaxHeight = 270,
                    DPI = 72
                }},
                {"PCHOME-尺寸5",new ImageOutput {
                    Width = 314,
                    MaxHeight = 282,
                    DPI = 72
                }},
                {"Momo",new ImageOutput {
                    Width = 1000,
                    MaxHeight = 300,
                    DPI = 200
                }},
                {"Momo-館內輪播看板",new ImageOutput {
                    Width = 818,
                    MaxHeight = 370,
                    DPI = 200
                }},
                {"Momo-輪播看板",new ImageOutput {
                    Width = 960,
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
                    MaxHeight = 600
                }},
                {"Shopee-簡易圖片-圖片點擊區",new ImageOutput {
                    Width = 1200,
                    MinHeight = 100,
                    MaxHeight = 2200
                }}
            };

        private readonly IImageService _imageService;
        private readonly IZipService _zipService;

        public ImageHandler()
        {
            _imageService = new ImageService();
            _zipService = new ZipService();
        }


        public async Task<ResponseData> ConvertAsync(RequestData requestData, Dictionary<string, ImageOutput> dict)
        {
            Dictionary<string, ImageOutput> rs = await _imageService.SizingAsync(requestData, _ecDict);

            return null;
        }
    }
}
