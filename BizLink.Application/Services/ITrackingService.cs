using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public interface ITrackingService
    {
        /// <summary>
        /// 追踪用户操作行为
        /// </summary>
        /// <param name="actionName">操作名称，如 "创建用户", "删除订单"</param>
        /// <param name="details">可选的详细信息，可以是JSON字符串</param>
        void TrackAction(string actionName, string details = null);

        /// <summary>
        /// 追踪页面访问
        /// </summary>
        /// <param name="pageName">页面名称，如 "用户管理页"</param>
        void TrackPageView(string pageName);

        /// <summary>
        /// 追踪异常和错误
        /// </summary>
        /// <param name="exception">捕获到的异常对象</param>
        /// <param name="context">发生异常时的上下文信息</param>
        void TrackError(Exception exception, string context = null);
    }
}
