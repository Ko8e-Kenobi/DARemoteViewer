using DARemoteViewer.Domain.Models;
using DARemoteViewer.Domain.Services;
using DARemoteViewer.WPF.ViewModel.Commands;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Printing;
using System.ServiceProcess;
using System.Text;
using System.Windows;
using System.Windows.Input;

using static System.Formats.Asn1.AsnWriter;

namespace DARemoteViewer.WPF.ViewModel
{
    public class AddConnectionPopUpViewModel : ViewModelBase
    {
        public DAConnection Connection { get; set; }
        public bool IsConfirmed { get; set; }
        public bool IsCanceled { get; set; }


        public Action CloseAction { get; set; }

        private void CloseCommandFunction()
        {
            CloseAction();
        }

        private string status;
        private string filter = "";
        private ICommand _okCommand;
        private ICommand _cancelCommand;
        private ICommand _testConnectionCommand;
        private ICommand _getServicesCommand;
        private ICommand _addServiceCommand;
        private ICommand _removeServiceCommand;
        private ObservableCollection<DAService> foundServices = new ObservableCollection<DAService>();
        private ObservableCollection<DAService> savedServices = new ObservableCollection<DAService>();
        private DAService selectedFoundService;
        private DAService selectedSavedService;

        private bool OkCommand_CanExecute()
        {
            return true;
        }
        private bool CancelCommand_CanExecute()
        {
            return true;
        }
        private bool TestConnectionCommand_CanExecute()
        {
            return true;
        }
        private bool GetServicesCommand_CanExecute()
        {
            return true;
        }
        private bool AddServiceCommand_CanExecute()
        {
            return true;
        }
        private bool RemoveServiceCommand_CanExecute()
        {
            return true;
        }
        

        public ICommand OkCommand 
        {
            get
            {
                if (_okCommand is null)
                {
                    _okCommand = new VoidCommandBase(OkCommand_Execute, OkCommand_CanExecute) { };
                }
                return _okCommand;
            }
            set { _okCommand = value; }
        }
        public ICommand CancelCommand
        {
            get
            {
                if (_cancelCommand is null)
                {
                    _cancelCommand = new VoidCommandBase(CancelCommand_Execute, CancelCommand_CanExecute) { };
                }
                return _cancelCommand;
            }
            set { _cancelCommand = value; }
        }
        public ICommand TestConnectionCommand
        {
            get
            {
                if (_testConnectionCommand is null)
                {
                    _testConnectionCommand = new VoidCommandBase(TestConnectionCommand_Execute, TestConnectionCommand_CanExecute) { };
                }
                return _testConnectionCommand;
            }
            set { _testConnectionCommand = value; }
        }
        public ICommand GetServicesCommand
        {
            get
            {
                if (_getServicesCommand is null)
                {
                    _getServicesCommand = new VoidCommandBase(GetServicesCommand_Execute, GetServicesCommand_CanExecute) { };
                }
                return _getServicesCommand;
            }
            set { _getServicesCommand = value; }
        }
        public ICommand AddServiceCommand 
        {
            get
            {
                if (_addServiceCommand is null)
                {
                    _addServiceCommand = new VoidCommandBase(AddServiceCommand_Execute, AddServiceCommand_CanExecute) { };
                }
                return _addServiceCommand;
            }
            set { _addServiceCommand = value; }
        }

        public ICommand RemoveServiceCommand
        {
            get
            {
                if (_removeServiceCommand is null)
                {
                    _removeServiceCommand = new VoidCommandBase(RemoveServiceCommand_Execute, RemoveServiceCommand_CanExecute) { };
                }
                return _removeServiceCommand;
            }
            set { _removeServiceCommand = value; }
        }

        public string Status
        {
            get { return status; }
            set
            {
                status = value;
                OnPropertyChanged();
            }
        }
        public string Filter
        {
            get { return filter; }
            set
            {
                filter = value;
                OnPropertyChanged();
            }
        }
        public DAService SelectedFoundService 
        {
            get 
            {
                return selectedFoundService; 
            }
            set
            {
                selectedFoundService = value;
                //selectedFoundService.IsSelected = true;
                //AddService();
                OnPropertyChanged();
            }
        }


        public ObservableCollection<DAService> FoundServices 
        {
            get { return foundServices; }
            set 
            {
                foundServices = value;
                OnPropertyChanged();
            }

        }
        public DAService SelectedSavedService
        {
            get
            {
                return selectedSavedService;
            }
            set
            {
                selectedSavedService = value;
                //selectedSavedService.IsSelected = false;
                //RemoveService();
                OnPropertyChanged();
            }
        }
        public ObservableCollection<DAService> SavedServices
        {
            get { return savedServices; }
            set
            {
                savedServices = value;
                OnPropertyChanged();
            }

        }
        private void CancelCommand_Execute()
        {
            IsCanceled = true;
            CloseCommandFunction();
        }

        private void OkCommand_Execute()
        {
            IsConfirmed = true;
            CloseCommandFunction();
        }
        private void TestConnectionCommand_Execute()
        {
            // Ping's the local machine.
            Ping pingSender = new Ping();
            IPAddress address; 
            bool ipCorrect = IPAddress.TryParse(Connection.IPAddress, out address);

            // Create a buffer of 32 bytes of data to be transmitted.
            string data = "ping";
            byte[] buffer = Encoding.ASCII.GetBytes(data);

            // Wait 10 seconds for a reply.
            int timeout = 10000;

            // Set options for transmission:
            // The data can go through 64 gateways or routers
            // before it is destroyed, and the data packet
            // cannot be fragmented.
            PingOptions options = new PingOptions(64, true);
            PingReply reply;
            if (ipCorrect)
            {
                reply = pingSender.Send(address, timeout, buffer, options);
                if (reply.Status == IPStatus.Success)
                {
                    //Console.WriteLine("Address: {0}", reply.Address.ToString());
                    //Console.WriteLine("RoundTrip time: {0}", reply.RoundtripTime);
                    //Console.WriteLine("Time to live: {0}", reply.Options.Ttl);
                    //Console.WriteLine("Don't fragment: {0}", reply.Options.DontFragment);
                    //Console.WriteLine("Buffer size: {0}", reply.Buffer.Length);
                    Status = "Connected";
                }
                else
                {
                    Status = "No connection";
                    Console.WriteLine(reply.Status);
                }
            }
            //ServiceManagerWithAuthentification();
        }

        private void GetServicesCommand_Execute()
        {
            
            ServiceController[] scs = ServiceController.GetServices(Connection.IPAddress);
            scs = scs.Where(service => service.ServiceName.Contains(filter.Trim(),StringComparison.OrdinalIgnoreCase)).ToArray();
            if (FoundServices is not null)
            {
                FoundServices.Clear();
            }
            
            foreach (ServiceController service in scs)
            {
                FoundServices.Add(new DAService { Name = service.ServiceName, Controller = service });

                //if (service.ServiceName == "DA$LABELLQ31")
                //{
                //    //service.Start();
                //    MessageBox.Show(service.Status.ToString());
                //}
            }
        }

        private void AddServiceCommand_Execute()
        {
            AddService();
        }

        private void RemoveServiceCommand_Execute()
        {
            RemoveService();
        }
        
        private void AddService() 
        {
            foreach (var item in FoundServices.Where(x=>x.IsSelected))
            {
                if (item is not null)
                {
                    if (!SavedServices.Any(i=> i.Name == item.Name))
                    {
                        item.IsSelected = false;
                        SavedServices.Add(item);

                    }
                }
            }
            OnPropertyChanged(() => FoundServices);
        }

        private void RemoveService()
        {
            for (int i = SavedServices.Count() - 1; i >= 0; i--)
            {
                if (SavedServices[i].IsSelected) 
                    SavedServices.RemoveAt(i);
            }
        }
        /// <summary>
        /// Services logic with authentification backup to implement in case issues with authorization
        /// </summary>
        private void ServiceManagerWithAuthentification()
        {
            ConnectionOptions options = new ConnectionOptions();
            options.Username = "User";
            options.Password = "Password";
            ManagementScope scope = new ManagementScope("\\\\10.71.27.49\\root\\cimv2", options);
            scope.Connect();

            //Query system for Operating System information
            ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_Service ");//"SELECT * FROM Win32_Service WHERE Name = 'DA$LABELLR51'"
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);

            ManagementObjectCollection queryCollection = searcher.Get();

            foreach (ManagementObject oReturn in queryCollection)
            {
                // invoke start
                //ManagementBaseObject outParams = oReturn.InvokeMethod("StartService", null, null);

                //invoke stop
                //ManagementBaseObject outParams2 = oReturn.InvokeMethod("StopService", null, null);

                //get result
                //string a = outParams["ReturnValue"].ToString();

                // get service state
                //string state = oReturn.Properties["State"].Value.ToString().Trim();

                //MessageBox.Show(state);// TO DISPLAY STATOS FROM SERVICE IN REMOTE COMPUTER
            }



        }
    }
}
