using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Enums
{
    /// <summary>
    /// 工步/物料任务类型定义 (TaskCategory / TaskSubType)
    /// </summary>
    public static class TaskCategories
    {
        // --- 1. 预处理与准备 (Pre-process) ---
        public const string MaterialCheck = "MAT_CHECK";     // 材料准备/核对 (Checking)
        public const string Picking = "PICKING";             // 拣配 (Picking)
        public const string Kitting = "KITTING";             // 齐套确认 (Kitting)
        public const string Cutting = "CUTTING";             // 裁剪/切线 (Cutting)

        // --- 2. 线缆加工 (Cable Processing) ---
        public const string CableCut = "CABLE_CUT";          // 电缆切断 (Material Level)
        public const string Stripping = "STRIPPING";         // 剥线
        public const string EndWorking = "END_WORKING";      // 剥线后处理 (如捻线、搪锡前处理)
        public const string Printing = "PRINTING";           // 喷码/印字

        // --- 3. 连接与端子 (Connection & Termination) ---
        public const string MachCrimping = "MACH_CRIMP";     // 机器压接
        public const string HandCrimping = "HAND_CRIMP";     // 手工压接
        public const string Soldering = "SOLDERING";         // 焊接
        public const string PipeCrimping = "PIPE_CRIMP";     // 水气管压接头
        public const string InsertTerminal = "TERM_INSERT";  // 插端子 (穿胶壳)

        // --- 4. 组装与防护 (Assembly & Protection) ---
        public const string TubeShrinking = "TUBE_SHRINK";   // 吹热缩管
        public const string AssemblingCover = "COVER_ASSY";  // 装外壳
        public const string SheetMetalAssy = "SHEET_METAL";  // 钣金装配
        public const string BackpackAssy = "BACKPACK";       // 背包装配 (特定工艺)
        public const string Dispensing = "DISPENSING";       // 点胶
        public const string FinalAssembly = "FINAL_ASSY";    // 总装装配

        // --- 5. 标识与测试 (Identification & Testing) ---
        public const string Labeling = "LABELING";           // 贴标签
        public const string PressureTest = "PRESS_TEST";     // 水压测试

        // --- 6. 完工 (Ending) ---
        public const string AssemblyEnd = "ASSY_END";        // 装配完工
        public const string InspectionEnd = "INSPE_END";        // 检验完工

        // --- 7. 物流与异常 (Logistics & Exception) ---
        public const string Replenishment = "REPLENISH";     // 补料 (生产过程中的临时补货)
        public const string MaterialReturn = "MAT_RETURN";   // 退料 (剩余物料退库)

        public const string GoodsReceipt = "RECEIPT";   // 收货入库


        // --- 8. SCP工艺 ---
        // ==========================================
        // 1. 电缆制造核心工艺 (Cable Production)
        // ==========================================

        /// <summary>
        /// 绝缘/挤塑 (Insulation / Extrusion)
        /// 描述：芯线挤出，将导体包裹绝缘层
        /// </summary>
        public const string Insulation = "INSULATION";

        /// <summary>
        /// 松套/保护层 (Loose Tube)
        /// 描述：光纤或特种线缆的松套管挤出
        /// </summary>
        public const string LooseTube = "LOOSE_TUBE";

        /// <summary>
        /// 双绞 (Pair Twisting)
        /// 描述：将两根芯线绞合成对 (Twisted Pair)
        /// </summary>
        public const string PairTwisting = "PAIR_TWIST";

        /// <summary>
        /// 单绞 (Single Twisting / Cabling)
        /// 描述：成缆工艺，将多根芯线或线对绞合成缆芯
        /// </summary>
        public const string SingleTwisting = "SINGLE_TWIST"; // 或 "CABLING"

        /// <summary>
        /// 编织 (Braiding)
        /// 描述：金属丝屏蔽层编织
        /// </summary>
        public const string Braiding = "BRAIDING";

        /// <summary>
        /// 复绕 (Rewinding)
        /// 描述：倒轴、分盘或火花重测
        /// </summary>
        public const string Rewinding = "REWINDING";

        /// <summary>
        /// 护套 (Sheathing / Jacketing)
        /// 描述：外被挤出，最终成缆
        /// </summary>
        public const string Sheathing = "SHEATHING";

        // ==========================================
        // 2. 检验与入库 (Quality & Logistics)
        // ==========================================

        /// <summary>
        /// 终检测试 (Final Testing)
        /// 描述：耐压、绝缘电阻、导通等电气性能测试
        /// </summary>
        public const string FinalTest = "FINAL_TEST";

        // ==========================================
        // 3. 辅助动作 (Sub-Types for MaterialTask)
        // ==========================================

        /// <summary>
        /// 投料/上料 (Feeding)
        /// 描述：上述所有生产工序（绝缘、编织、护套等）通用的物料级任务动作
        /// </summary>
        public const string Feeding = "FEEDING";


    }
}
