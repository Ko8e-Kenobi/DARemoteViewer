using System.Collections.ObjectModel;

namespace DARemoteViewer.Domain.Models
{
    public class Config : DomainModel
    {
        public string fileName {  get; set; }
        public ObservableCollection<DAConnection> connections { get; set; }
    }
}
