using BizLink.MES.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public interface IWorkOrderTaskService
    {
        Task<WorkOrderTaskDto> GetByIdAsync(int id);

        Task<List<WorkOrderTaskDto>> GetByIdAsync(List<int> ids);

        Task<WorkOrderTaskDto> GetByProcessIdAsync(int id,string cableItem);

        Task<List<WorkOrderTaskDto>> GetByProcessIdsAsync(List<int> processid);

        Task<List<WorkOrderTaskDto>> GetByOrderNoAsync(string orderno);

        Task<List<WorkOrderTaskDto>> GetByTaskNoAsync(string taskno);

        Task<List<WorkOrderTaskDto>> GetListByOrderIdsAsync(List<int> orderid);



        /// <summary>
        /// 创建新用户
        /// </summary>
        Task<WorkOrderTaskDto> CreateAsync(WorkOrderTaskCreateDto input);

        Task<bool> BatchCreateAsync(List<WorkOrderTaskCreateDto> input);

        /// <summary>
        /// 更新用户信息
        /// </summary>
        Task<bool> UpdateAsync(WorkOrderTaskUpdateDto input);

        /// <summary>
        /// 删除用户 (逻辑删除)
        /// </summary>
        Task<bool> DeleteAsync(int id);

        Task SuspendTask(int taskid);
    }
}
