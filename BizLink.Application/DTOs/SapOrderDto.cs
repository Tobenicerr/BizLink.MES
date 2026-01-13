using BizLink.MES.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.DTOs
{
    public class SapOrderDto
    {
        public List<SapOrderOperation>? sapOrderOperations
        {
            get; set;
        }
        public List<SapOrderBom>? sapOrderBoms
        {
            get; set;
        }
    }

    public class SapOrderRequest
    {
        public string FactoryCode 
        { 
            get; set; 
        }
        public DateTime? DispatchDate 
        { 
            get; set; 
        }

        public List<string>? WorkCenterCode 
        {
            get; set;
        }   
        public List<string>? OrderNos 
        { 
            get; set;
        }
    }
}
