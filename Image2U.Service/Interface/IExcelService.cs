using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Image2U.Service.Interface
{
    interface IExcelService
    {
        Task<byte[]> GetExcel(string base64);
    }
}
