using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Attributes
{
    /// <summary>
    /// 自定义特性，用于标记 C# 属性对应的 SAP 表字段名
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class SapFieldNameAttribute : Attribute
    {
        public string Name
        {
            get;
        }

        public SapFieldNameAttribute(string name)
        {
            this.Name = name;
        }
    }
}
