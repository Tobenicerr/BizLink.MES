using BizLink.MES.Domain.Common;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Repositories;
using BizLink.MES.Infrastructure.Persistence.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Infrastructure.Persistence.Repositories
{
    public class WiDocumentRepository : GenericRepository<WiDocument>, IWiDocumentRepository
    {
        public WiDocumentRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<WiDocument> GetByDocumentNoAsync(string documentNo)
        {
            return await _db.Queryable<WiDocument>().Where(x => x.DocumentNo == documentNo).FirstAsync();
        }

        public async Task<List<WiDocument>> GetListByConstructionAsync(string constructionNo)
        {
            return await _db.Queryable<WiDocument>().Where(x => x.ConstructionNo == constructionNo).ToListAsync();
        }
    }
}
