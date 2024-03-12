using DARemoteViewer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DARemoteViewer.Domain.Services.ConfigServices.CommandServices
{
    public class UpdateConfig : ConfigCommandBase
    {
        public  Config ConfigToUpdate {  get; set; }
        public UpdateConfig(Config toUpdate)
        {
            this.ConfigToUpdate = toUpdate;
        }
        public override void Execute()
        {
            Update();
        }
        private void Update() 
        {
            XmlSerializer xs = new XmlSerializer(typeof(Config));
            using (TextWriter tw = new StreamWriter(ConfigToUpdate.fileName))
            { xs.Serialize(tw, ConfigToUpdate); };
        }
    }
}
