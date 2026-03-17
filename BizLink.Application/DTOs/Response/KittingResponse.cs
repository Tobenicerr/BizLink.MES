using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.DTOs.Response
{
    public class KittingResponse
    {

        public int OrderId { get; set; }
        public string OrderNo { get; set; }
        public string OperationNo { get; set; }
        public string OperationStatus { get; set; }

        // 统计信息
        public int CableItemCount { get; set; }
        public int RawItemCount { get; set; }
        public int RawMtrBatchCount { get; set; } // 已扫描的物料批次数
        public int LabelCount { get; set; }

        // 明细列表
        public List<KittingItemDto> CableItems { get; set; } = new List<KittingItemDto>();
        public List<KittingItemDto> CenterStockItems { get; set; } = new List<KittingItemDto>();
        public List<KittingItemDto> AutoStockItems { get; set; } = new List<KittingItemDto>();
    }

    public class KittingItemDto
    {
        public int BomId { get; set; }
        public string ItemNo { get; set; }      // BOM Item
        public string MaterialCode { get; set; }
        public string MaterialDesc { get; set; }
        public decimal Quantity { get; set; }          // 需求数量
        public decimal CompletedQuantity { get; set; } // 已拣配/已完成数量
        public string ConsumeType { get; set; }        // 类型 (用于前端分组显示)
    }
}
