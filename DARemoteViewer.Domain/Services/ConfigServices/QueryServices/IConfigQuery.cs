using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DARemoteViewer.Domain.Services.ConfigServices.QueryServices
{
    public interface IConfigQuery<ReturnT,InputT>
    {
        public ReturnT Execute(InputT command);
    }
}
