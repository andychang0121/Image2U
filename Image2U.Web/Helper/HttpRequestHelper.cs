using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Image2U.Web.Helper
{
    public static class HttpRequestHelper
    {
        public static IEnumerable<HttpPostedFile> GetHttpFiles(this HttpRequest httpRequest)
        {
            HttpFileCollection files = httpRequest.Files;

            int fileCount = files.Count;

            List<HttpPostedFile> formFiles = new List<HttpPostedFile>();

            for (int i = 0; i < fileCount; i++)
            {
                HttpPostedFile item = files[i];

                formFiles.Add(item);
            }

            return formFiles;
        }
    }
}