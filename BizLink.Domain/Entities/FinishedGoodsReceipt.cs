using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Entities
{
    [SugarTable("Wms_FinishedGoodsReceipt")]
    public class FinishedGoodsReceipt
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id 
        { 
            get; 
            set; 
        }

        [SugarColumn(IsNullable = true)]
        public int WorkOrderId 
        { 
            get; 
            set; 
        }

        [SugarColumn(IsNullable = true)]
        public int FactoryId
        {
            get;
            set;
        }

        [SugarColumn(IsNullable = true)]
        public string? FactoryCode
        {
            get; set;
        } 

        [SugarColumn(IsNullable = true)]
        public string? WorkOrderNo
        {
            get; set;
        } // 物料代码

        [SugarColumn(IsNullable = true)]
        public int WorkOrderProcessId
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]
        public string? MaterialCode 
        { 
            get; 
            set; 
        } // 物料代码

        [SugarColumn(IsNullable = true)]
        public string? MaterialDesc 
        { 
            get; 
            set; 
        } // 物料名称

        [SugarColumn(IsNullable = true)]
        public decimal? Quantity { get; set; } // 数量

        [SugarColumn(IsNullable = true)]
        public string? BaseUnit { get; set; } // 单位

        [SugarColumn(IsNullable = true)]
        public string? WorkCenterCode
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]
        public string? StorageLocation
        {
            get;
            set;
        } // 库存地点

        [SugarColumn(IsNullable = true)]
        public string? SapTransferNo
        {
            get;
            set;
        }

        [SugarColumn(IsNullable = true)]
        public string? SapStatus
        {
            get;
            set;
        } = "0";

        [SugarColumn(IsNullable = true)]
        public string? SapBatchNo
        {
            get;
            set;
        }

        [SugarColumn(IsNullable = true)]
        public string? SapMessageType
        {
            get;
            set;
        }

        [SugarColumn(IsNullable = true)]
        public string? SapMessage
        {
            get;
            set;
        }

        [SugarColumn(IsNullable = true)]
        public bool? IsPrinted
        {
            get;
            set;
        } = false;

        [SugarColumn(IsNullable = true)]
        public int? PrintCount { get; set; } = 0;

        [SugarColumn(IsNullable = true)]
        public DateTime? CreatedOn
        {
            get; set;
        } = DateTime.Now; // 创建时间

        [SugarColumn(IsNullable = true)]
        public string? CreatedBy
        {
            get; set;
        } // 创建人

        [SugarColumn(IsNullable = true)]
        public DateTime? UpdatedOn
        {
            get; set;
        } // 更新时间

        [SugarColumn(IsNullable = true)]
        public string? UpdatedBy
        {
            get; set;
        } // 更新人
    }
}
