using DARemoteViewer.Domain.Services;
using DARemoteViewer.Domain.Services.Static;
using DARemoteViewer.Domain.Models;
using System.Collections.ObjectModel;
using DARemoteViewer.Domain.Services.ConfigServices.QueryServices;
using System.IO;
using System.Windows.Input;
using DARemoteViewer.Domain.Services.ConfigServices.CommandServices;
using DARemoteViewer.Domain.Services.ConfigServices;
using DARemoteViewer.WPF.ViewModel.Commands;
using System.Windows.Navigation;
using System.Windows;
namespace DARemoteViewer.WPF.ViewModel
{
    public class MainWindowViewModel: ViewModelBase
    {

        

        #region Constructors
        public MainWindowViewModel(ObservableCollection<IConfigQuery<Config>> queries, ObservableCollection<IService<ConfigCommandBase>> services) //ObservableCollection<ICommandService<ICommandBase>> commandServices)
        {
            this.queries = queries;
            this.services = services;

            InitializeServices();

            string defaultConfigFileName = $"{Directory.GetCurrentDirectory()}\\Resources\\DefaultConfig.xml";

            var activeConfigFileName = StaticMethods.GetAppSettings("ActiveConfigFileName");
            Config startConfig;
            try
            {
                startConfig = LoadConfigService.Execute(activeConfigFileName);
            }
            catch (Exception)
            {
                MessageBox.Show("Not found active config file. Default config will be applied");
                startConfig = LoadConfigService.Execute(defaultConfigFileName);
            }
            ActiveConfig = startConfig;
        }

        #endregion

        #region Private fields
        private Config activeConfig;
        private DAConnection selectedConnection;
        private CreateConfigService CreateConfigService;
        private UpdateConfigService UpdateConfigService;
        private LoadConfigService LoadConfigService;
        private ICommand _createConfigCommand;
        private ICommand _updateConfigCommand;
        private ICommand _loadConfigCommand;
        private ICommand _addConnectionCommand;
        private ICommand _removeConnectionCommand;
        private ObservableCollection<IConfigQuery<Config>> queries;
        private ObservableCollection<IService<ConfigCommandBase>> services;
        #endregion

        #region Public properties
        public Config ActiveConfig
        {
            get { return activeConfig; }
            set
            {
                if (activeConfig is not null)
                {
                    if (activeConfig.fileName != value.fileName)
                    {
                        StaticMethods.AddUpdateAppSettings("ActiveConfigFileName", value.fileName);
                    }
                }
                activeConfig = value;
                
                OnPropertyChanged();
            }
        }
        public DAConnection SelectedConnection
        {
            get
            {
                return selectedConnection;
            }
            set
            {
                selectedConnection = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Commands
        private bool CreateConfigCommand_CanExecute()
        {
            return true;
        }
        private void CreateConfigCommand_Execute()
        {
            string fileName = StaticMethods.SaveFileDialog();
            CreateConfigService.Execute(new CreateConfig(fileName, ActiveConfig));
            ActiveConfig = LoadConfigService.Execute(fileName);

        }
        public ICommand CreateConfigCommand
        {
            get
            {
                if (_createConfigCommand is null)
                    _createConfigCommand = new VoidCommandBase(CreateConfigCommand_Execute, CreateConfigCommand_CanExecute) { };
                return _createConfigCommand;
            }
            set
            {
                _createConfigCommand = value;
            }
        }
        private bool UpdateConfigCommand_CanExecute()
        {
            return activeConfig.Name != "Default";
        }
        private void UpdateConfigCommand_Execute()
        {
            UpdateConfigService.Execute(new UpdateConfig(ActiveConfig));
        }
        public ICommand UpdateConfigCommand
        {
            get
            {
                if (_updateConfigCommand is null)
                {
                    _updateConfigCommand = new VoidCommandBase(UpdateConfigCommand_Execute, UpdateConfigCommand_CanExecute) { };
                }
                return _updateConfigCommand;
            }
            set
            {
                _updateConfigCommand = value;
            }
        }
        private bool LoadConfigCommand_CanExecute()
        {
            return true;
        }
        private void LoadConfigCommand_Execute()
        {
            string fileName = StaticMethods.FileDialog("xml");
            var newConfig = LoadConfigService.Execute(fileName);
            newConfig.Name = Path.GetFileName(fileName);
            ActiveConfig = newConfig;
        }

        public ICommand LoadConfigCommand
        {
            get
            {
                if (_loadConfigCommand is null)
                {
                    _loadConfigCommand = new VoidCommandBase(LoadConfigCommand_Execute, LoadConfigCommand_CanExecute) { };
                }
                return _loadConfigCommand;
            }
            set { _loadConfigCommand = value; }
        }
        private void AddConnectionCommand_Execute()
        {
            ActiveConfig.connections.Add(new DAConnection { Name = "Test Connection", hostName = "localhost" });
        }
        private bool AddConnectionCommand_CanExecute()
        {
            return true;
        }
        public ICommand AddConnectionCommand
        {
            get
            {
                if (_addConnectionCommand is null)
                {
                    _addConnectionCommand = new VoidCommandBase(AddConnectionCommand_Execute, AddConnectionCommand_CanExecute) { };
                }
                return _addConnectionCommand;
            }
            set { _addConnectionCommand = value; }
        }
        private void RemoveConnectionCommand_Execute()
        {
           ActiveConfig.connections.Remove(selectedConnection);
        }
        public ICommand RemoveConnectionCommand
        {
            get
            {
                if (_removeConnectionCommand is null)
                {
                    _removeConnectionCommand = new VoidCommandBase(RemoveConnectionCommand_Execute, RemoveConnection_CanExecute) { };
                }
                return _removeConnectionCommand;
            }
            set { _removeConnectionCommand = value; }
        }
        private bool RemoveConnection_CanExecute()
        {
            return ActiveConfig.connections.Count > 1;
        }
        #endregion

        #region Private methods
        private void InitializeServices()
        {
            if (queries is not null)
            {
                foreach (var query in this.queries)
                {
                    if (typeof(LoadConfigService) == query.GetType())
                    {
                        LoadConfigService = (LoadConfigService)query;
                    }
                    else { throw new Exception("Unknown Query"); }
                }
            }
            if (services is not null)
            {
                foreach (var service in this.services)
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
        }
        #endregion










    }
}
