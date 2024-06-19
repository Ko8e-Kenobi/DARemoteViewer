using DARemoteViewer.Domain.Models;
using DARemoteViewer.Domain.Services.Commands;
using System.Xml.Serialization;

namespace DARemoteViewer.Domain.Services.Commands.ConfigCommands
{
    public class UpdateConfig : CommandBase
    {
        public Config ConfigToUpdate { get; set; }
        public UpdateConfig(Config toUpdate)
        {
            ConfigToUpdate = toUpdate;
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
