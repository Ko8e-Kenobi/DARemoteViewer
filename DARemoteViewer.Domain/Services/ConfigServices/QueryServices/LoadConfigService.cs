using DARemoteViewer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DARemoteViewer.Domain.Services.ConfigServices.QueryServices
{
    public class LoadConfigService : IConfigQuery<Config>
    {
        public Config Execute(string fileName)
        {
            XmlSerializer xs = new XmlSerializer(typeof(Config));
            try
            {
                using (var sr = new StreamReader(fileName))
                {
                    Config config = (Config)xs.Deserialize(sr);
                    return config;
                }
            }
            catch (Exception)
            {
                string defaultConfigFile = $"{Directory.GetCurrentDirectory()}\\Resources\\DefaultConfig.xml";
                using (var sr = new StreamReader(defaultConfigFile))
                {
                    Config config = (Config)xs.Deserialize(sr);
                    config.fileName = defaultConfigFile;
                    return config;
                }
            }
        }
    }
}
