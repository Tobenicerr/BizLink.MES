using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public interface ICurrentUserService
    {
        int UserId { get; }
        string UserName { get; }
        bool IsLoggedIn { get; }
    }
}
