using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.DTOs
{
    public class TransferStockRequest
    {
        public string? LocationCode
        {
            get; set;
        } = null;

        public List<string> BarCodes
        {
            get; set;
        }
    }
}
