using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Entities.Views;
using BizLink.MES.Domain.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Repositories
{
    public interface IParameterGroupRepository : IGenericRepository<ParameterGroup>
    {
        /// <summary>
        /// 根据分组键获取一个参数组及其所有子项
        /// </summary>
        /// <param name="groupKey">分组键</param>
        /// <returns>包含所有明细项的参数组</returns>
        Task<ParameterGroup> GetGroupWithItemsAsync(string groupKey);

        /// <summary>
        /// 获取所有参数组的树形结构（包含子项）
        /// </summary>
        /// <returns>参数组的树形列表</returns>
        Task<List<ParameterGroup>> GetParameterTreeAsync();
    }
}
