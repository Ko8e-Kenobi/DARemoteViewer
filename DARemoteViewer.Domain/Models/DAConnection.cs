using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DARemoteViewer.Domain.Models
{
    public class DAConnection : DomainModel
    {
        public DAUser User { get; set; }

        public Collection<DAService> Service { get; set; }
    }
}
