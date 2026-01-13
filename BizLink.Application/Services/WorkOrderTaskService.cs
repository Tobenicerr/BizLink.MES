using AutoMapper;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Domain.Common;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Repositories;
using BizLink.MES.Infrastructure.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public class WorkOrderTaskService : IWorkOrderTaskService
    {

        private readonly IWorkOrderTaskRepository _workOrderTaskRepository;

        private readonly IWorkOrderTaskMaterialAddRepository _workOrderTaskMaterialAddRepository;
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper; // 2. 声明 IMapper

        public WorkOrderTaskService(IWorkOrderTaskRepository workOrderTaskRepository, IMapper mapper,IUnitOfWork unitOfWork, IWorkOrderTaskMaterialAddRepository workOrderTaskMaterialAddRepository)
        {
            _workOrderTaskRepository = workOrderTaskRepository;
            _workOrderTaskMaterialAddRepository = workOrderTaskMaterialAddRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async  Task<bool> BatchCreateAsync(List<WorkOrderTaskCreateDto> input)
        {
            var entity = input.Select(x => _mapper.Map<WorkOrderTask>(x)).ToList();
            return await _workOrderTaskRepository.BatchAddAsync(entity);
        }

        public async Task<WorkOrderTaskDto> CreateAsync(WorkOrderTaskCreateDto input)
        {

            var entity = _mapper.Map<WorkOrderTask>(input);

            var result = await _workOrderTaskRepository.AddAsync(entity);
            return _mapper.Map<WorkOrderTaskDto>(result);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _workOrderTaskRepository.DeleteAsync(id);
        }

        public async Task<WorkOrderTaskDto> GetByIdAsync(int id)
        {
            var entity = await _workOrderTaskRepository.GetByIdAsync(id);
            return _mapper.Map<WorkOrderTaskDto>(entity);
        }

        public async Task<List<WorkOrderTaskDto>> GetByIdAsync(List<int> ids)
        {
            var entities = await _workOrderTaskRepository.GetListByIdsAsync(ids);
            return _mapper.Map<List<WorkOrderTaskDto>>(entities);
        }

        public async Task<List<WorkOrderTaskDto>> GetByOrderNoAsync(string orderno)
        {
            var entity = await _workOrderTaskRepository.GetByOrderNoAsync(orderno);
            return entity.Select(x => _mapper.Map<WorkOrderTaskDto>(x)).ToList();
        }

        public async Task<WorkOrderTaskDto> GetByProcessIdAsync(int id, string cableItem)
        {
            var entity = await _workOrderTaskRepository.GetByProcessIdAsync(id, cableItem);
            return _mapper.Map<WorkOrderTaskDto>(entity);
        }

        public async Task<List<WorkOrderTaskDto>> GetByProcessIdsAsync(List<int> processid)
        {
            var entity = await _workOrderTaskRepository.GetByProcessIdsAsync(processid);
            return entity.Select(x => _mapper.Map<WorkOrderTaskDto>(x)).ToList();
        }

        public async Task<List<WorkOrderTaskDto>> GetByTaskNoAsync(string taskno)
        {
            var entity = await _workOrderTaskRepository.GetByTaskNoAsync(taskno);
            return entity.Select(x => _mapper.Map<WorkOrderTaskDto>(x)).ToList();
        }

        public async Task<List<WorkOrderTaskDto>> GetListByOrderIdsAsync(List<int> orderid)
        {
            var entities = await _workOrderTaskRepository.GetListByOrderIdsAsync(orderid);
            return _mapper.Map<List<WorkOrderTaskDto>>(entities);
        }

        public async Task SuspendTask(int taskid)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var task = await _workOrderTaskRepository.GetByIdAsync(taskid);
                task.Status = "3"; //3-暂停
                await _workOrderTaskRepository.UpdateAsync(task);

                var materialList = await _workOrderTaskMaterialAddRepository.GetByTaskIdAsync(taskid);
                if (materialList != null && materialList.Where(x => x.Status.Equals("1")).Count() > 0)
                {
                    foreach (var item in materialList.Where(x => x.Status.Equals("1")))
                    {
                        await _workOrderTaskMaterialAddRepository.UpdateAsync(new WorkOrderTaskMaterialAdd()
                        {
                            Id = item.Id,
                            Status = "0"
                        });
                    }
                }


                await _unitOfWork.CommitAsync();


            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }

        }

        public async  Task<bool> UpdateAsync(WorkOrderTaskUpdateDto input)
        {
            var entity = await _workOrderTaskRepository.GetByIdAsync(input.Id);
            _mapper.Map(input, entity);
           return await _workOrderTaskRepository.UpdateAsync(entity);

        }
    }
}
