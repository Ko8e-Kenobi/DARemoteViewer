using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DARemoteViewer;
using System.IO;
using System.Collections.ObjectModel;
using DARemoteViewer.Domain.Services.ConfigServices.CommandServices;
namespace DARemoteViewer.Domain.Models
{
    public class Config : DomainModel
    {
        public string fileName {  get; set; }
        public ObservableCollection<DAConnection> connections { get; set; }
    }
}
