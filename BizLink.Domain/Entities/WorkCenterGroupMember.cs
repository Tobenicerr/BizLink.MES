using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Entities
{
    [SugarTable("Mes_WorkCenterGroupMember", IsDisabledUpdateAll = true)]

    public class WorkCenterGroupMember
    {
        public int GroupId
        {
            get; set;
        }
        public int WorkCenterId
        {
            get; set;
        }
    }
}
