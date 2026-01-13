using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Mappings
{
    /// <summary>
    /// 一个标记接口，表示该类型可以从指定的源类型 T 映射而来。
    /// </summary>
    /// <typeparam name="T">源类型</typeparam>
    public interface IMapFrom<T>
    {
        /// <summary>
        /// 创建一个从源类型到目标类型的映射配置。
        /// 默认实现是直接创建一个标准的映射。
        /// 如果有特殊需求（如忽略某个字段），可以在实现类中重写此方法。
        /// </summary>
        /// <param name="profile">AutoMapper 的 Profile 实例</param>
        void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType());
    }
}
