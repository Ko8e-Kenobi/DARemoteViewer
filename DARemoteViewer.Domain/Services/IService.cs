using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DARemoteViewer.Domain.Services
{
    public interface IService<T>
    {
        public void Execute(T command);
    }
}
