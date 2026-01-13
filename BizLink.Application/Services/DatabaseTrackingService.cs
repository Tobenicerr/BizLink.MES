using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public class DatabaseTrackingService : ITrackingService
    {
        private readonly IActivityLogRepository _logRepository;
        private readonly ICurrentUserService _currentUserService; // <- 依赖接口

        // 通过构造函数注入 ICurrentUserService
        public DatabaseTrackingService(IActivityLogRepository logRepository, ICurrentUserService currentUserService)
        {
            _logRepository = logRepository;
            _currentUserService = currentUserService;
        }

        private void Log(string type, string content, string details = null)
        {
            var log = new ActivityLog
            {
                LogType = type,
                LogContent = content,
                Details = details,
                // 从注入的服务中获取用户信息
                UserId = _currentUserService.UserId,
                UserName = _currentUserService.UserName
            };
            // 异步执行，不阻塞主线程
            Task.Run(() => _logRepository.AddAsync(log));
        }

        public void TrackAction(string actionName, string details = null) => Log("Action", actionName, details);
        public void TrackPageView(string pageName) => Log("PageView", pageName);
        public void TrackError(Exception exception, string context = null)
        {
            string details = $"Message: {exception.Message}\nStackTrace: {exception.StackTrace}";
            Log("Error", context ?? "Unhandled Exception", details);
        }
    }

}
