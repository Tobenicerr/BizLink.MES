using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.MAUI
{
    public class MesApiSettings
    {
        public string BaseUrl
        {
            get; set;
        }
        public Dictionary<string, string> Endpoints
        {
            get; set;
        }
    }

}
