using AutoMapper;
using BizLink.MES.Application.Mappings;
using BizLink.MES.Domain.Attributes;
using BizLink.MES.Domain.Entities;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.DTOs
{
    public class CableCutParamDto :IMapFrom<CableCutParam>
    {
        public int Id
        {
            get; set;
        }
        public int? CuttoLeranceId
        {
            get; set;
        }

        public string? SemiMaterialCode
        {
            get; set;
        }
        public string? CableMaterialCode
        {
            get; set;
        }

        public string? CableType
        {
            get; set;
        }

        public string? DrawingCode
        {
            get; set;
        }

        public string? PositionItem
        {
            get; set;
        }

        public int? CablePcs
        {
            get; set;
        }

        public string? PostionNo
        {
            get; set;
        }

        public decimal? BomLength
        {
            get; set;
        }

        public decimal? UpTol
        {
            get; set;
        }

        public decimal? DownTol
        {
            get; set;
        }

        public decimal? AlphaFactor
        {
            get; set;
        }

        public decimal? BetaFactor
        {
            get; set;
        }
        public decimal? CuttingLength
        {
            get; set;
        }

        public decimal? CuttingTime
        {
            get; set;
        }

        public string? ReelCode
        {
            get; set;
        }

        public string? Remark
        {
            get; set;
        }
        public string? Status
        {
            get; set;
        }
        public string? CreateDate
        {
            get; set;
        }
        public string? CreateTime
        {
            get; set;
        }
        public string? CreateBy
        {
            get; set;
        }

        public string? UpdateDate
        {
            get; set;
        }

        public string? UpdateTime
        {
            get; set;
        }
        public string? UpdateBy
        {
            get; set;
        }
    }

    public class CableCutParamCreateDto : IMapFrom<CableCutParam>
    {
        public int? CuttoLeranceId
        {
            get; set;
        }

        private string? _semiMaterialCode;
        public string? SemiMaterialCode
        {
            get
            {
                return _semiMaterialCode;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _semiMaterialCode = value;
                }
                else
                {
                    _semiMaterialCode = value.TrimStart('0');
                }
            }
        }

        private string? _cableMaterialCode;

        public string? CableMaterialCode
        {
            get
            {
                return _cableMaterialCode;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _cableMaterialCode = value;
                }
                else
                {
                    _cableMaterialCode = value.TrimStart('0');
                }
            }
        }

        public string? CableType
        {
            get; set;
        }

        public string? DrawingCode
        {
            get; set;
        }

        public string? PositionItem
        {
            get; set;
        }

        public int? CablePcs
        {
            get; set;
        }

        public string? PostionNo
        {
            get; set;
        }

        public decimal? BomLength
        {
            get; set;
        }

        public decimal? UpTol
        {
            get; set;
        }

        public decimal? DownTol
        {
            get; set;
        }

        public decimal? AlphaFactor
        {
            get; set;
        }

        public decimal? BetaFactor
        {
            get; set;
        }
        public decimal? CuttingLength
        {
            get; set;
        }

        public decimal? CuttingTime
        {
            get; set;
        }

        public string? ReelCode
        {
            get; set;
        }

        public string? Remark
        {
            get; set;
        }
        public string? Status
        {
            get; set;
        }
        public string? CreateDate
        {
            get; set;
        }
        public string? CreateTime
        {
            get; set;
        }
        public string? CreateBy
        {
            get; set;
        }

        public string? UpdateDate
        {
            get; set;
        }

        public string? UpdateTime
        {
            get; set;
        }
        public string? UpdateBy
        {
            get; set;
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CableCutParamCreateDto, CableCutParam>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

    public class CableCutParamUpdateDto : IMapFrom<CableCutParam>
    {

    }
}
