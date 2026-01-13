using BizLink.MES.Application.Common;
using BizLink.MES.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public interface IWiDocumentService : IGenericService<WiDocumentDto, WiDocumentCreateDto, WiDocumentUpdateDto>
    {
        Task<string> UploadPdfAsync(string localFilePath, string docType);

        Task<List<WiDocumentDto>> GetListByMaterialCodeAsync(int factoryid, string materialcode);

    }
}
