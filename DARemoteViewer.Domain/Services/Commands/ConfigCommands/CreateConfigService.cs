using DARemoteViewer.Domain.Services.Commands;

namespace DARemoteViewer.Domain.Services.Commands.ConfigCommands
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
