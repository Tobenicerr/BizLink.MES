using BizLink.MES.Domain.Enums;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Entities
{
    [SugarTable("Mes_StationMaterialLoading")]

    public class StationMaterialLoading
    {

        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        // ==========================================
        // 1. 位置信息 (Where)
        // ==========================================
        public int FactoryId { get; set; }




        /// <summary>
        /// 挂在哪个工位/切线机上
        /// 注意：如果是集中供料，这里存储的是“供料塔/干燥桶”的虚拟工位ID
        /// </summary>
        [SugarColumn(IsNullable = false)]
        public int WorkStationId { get; set; }

        /// <summary>
        /// 工位编码快照
        /// </summary>
        [SugarColumn(Length = 50)]
        public string WorkStationCode { get; set; }

        public int LineSideInventoryId { get; set; }

        // ==========================================
        // 2. 物料信息 (What)
        // ==========================================
        /// <summary>
        /// 原材料料号 (如: 电缆、端子、塑料粒子)
        /// </summary>
        [SugarColumn(Length = 50)]
        public string MaterialCode { get; set; }


        public string MaterialDesc { get; set; }

        /// <summary>
        /// 供应商批次/内部批次条码 (追溯核心)
        /// </summary>
        [SugarColumn(Length = 50)]
        public string BatchCode { get; set; }

        [SugarColumn(Length = 50)]
        public string BarCode { get; set; }

        // ==========================================
        // 3. 数量管理 (How Much)
        // ==========================================
        /// <summary>
        /// 上料时的初始数量 (整盘量)
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// 当前剩余数量 (随生产实时扣减)
        /// </summary>
        public decimal LastQuantity { get; set; }

        /// <summary>
        /// 单位 (M, KG, PCS)
        /// </summary>
        [SugarColumn(Length = 10)]
        public string BaseUnit { get; set; }

        // ==========================================
        // 4. 状态控制 (State)
        // ==========================================
        /// <summary>
        /// 状态: 1=使用中(Active), 2=已耗尽(Depleted), 3=已卸载(Unloaded)
        /// </summary>
        public string Status { get; set; } = "1";

        /// <summary>
        /// 供料模式: 1=独立供料(Local), 2=集中供料(Central)
        /// 独立供料：库存属于 WorkStationId 指定的机台
        /// 集中供料：库存属于公共供料塔，WorkStationId 指向供料塔
        /// </summary>
        public SupplyMode SupplyMode { get; set; } = SupplyMode.Local;

        /// <summary>
        /// 乐观锁版本号
        /// 关键：用于处理多机台同时扣减同一个集中供料塔时的并发冲突
        /// </summary>
        [SugarColumn(IsNullable = true, IsEnableUpdateVersionValidation = true)]
        public long Version { get; set; }

        // ==========================================
        // 5. 绑定工单 (Optional Context)
        // ==========================================
        /// <summary>
        /// 绑定工单号
        /// 如果是专用料，可以绑定工单；如果是通用料(如大盘线)，设为 NULL
        /// </summary>
        [SugarColumn(IsNullable = true, Length = 50)]
        public string? BoundWorkOrderNo { get; set; }

        [SugarColumn(IsNullable = true, ColumnDataType = "NVARCHAR(200)")]
        public string? Remark { get; set; }

        // ==========================================
        // 6. 审计 (Audit)
        // ==========================================
        public DateTime LoadedTime { get; set; } = DateTime.Now;

        [SugarColumn(Length = 50)]
        public string LoadedBy { get; set; }

        [SugarColumn(IsNullable = true)]
        public DateTime? UnloadedTime { get; set; }
    }
}
