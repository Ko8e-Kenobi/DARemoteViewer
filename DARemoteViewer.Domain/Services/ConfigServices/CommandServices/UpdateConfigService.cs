using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using DARemoteViewer.Domain.Models;

namespace DARemoteViewer.Domain.Services.ConfigServices.CommandServices
{
    public class UpdateConfigService : IConfigService<UpdateConfig>
    {
        public void Execute(ConfigCommandBase command)
        {
            if (command is UpdateConfig) 
            {
                command.Execute();
            }
        }
    }
}
