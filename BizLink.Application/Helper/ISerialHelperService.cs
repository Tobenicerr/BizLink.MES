using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Helper
{
    public interface ISerialHelperService
    {
        string GenerateNext(string name);
    }
}
