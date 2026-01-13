using BizLink.MES.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Helper
{
    public class SerialHelperService: ISerialHelperService
    {
        private readonly ISerialSequenceRepository _serialSequenceRepository;


        public SerialHelperService(ISerialSequenceRepository serialSequenceRepository)
        {
            _serialSequenceRepository = serialSequenceRepository;
        }
        public string GenerateNext(string name)
        {
            return _serialSequenceRepository.GenerateNext(name);
        }
    }
}
