using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Image2U.Service.Interface;

namespace Image2U.Service.Repository
{
    public class ExcelService : IExcelService
    {
        public Task<byte[]> GetExcel(string base64)
        {
            throw new NotImplementedException();
        }
    }
}
