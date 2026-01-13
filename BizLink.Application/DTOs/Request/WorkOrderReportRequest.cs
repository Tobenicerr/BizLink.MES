using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.DTOs.Request
{
    public class WorkOrderReportRequest
    {
        public string FactoryCode
        {
            get; set;
        }
        public int ProcessId
        {
            get; set;
        }
        public string? EmployeeId { get; set; } = "";
        public int? ConfirmId { get; set; } = null;
    }
}
