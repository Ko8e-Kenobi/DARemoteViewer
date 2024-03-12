using DARemoteViewer.Domain.Services;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DARemoteViewer.WPF;
using DARemoteViewer.Domain.Services.Static;
using DARemoteViewer.Domain.Models;
using System.Collections.ObjectModel;
using DARemoteViewer.Domain.Services.ConfigServices.QueryServices;
using System.IO;
using System.DirectoryServices;
using System.Windows.Input;
using DARemoteViewer.Domain.Services.ConfigServices.CommandServices;
using DARemoteViewer.Domain.Services.ConfigServices;
using DARemoteViewer.WPF.ViewModel.Commands;
using System.Configuration;
namespace DARemoteViewer.WPF.ViewModel
{
    public class MainWindowViewModel: ViewModelBase
    {
        private Config activeConfig;
        private IConfigQuery<Config> queries;
        private CreateConfigService CreateConfigService;
        private UpdateConfigService UpdateConfigService;
        private UpdateConfig UpdateConfig;
        private CreateConfig CreateConfig;
        string defaultConfigFileName = $"{Directory.GetCurrentDirectory()}\\DefaultConfig.xml";


        public MainWindowViewModel(IConfigQuery<Config> queries, ObservableCollection<IService<ConfigCommandBase>> services) //ObservableCollection<ICommandService<ICommandBase>> commandServices)
        {
            this.queries = queries;
            defaultConfigFileName = StaticMethods.GetAppSettings("ActiveConfigFileName");
            ActiveConfig = this.queries.Execute(defaultConfigFileName);

            if (services != null) 
            {
                foreach (var service in services)
                {
                    if (typeof(CreateConfigService) == service.GetType())
                    {
                        CreateConfigService = (CreateConfigService)service;
                    }
                    else if (typeof(UpdateConfigService) == service.GetType())
                    {
                        UpdateConfigService = (UpdateConfigService)service;
                    }
                    else { throw new Exception("Unknown Service"); }
                }
            }
            else
            {
                throw new Exception("No services passed to constructor");
            }

            //CreateConfigService.Execute(new CreateConfig(defaultConfigFileName, ActiveConfig));
            //ActiveConfig.connections.Add(new DAConnection { Name = "New connection", Id = ActiveConfig.connections.Count() + 1});
            //ActiveConfig.connections.Add(new DAConnection { Name = "Connection", Id = ActiveConfig.connections.Max(x => x.Id) + 1 });
            //ActiveConfig.connections.Add(new DAConnection { Name = "Connection", Id = ActiveConfig.connections.Max(x => x.Id) + 1 });
            //ActiveConfig.connections.Add(new DAConnection { Name = "Connection", Id = ActiveConfig.connections.Max(x => x.Id) + 1 });
            //ActiveConfig.connections.Add(new DAConnection { Name = "Connection", Id = ActiveConfig.connections.Max(x => x.Id) + 1 });
            //UpdateConfigService.Execute(new UpdateConfig(ActiveConfig));
        }

        public Config ActiveConfig 
        { 
            get { return activeConfig; }
            set {  
                activeConfig = value;
                OnPropertyChanged();
            }
        }
        private void _CreateConfigCommand() 
        {
            string fileName = StaticMethods.SaveFileDialog();
            CreateConfigService.Execute(new CreateConfig(fileName, ActiveConfig));
            StaticMethods.AddUpdateAppSettings("ActiveConfigFileName", ActiveConfig.fileName);
        }
        private ICommand _createConfigCommand;
        public ICommand CreateConfigCommand 
        {
            get
            {
                if (_createConfigCommand is null)
                    _createConfigCommand = new VoidCommandBase(_CreateConfigCommand, true) { };
                return _createConfigCommand;
            }
            set
            {
                _createConfigCommand = value;
            }
        }
        private ICommand _updateConfigCommand;
        public void _UpdateConfigCommand()
        {
            UpdateConfigService.Execute(new UpdateConfig(ActiveConfig));
        }
        public ICommand UpdateConfigCommand 
        {
            get 
            {
                if (_updateConfigCommand is null)
                {
                    _updateConfigCommand = new VoidCommandBase(_UpdateConfigCommand, true) { };
                }
                return _updateConfigCommand;
            }
            set 
            {
                _updateConfigCommand = value;
            }
        }
        private ICommand _loadConfigCommand;
        public void _LoadConfigCommand()
        {
            string fileName = StaticMethods.FileDialog("xml");
            var newConfig = this.queries.Execute(fileName);
            newConfig.Name = Path.GetFileName(fileName);
            ActiveConfig = newConfig;
        }
        public ICommand LoadConfigCommand
        {
            get
            {
                if (_loadConfigCommand is null)
                {
                    _loadConfigCommand = new VoidCommandBase(_LoadConfigCommand, true) { };
                }
                return _loadConfigCommand;
            }
            set { _loadConfigCommand = value; }
        }
    }
}
