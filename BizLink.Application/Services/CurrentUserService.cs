using BizLink.MES.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.WinForms.Common
{
    public class CurrentUserService : ICurrentUserService
    {
        public int UserId => AppSession.IsLoggedIn ? AppSession.CurrentUser.Id : 0;
        public string UserName => AppSession.IsLoggedIn ? AppSession.CurrentUser.UserName : "Anonymous";
        public bool IsLoggedIn => AppSession.IsLoggedIn;
    }
}
