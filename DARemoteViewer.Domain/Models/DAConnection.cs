using System.Collections.ObjectModel;

namespace DARemoteViewer.Domain.Models
{
    public class DAConnection : DomainModel
    {
        public DAUser User { get; set; }
        public string hostName {  get; set; }
        public string IPAddress { get; set; }
        public Collection<DAService> Service { get; set; }
    }
}
