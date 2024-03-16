using DARemoteViewer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Serialization;
namespace DARemoteViewer.Domain.Services.ConfigServices.CommandServices
{
    public class CreateConfigService : IConfigService<CreateConfig>
    {
        public void Execute(CommandBase command)
        {
            if (command is CreateConfig)
            {
                command.Execute();
            }
        }
    }
}
