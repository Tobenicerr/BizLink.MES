using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.MAUI.Model
{
    public class MaterialTagInfo
    {
        public string MaterialCode
        {
            get; set;
        } // 物料号
        public string BatchCode
        {
            get; set;
        }    // 批次号
        public string BarCode
        {
            get; set;
        }      // 标签号
        public decimal Quantity
        {
            get; set;
        }      // 数量
        public string LocationCode
        {
            get; set;
        } // 原库位
    }
}
