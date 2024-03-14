using DARemoteViewer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DARemoteViewer.Domain.Services.ConfigServices.CommandServices
{
    public class CreateConfig : ConfigCommandBase
    {
        private string newFileName { get; set; }
        private Config newConfig { get; set; }
        public CreateConfig(string newFileName, Config newConfig)
        {
            this.newFileName = newFileName;
            this.newConfig = newConfig;
        }
        public override void Execute()
        {
            Create(newFileName,newConfig);
        }
        private void Create(string newFileName, Config newConfig) 
        {
            newConfig.fileName = newFileName;
            newConfig.Name = Path.GetFileName(newFileName);
            XmlSerializer xs = new XmlSerializer(typeof(Config));
            using (TextWriter tw = new StreamWriter(newFileName))
            { xs.Serialize(tw, newConfig); };
        }
    }
}
