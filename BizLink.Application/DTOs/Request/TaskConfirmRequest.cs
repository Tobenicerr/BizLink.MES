using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.DTOs.Request
{
    public class TaskConfirmRequest
    {
        public int TaskId { get; set; }

        public string UserCode { get; set; }

        public decimal CompletedQty { get; set; }

        public bool ResultFlag { get; set; } = true;
    }
}
