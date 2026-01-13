using BizLink.MES.Domain.Attributes;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Entities
{
    [SugarTable("Mes_WorkOrderOperationConfirm", IsDisabledUpdateAll = true)]
    public class WorkOrderOperationConfirm
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id
        {
            get; set;
        }
        [SugarColumn(IsNullable = true)]
        public int? WorkOrderId
        {
            get; set;
        }
        [SugarColumn(IsNullable = true)]
        public int? ProcessId
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]

        public int? TaskConfirmId
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, Length = 10)]
        [SapFieldName("CONF_NO")]

        public int? SapConfirmationNo
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, Length = 50)]
        [SapFieldName("ORDERID")]
        public string? WorkOrderNo
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, Length = 50)]
        [SapFieldName("SEQUENCE")]
        public string? ConfirmSequence
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, Length = 5)]
        [SapFieldName("FIN_CONF")]
        public string? CompletedFlag
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, Length = 10)]
        [SapFieldName("OPERATION")]
        public string? OperationNo
        {
            get; set;
        }


        [SugarColumn(IsNullable = true, Length = 10)]
        [SapFieldName("WORK_CNTR")]
        public string? WorkCenterCode
        {
            get; set;
        }
        [SugarColumn(IsNullable = true)]
        [SapFieldName("POSTG_DATE")]
        public DateTime? PostingDate
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, ColumnDataType = "nvarchar(255)")]
        [SapFieldName("CONF_TEXT")]
        public string? Remark
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, Length = 50)]
        [SapFieldName("PERS_NO")]
        public string? EmployeeId
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, Length = 10)]
        [SapFieldName("PLANT")]
        public string? FactoryCode
        {
            get; set;
        }



        [SugarColumn(IsNullable = true, Length = 10)]
        [SapFieldName("CONF_QUAN_UNIT")]
        public string? BaseUnit
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, ColumnDataType = "decimal(18, 4)")]
        [SapFieldName("YIELD")]
        public decimal? YieldQuantity
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, ColumnDataType = "decimal(18, 4)")]
        [SapFieldName("SCRAP")]
        public decimal? ScrapQuantity
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, Length = 20)]
        [SapFieldName("EXEC_START_DATE")]
        public string? ActStartDate
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, Length = 20)]
        [SapFieldName("EXEC_START_TIME")]
        public string? ActStartTime
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, Length = 20)]
        [SapFieldName("EXEC_FIN_DATE")]
        public string? ActFinishDate
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, Length = 20)]
        [SapFieldName("EXEC_FIN_TIME")]
        public string? ActFinishTime
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, ColumnDataType = "nvarchar(255)")]
        [SapFieldName("MSG")]
        public string? Message
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, Length = 20)]
        [SapFieldName("MSG_TYPE")]
        public string? MessageType
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, Length = 20)]
        public string? Status
        {
            get; set;
        } = "0";

        [SugarColumn(IsNullable = true)]
        public DateTime CreatedAt
        {
            get; set;
        } = DateTime.Now;
        [SugarColumn(IsNullable = true, Length = 50)]
        public string? CreatedBy
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]
        public DateTime? UpdatedAt
        {
            get; set;
        }
        [SugarColumn(IsNullable = true, Length = 50)]
        public string? UpdatedBy
        {
            get; set;
        }
        [Navigate(NavigateType.OneToMany, nameof(WorkOrderOperationConsump.OperationConfirmId))]

        public List<WorkOrderOperationConsump>? Consumps
        {
            get; set;
        }
    }
}
