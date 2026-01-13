using AutoMapper;
using BCrypt.Net;
using BizLink.MES.Application.Common;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper; // 2. 声明 IMapper


        public UserService(IUserRepository userRepository, ICurrentUserService currentUserService, IMapper mapper)
        {
            _userRepository = userRepository;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<UserDto> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) throw new Exception("用户不存在");
            // 手动映射
            return new UserDto
            {
                Id = user.Id,
                EmployeeId = user.EmployeeId,
                DomainAccount = user.DomainAccount,
                UserName = user.UserName,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                FactoryName = (user as dynamic).FactoryName // 假设 FactoryName 是动态属性
            };
        }

        public async Task<UserDto> GetByDomainAccountAsync(string domainAccount)
        {
            var user = await _userRepository.GetByDomainAccountAsync(domainAccount);

            return _mapper.Map<UserDto>(user);

            // 手动映射
            //return new UserDto
            //{
            //    Id = user.Id,
            //    EmployeeId = user.EmployeeId,
            //    DomainAccount = user.DomainAccount,
            //    UserName = user.UserName,
            //    IsActive = user.IsActive,
            //    CreatedAt = user.CreatedAt,
            //    FactoryName = (user as dynamic).FactoryName // 假设 FactoryName 是动态属性
            //};

        }

        public async Task<PagedResultDto<UserDto>> GetPagedListAsync(int pageIndex, int pageSize, string keyword, bool? isActive)
        {
            var (users, totalCount) = await _userRepository.GetPagedListAsync(pageIndex, pageSize, keyword, isActive);

            // 手动进行批量映射
            var userDtos = users.Select(u => new UserDto
            {
                Id = u.Id,
                EmployeeId = u.EmployeeId,
                DomainAccount = u.DomainAccount,
                UserName = u.UserName,
                IsActive = u.IsActive,
                CreatedAt = u.CreatedAt,
                FactoryName = (u as dynamic).FactoryName
            });

            return new PagedResultDto<UserDto> { Items = userDtos, TotalCount = totalCount };
        }


        public async Task<UserDto> CreateAsync(UserCreateDto input)
        {
            try
            {
                var userExists = await _userRepository.GetByEmployeeIdAsync(input.EmployeeId);
                if (userExists != null) throw new Exception("工号已存在");
                if (!string.IsNullOrWhiteSpace(input.DomainAccount))
                {
                    userExists = await _userRepository.GetByDomainAccountAsync(input.DomainAccount);
                    if (userExists != null) throw new Exception("域账号已存在");
                }

                var user = _mapper.Map<User>(input);

                var rtn = await _userRepository.AddAsync(user);
                return _mapper.Map<UserDto>(rtn);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<bool> UpdateAsync(UserUpdateDto input)
        {
            var user = await _userRepository.GetByIdAsync(input.Id);
            if (user == null) throw new Exception("用户不存在");

            // 手动将更新DTO的属性映射到已存在的实体上
            //user.EmployeeId = input.EmployeeId;
            //user.DomainAccount = input.DomainAccount;
            //user.UserName = input.UserName;
            //user.FactoryId = input.FactoryId;
            //user.IsActive = input.IsActive;

            _mapper.Map(input, user);

            return await _userRepository.UpdateAsync(user);
        }

        public async Task<bool> DeleteAsync(int id)
        {
           return  await _userRepository.DeleteAsync(id);
        }


        public Task<IEnumerable<UserDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<UserDto> GetByEmployeeIdAsync(string employeeId)
        {
            var entity =await  _userRepository.GetByEmployeeIdAsync(employeeId);
            return _mapper.Map<UserDto>(entity);
        }

        public Task<List<int>> CreateBatchAsync(List<UserCreateDto> createDto)
        {
            throw new NotImplementedException();
        }

        public async Task<List<UserDto>> GetByEmployeeIdAsync(List<string> employeeIds)
        {
            var entities = await _userRepository.GetByEmployeeIdAsync(employeeIds);
            return _mapper.Map<List<UserDto>>(entities);
        }
    }
}
