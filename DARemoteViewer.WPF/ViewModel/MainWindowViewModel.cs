using DARemoteViewer.Domain.Services;
using DARemoteViewer.Domain.Services.Static;
using DARemoteViewer.Domain.Models;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using DARemoteViewer.WPF.ViewModel.Commands;
using System.Windows.Navigation;
using System.Windows;
using System.Windows.Controls.Primitives;
using DARemoteViewer.WPF.Views;
using DARemoteViewer.Domain.Services.Commands;
using DARemoteViewer.Domain.Services.Commands.ConfigCommands;
using DARemoteViewer.Domain.Services.Queries.ConfigQueries;
using System.Windows.Media.Animation;
using System.ServiceProcess;
using static System.Net.WebRequestMethods;
using System.Xml.Linq;
namespace DARemoteViewer.WPF.ViewModel
{
    public class MainWindowViewModel: ViewModelBase
    {
        #region Constructors
        public MainWindowViewModel(ObservableCollection<IQuery<Config, string>> queries, ObservableCollection<IService<CommandBase>> services) 
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
                CheckServiceStatusTask();
            }
            catch (Exception)
            {
                MessageBox.Show("Not found active config file. Default config will be applied");
                CreateConfigService.Execute(new CreateConfig(defaultConfigFileName, 
                    new Config
                    {
                        Name = "Default",
                        connections = new ObservableCollection<DAConnection>
                        {
                            new DAConnection
                            {
                                Name="Default"
                            }
                        }
                    }));
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
        private ICommand _editConnectionCommand;
        private ICommand _addConnectionCommand;
        private ICommand _removeConnectionCommand;
        private VoidCommandBase _startServiceCommand;
        private bool connectionIsSelected = false;
        private ObservableCollection<IQuery<Config,string>> queries;
        private ObservableCollection<IService<CommandBase>> services;
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
                connectionIsSelected = true;
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

        Window PopUpAddConnection;
        private bool toClosePopUp = false;
        public bool ToClosePopUp
        {
            get
            {
                return toClosePopUp;
            }
            set
            {
                toClosePopUp = value;
                this.PopUpAddConnection.Close();
            }
        }

        private void EditConnectionCommand_Execute()
        {
            AddConnectionPopUpViewModel editConnection = new AddConnectionPopUpViewModel();
            editConnection.Connection = SelectedConnection;
            editConnection.IsConfirmed = ToClosePopUp;
            PopUpAddConnection = new AddConnectionPopUp(editConnection);
            PopUpAddConnection.Owner = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
            editConnection.SavedServices = SelectedConnection.Services;
            PopUpAddConnection.ShowDialog();

            
            if (editConnection.IsConfirmed)
            {
                //ActiveConfig.connections.Add(addConnection.Connection);
            }
            else if (editConnection.IsCanceled)
            {
                MessageBox.Show("Canceled");
            }

            PopUpAddConnection.Close();

        }
        private bool EditConnectionCommand_CanExecute()
        {
            return connectionIsSelected;
        }
        public ICommand EditConnectionCommand
        {
            get
            {
                if (_editConnectionCommand is null)
                {
                    _editConnectionCommand = new VoidCommandBase(EditConnectionCommand_Execute, EditConnectionCommand_CanExecute) { };
                }
                return _editConnectionCommand;
            }
            set { _editConnectionCommand = value; }
        }

        private void AddConnectionCommand_Execute()
        {
            AddConnectionPopUpViewModel addConnection = new AddConnectionPopUpViewModel();
            addConnection.Connection = new DAConnection();
            //addConnection.Connection.User = new DAUser();
            addConnection.Connection.Name = "MyConnection";
            addConnection.Connection.IPAddress = "localhost";
            addConnection.IsConfirmed = ToClosePopUp;
            PopUpAddConnection = new AddConnectionPopUp(addConnection);
            PopUpAddConnection.Owner = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
            PopUpAddConnection.ShowDialog();

            if (addConnection.IsConfirmed)
            {
                ActiveConfig.connections.Add(addConnection.Connection);
            }
            else if (addConnection.IsCanceled)
            {
                MessageBox.Show("Canceled");
            }
            PopUpAddConnection.Close();

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
        public ICommand StartServiceCommand
        {
            get
            {
                if (_startServiceCommand is null)
                {
                    _startServiceCommand = new VoidCommandBase(obj =>
                    {
                        if (obj != null)
                        {
                            string serviceName = obj as string;
                            StartServiceCommand_Execute(serviceName);
                        }
                    });
                }
                return _startServiceCommand;
            }
           // set { _startServiceCommand = value; }
        }
        private void StartServiceCommand_Execute(string serviceName)
        {
            var service = SelectedConnection.Services.FirstOrDefault(x => x.Name == serviceName);
            service.Controller.Start();
            //service.StartEnable = false;
        }
        //private bool StartServiceCommand_CanExecute()
        //{
        //    //return SelectedConnection.Services.FirstOrDefault(x => x.Name == serviceName).Controller.Status == ServiceControllerStatus.Stopped;
        //    return true;
        //}
        #endregion
        private async void CheckServiceStatusTask()
        {
            while (true)
            {
                CheckServicesStatus();
                await Task.Delay(1000);
            }

        }
        private void CheckServicesStatus()
        {
            if (SelectedConnection is not null)
            {
                if (connectionIsSelected)
                {
                    foreach (var item in SelectedConnection.Services)
                    {
                        if (item.Controller is null)
                        {
                            var tempController = ServiceController.GetServices(selectedConnection?.IPAddress).FirstOrDefault(x => x.ServiceName == item.Name);
                            if (tempController is not null)
                            {
                                item.Controller = tempController;
                                //item.Exist = true;
                            }
                            else { continue; }
                        }
                        //if (item.Exist)
                        //{
                        item.Controller.Refresh();
                        item.Status = item.Controller.Status.ToString();
                        //}
                        //item.StartEnable = item.Status == ServiceControllerStatus.Stopped.ToString() || item.Status == ServiceControllerStatus.Paused.ToString();

                        // StartServiceCommand_CanExecute(item.Name);
                    }
                }
            }
        }
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
