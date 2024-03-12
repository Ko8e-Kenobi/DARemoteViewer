using System.CodeDom;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;
using DARemoteViewer.Domain.Models;
using DARemoteViewer.Domain.Services;
using DARemoteViewer.Domain.Services.ConfigServices;
using DARemoteViewer.Domain.Services.ConfigServices.CommandServices;
using DARemoteViewer.Domain.Services.ConfigServices.QueryServices;
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
            var configCommands = new ObservableCollection<IService<ConfigCommandBase>>();
            configCommands.Add(new CreateConfigService());
            configCommands.Add(new UpdateConfigService());

            var configQueries = new ObservableCollection<IConfigQuery<Config>>();
            configQueries.Add(new LoadConfigService());

            var viewModel = new MainWindowViewModel(configQueries, configCommands);
            Window window = new MainWindow(viewModel);
            window.Show();
        }
    }

}
