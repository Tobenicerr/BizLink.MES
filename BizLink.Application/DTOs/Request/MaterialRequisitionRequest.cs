using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.DTOs.Request
{

    public class MaterialRequisitionRequest
    {
        public string? VoucherNo
        {
            get; set;
        }
        public string VoucherTypeCategoryCode
        {
            get; set;
        }

        public string VoucherTypeName
        {
            get; set;
        }
        public string OWarehouseCode
        {
            get; set;
        }
        public string IWarehouseCode
        {
            get; set;
        }
        public string OStorageCode
        {
            get; set;
        }
        public string IStorageCode
        {
            get; set;
        }
        public string VirtualOPlantCode
        {
            get; set;
        }
        public string VirtualIPlantCode
        {
            get; set;
        }
        public List<MaterialEntry> EntryDtos
        {
            get; set;
        }
        public string PlantCode
        {
            get; set;
        }
        public bool IsActive
        {
            get; set;
        } = false;
        public string? IsActiveDisplay
        {
            get; set;
        }
        public string OperateUser
        {
            get; set;
        }
        public string OperateUserName
        {
            get; set;
        }
        public string Remark
        {
            get; set;
        }
    }

    public class MaterialEntry
    {
        public int MaterialId
        {
            get; set;
        }
        public decimal Qty
        {
            get; set;
        }

    }
}
