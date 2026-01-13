using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.DTOs.Request
{
    public class MaterialSyncRequest
    {
        public string FactoryCode
        {
            get; set;
        }

        public List<string>? MaterialCodes
        {
            get; set;
        }

        public DateTime? StartTime
        {
            get; set;
        }

        public DateTime? EndTime
        {
            get; set;
        }
    }
}
