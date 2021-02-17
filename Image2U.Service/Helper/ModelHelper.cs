using Image2U.Service.Models;

namespace Image2U.Service.Helper
{
    public static class ModelHelper
    {
        public static bool IsPortsait(this RequestData requestData)
            => requestData.Height > requestData.Width;
    }
}
