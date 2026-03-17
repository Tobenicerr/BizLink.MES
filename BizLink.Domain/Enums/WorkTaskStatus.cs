using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Enums
{
    /// <summary>
    /// 任务生命周期状态 (Status)
    /// 使用数字字符串以便于逻辑流转控制
    /// </summary>
    public static class WorkTaskStatus
    {
        /// <summary>
        /// 新建：刚刚生成，未下发给产线
        /// </summary>
        public const string New = "10";

        /// <summary>
        /// 已下发：已推送到车间/工控机，等待排程
        /// </summary>
        public const string Released = "20";

        /// <summary>
        /// 已排产：已分配具体的机台或人员
        /// </summary>
        public const string Scheduled = "30";

        /// <summary>
        /// 执行中：正在生产，已有报工记录
        /// </summary>
        public const string InProgress = "40";

        /// <summary>
        /// 已挂起：因缺料、设备故障等原因暂停
        /// </summary>
        public const string Suspended = "45";

        /// <summary>
        /// 已完成：生产数量达标，等待关闭
        /// </summary>
        public const string Completed = "50";

        /// <summary>
        /// 已关闭：财务/质量已核算，流程终结
        /// </summary>
        public const string Closed = "60";

        /// <summary>
        /// 已删除/已取消：逻辑删除
        /// </summary>
        public const string Cancelled = "99";
    }
}
