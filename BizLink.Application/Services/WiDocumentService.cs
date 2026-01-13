using AutoMapper;
using BizLink.MES.Application.ApiClient;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Repositories;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public class WiDocumentService : IWiDocumentService
    {
        private readonly IMesApiClient _apiClient;
        private readonly ServiceEndpointSettings _apiSettings; // 假设您有配置类
        private readonly IWiDocumentRepository _wiDocumentRepository;
        private readonly IMapper _mapper; // 声明 IMapper

        public WiDocumentService(IMesApiClient apiClient, IOptions<Dictionary<string, ServiceEndpointSettings>> apiSettings, IWiDocumentRepository wiDocumentRepository, IMapper mapper)
        {
            _apiClient = apiClient;
            _apiSettings = apiSettings.Value["MesApi"]; // 获取 MES API 配置
            _wiDocumentRepository = wiDocumentRepository;
            _mapper = mapper;
        }

        public async Task<WiDocumentDto> CreateAsync(WiDocumentCreateDto createDto)
        {

            var entity = _mapper.Map<WiDocument>(createDto);
            var result =  await _wiDocumentRepository.AddAsync(entity);
            return _mapper.Map<WiDocumentDto>(result);
        }

        public Task<List<int>> CreateBatchAsync(List<WiDocumentCreateDto> createDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<WiDocumentDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<WiDocumentDto> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<WiDocumentDto>> GetListByMaterialCodeAsync(int factoryid, string materialcode)
        {
            var entities = await _wiDocumentRepository.GetListByMaterialCodeAsync(factoryid, materialcode);
            return _mapper.Map<List<WiDocumentDto>>(entities);
        }

        public Task<bool> UpdateAsync(WiDocumentUpdateDto updateDto)
        {
            throw new NotImplementedException();
        }

        // --- 组合方法：上传 + 保存 ---
        public async Task<WiDocumentDto> UploadAndSaveAsync(string localFilePath, WiDocumentCreateDto createDto)
        {
            // 1. 先上传物理文件
            string serverPath = await UploadPdfAsync(localFilePath, "WiDocument");

            // 2. 补全 DTO 信息
            createDto.DocumentPath = serverPath;
            createDto.DocumentName = Path.GetFileName(localFilePath);


            // 3. 保存到数据库
            var result =  await CreateAsync(createDto);
            return _mapper.Map<WiDocumentDto>(result);
        }

        public async Task<string> UploadPdfAsync(string localFilePath, string docType)
        {
            // 假设 appsettings.json 中配置了 "FileUpload": "api/File/Upload"
            var url = _apiSettings.Endpoints.ContainsKey("UploadPDFFile")
                ? _apiSettings.Endpoints["UploadPDFFile"]
                : "api/File/Upload";

            var formData = new Dictionary<string, string>
            {
                { "docType", docType } // 传递给后端的额外参数
            };

            // 这里的 string 是指后端返回的数据类型，例如返回文件路径
            var result = await _apiClient.UploadFileAsync<dynamic>(url, localFilePath, formData);

            if (result.IsSuccess)
            {
                // 假设后端返回 { "filePath": "/Resources/..." }
                return result.Data.filePath.ToString();
            }
            else
            {
                throw new System.Exception(result.Message);
            }
        }
    }
}
