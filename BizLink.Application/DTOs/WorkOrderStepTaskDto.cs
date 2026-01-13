using AutoMapper;
using BizLink.MES.Application.Mappings;
using BizLink.MES.Domain.Entities;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.DTOs
{
    public class WorkOrderStepTaskDto : IMapFrom<WorkOrderStepTask>
    {
        public int Id
        {
            get; set;
        }
        public int? OperationTaskId
        {
            get; set;
        }

        public int? StepId
        {
            get; set;
        }

        public string? StepCode
        {
            get; set;
        }

        public int? WorkCenterGroupId
        {
            get; set;
        }

        public string? WorkCenterGroupCode
        {
            get; set;
        }

        public int? ActualWorkCenterId
        {
            get; set;
        }

        public string? ActualWorkCenterCode
        {
            get; set;
        }

        public int? ActualWorkStationId
        {
            get; set;
        }

        public string? ActualWorkStationCode
        {
            get; set;
        }

        public decimal? Quantity
        {
            get; set;
        }

        public decimal? CompletedQuantity
        {
            get; set;
        }

        /// <summary>
        /// NORMAL, REWORK(返工), SAMPLE(抽检), TRIAL(试制)
        /// </summary>
        public string? TaskCategory
        {
            get; set;
        }

        public string? PreStepIds
        {
            get; set;
        }

        public string? Status
        {
            get; set;
        }

        public DateTime? CreatedOn
        {
            get; set;
        }

        public string? CreatedBy
        {
            get; set;
        }

        public DateTime? UpdatedOn
        {
            get; set;
        }
        public string? UpdateBy
        {
            get; set;
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<WorkOrderStepTask, WorkOrderStepTaskDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

    public class WorkOrderStepTaskCreateDto : IMapFrom<WorkOrderStepTask>
    {
        public int? OperationTaskId
        {
            get; set;
        }

        public int? StepId
        {
            get; set;
        }

        public string? StepCode
        {
            get; set;
        }

        public int? WorkCenterGroupId
        {
            get; set;
        }

        public string? WorkCenterGroupCode
        {
            get; set;
        }

        public int? ActualWorkCenterId
        {
            get; set;
        }

        public string? ActualWorkCenterCode
        {
            get; set;
        }

        public int? ActualWorkStationId
        {
            get; set;
        }

        public string? ActualWorkStationCode
        {
            get; set;
        }

        public decimal? Quantity
        {
            get; set;
        }

        public decimal? CompletedQuantity
        {
            get; set;
        }

        /// <summary>
        /// NORMAL, REWORK(返工), SAMPLE(抽检), TRIAL(试制)
        /// </summary>
        public string? TaskCategory
        {
            get; set;
        }

        public string? PreStepIds
        {
            get; set;
        }

        public string? Status
        {
            get; set;
        }

        public string? CreatedBy
        {
            get; set;
        }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<WorkOrderStepTaskCreateDto, WorkOrderStepTask>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

    public class WorkOrderStepTaskUpdateDto : IMapFrom<WorkOrderStepTask>
    {
        public int Id
        {
            get; set;
        }
        public int? OperationTaskId
        {
            get; set;
        }

        public int? StepId
        {
            get; set;
        }

        public string? StepCode
        {
            get; set;
        }

        public int? WorkCenterGroupId
        {
            get; set;
        }

        public string? WorkCenterGroupCode
        {
            get; set;
        }

        public int? ActualWorkCenterId
        {
            get; set;
        }

        public string? ActualWorkCenterCode
        {
            get; set;
        }

        public int? ActualWorkStationId
        {
            get; set;
        }

        public string? ActualWorkStationCode
        {
            get; set;
        }

        public decimal? Quantity
        {
            get; set;
        }

        public decimal? CompletedQuantity
        {
            get; set;
        }

        /// <summary>
        /// NORMAL, REWORK(返工), SAMPLE(抽检), TRIAL(试制)
        /// </summary>
        public string? TaskCategory
        {
            get; set;
        }

        public string? PreStepIds
        {
            get; set;
        }

        public string? Status
        {
            get; set;
        }

        public DateTime? UpdatedOn
        {
            get; set;
        }
        public string? UpdateBy
        {
            get; set;
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<WorkOrderStepTaskUpdateDto, WorkOrderStepTask>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

}
