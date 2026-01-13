using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Repositories
{
    public interface IWiDocumentRepository : IGenericRepository<WiDocument>
    {

        Task<List<WiDocument>> GetListByMaterialCodeAsync(int factoryid, string materialcode);
    }
}
