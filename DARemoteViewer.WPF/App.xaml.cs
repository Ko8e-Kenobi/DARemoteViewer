using System.Collections.ObjectModel;
using System.Windows;
using DARemoteViewer.Domain.Models;
using DARemoteViewer.Domain.Services;
using DARemoteViewer.Domain.Services.Commands;
using DARemoteViewer.Domain.Services.Commands.ConfigCommands;
using DARemoteViewer.Domain.Services.Queries.ConfigQueries;
using DARemoteViewer.WPF.ViewModel;

namespace DARemoteViewer.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            
            var commands = new ObservableCollection<IService<CommandBase>>();
            commands.Add(new CreateConfigService());
            commands.Add(new UpdateConfigService());

            var configQueries = new ObservableCollection<IQuery<Config,string>>();
            configQueries.Add(new LoadConfigService());



            var viewModel = new MainWindowViewModel(configQueries, commands);
            Window window = new MainWindow(viewModel);
            window.Show();
        }
    }
}
