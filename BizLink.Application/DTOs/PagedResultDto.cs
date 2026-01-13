using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.DTOs
{
    public class PagedResultDto<T>
    {
        public IEnumerable<T> Items
        {
            get; set;
        }
        public int TotalCount
        {
            get; set;
        }
    }
}
