using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DARemoteViewer.Domain.Services.ConfigServices.QueryServices
{
    public interface IConfigQuery<T>
    {
        public T Execute(string fileName);
    }
}
