using System.Collections.ObjectModel;

namespace DARemoteViewer.Domain.Models
{
    public class DAConnection : DomainModel
    {
        public string Name {  get; set; }
        public DAUser User { get; set; }
        public string hostName {  get; set; }
        public string IPAddress { get; set; }
        public ObservableCollection<DAService> Services { get; set; }
    }
}
