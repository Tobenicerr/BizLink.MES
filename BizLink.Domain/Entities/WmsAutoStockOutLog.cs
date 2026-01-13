using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Entities
{
    [SugarTable("WMSRBTOutRep")]
    public class WmsAutoStockOutLog
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }


        [SugarColumn(IsNullable = true, ColumnName = "BILLNO", Length = 50)]
        public string? BillNo { get; set; }

        [SugarColumn(IsNullable = true, ColumnName = "MATERIALCODE", Length = 50)]

        public string? MaterialCode { get; set;}

        [SugarColumn(IsNullable = true, ColumnName = "BATCH", Length = 50)]

        public string? BatchCode { get; set;
        }

        [SugarColumn(IsNullable = true, ColumnName = "QTY")]

        public decimal Quantity { get; set;}

        [SugarColumn(IsNullable = true, ColumnName = "BARCODEOD", Length = 50)]

        public string? BarCode { get; set;
        }

        [SugarColumn(IsNullable = true, ColumnName = "BARCODENEW", Length = 50)]

        public string? BarCodeNew { get; set;
        }
        [SugarColumn(IsNullable = true, ColumnName = "PLANT", Length = 20)]

        public string? FactoryCode { get; set;
        }

        [SugarColumn(IsNullable = true, ColumnName = "PROCESSFLG")]

        public int ProcessFlag { get; set;
        }
        [SugarColumn(IsNullable = true, ColumnName = "PROCESSREMARK1", Length = 255)]

        public string? Message { get; set;
        }

        [SugarColumn(IsNullable = true, ColumnName = "ADDTIME")]

        public DateTime? CreateTime { get; set;
        }
        [SugarColumn(IsNullable = true, ColumnName = "PROCESSTIME")]

        public DateTime? UpdateTime { get; set; }
    }
}
