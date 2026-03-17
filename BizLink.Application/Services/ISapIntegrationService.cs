using BizLink.MES.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public interface ISapIntegrationService
    {
        Task TransferToSapAsync(List<MaterialTransferLogCreateDto> dtos, string employeeId);
    }
}
