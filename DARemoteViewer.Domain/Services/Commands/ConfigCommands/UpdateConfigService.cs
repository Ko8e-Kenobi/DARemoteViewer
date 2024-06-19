using DARemoteViewer.Domain.Services.Commands;

namespace DARemoteViewer.Domain.Services.Commands.ConfigCommands
{
    public class UpdateConfigService : IConfigService<UpdateConfig>
    {
        public void Execute(CommandBase command)
        {
            if (command is UpdateConfig)
            {
                command.Execute();
            }
        }
    }
}
