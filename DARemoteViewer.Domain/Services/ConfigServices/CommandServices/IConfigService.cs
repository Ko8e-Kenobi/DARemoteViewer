using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DARemoteViewer.Domain.Services.ConfigServices.CommandServices
{
    public interface IConfigService<T> : IService<ConfigCommandBase>
    {
    }
}
