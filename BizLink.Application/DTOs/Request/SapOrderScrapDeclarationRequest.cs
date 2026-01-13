using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.DTOs.Request
{
    public class SapOrderScrapDeclarationRequest
    {
        public string FactoryCode
        {
            get; set;
        }

        public List<string>? WorkCenterCodes
        {
            get; set;
        }

        public DateTime? StartDate
        {
            get; set;
        }

        public DateTime? EndDate
        {
            get; set;
        }

        public List<string>? WorkOrderNos
        {
            get; set;
        }
    }
}
