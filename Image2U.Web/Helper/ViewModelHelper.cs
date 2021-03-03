using Image2U.Web.Models;
using Image2U.Web.Models.Image;
using System;
using System.Linq;

namespace Image2U.Web.Helper
{
    public static class RequestDataExtension
    {
        public static bool IsPortsait(this RequestData requestData)
            => requestData.Height > requestData.Width;

        public static bool ValidRequestData(this RequestData requestData)
            => !string.IsNullOrEmpty(requestData.Base64)
               && !string.IsNullOrEmpty(requestData.FileName);



        public static ResponseData GetResponseData(this ResponseData responseData, string ext)
        {
            ResponseData rs = new ResponseData
            {
                Result = ((byte[])responseData.Result).GetBase64(),
                FileName = $"{responseData.FileName.Split('.').FirstOrDefault()}.{ext}",
                ContentType = responseData.ContentType
            };
            return rs;
        }

        public static string SetDownloadFileName(this string fileName, string ext)
        {
            fileName = string.IsNullOrEmpty(fileName) ? $"{Guid.NewGuid()}.xxx" : fileName;

            return $"{fileName.Split('.').FirstOrDefault()}.{ext}";
        }

    }
}