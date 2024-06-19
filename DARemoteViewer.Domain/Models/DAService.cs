using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceProcess;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace DARemoteViewer.Domain.Models
{
    public class DAService : DomainModel, INotifyPropertyChanged, ICloneable
    {
        private bool isControllerInitialized { get; set; }
        private bool isSelected = false;
        private bool exist = false;
        private string status = "Missing";
        private bool startEnable;
        private bool stopEnable;
        public bool IsSelected
        {
            get 
            { return isSelected; }
            set
            {
                isSelected = value;
                OnPropertyChanged();
            }
        }
        public bool Exist
        {
            get { return exist; }
            set { exist = value; OnPropertyChanged(); }
        }

        public string Status
        {
            get => status;
            set 
            { 
                status = value; 
                OnPropertyChanged();
                OnPropertyChanged("StartEnable");
                OnPropertyChanged("StopEnable");
            }
        }
        // public bool StartEnable { get;set; }
        public bool StartEnable 
        {
            get 
            {
                startEnable = status == ServiceControllerStatus.Stopped.ToString() || status ==ServiceControllerStatus.Paused.ToString();
                return startEnable;
            }
            set 
            {
                startEnable = value;
                OnPropertyChanged();
            }
        }
        public bool StopEnable
        {
            get
            {
                stopEnable = status == ServiceControllerStatus.Running.ToString() || status == ServiceControllerStatus.Paused.ToString();
                return stopEnable;
            }
            set
            {
                stopEnable = value;
                OnPropertyChanged();
            }
        }
        [XmlIgnore]
        public ServiceController Controller { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public object Clone()
        {
            return new DAService { Controller = this.Controller, Name = this.Name };
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            OnPropertyChangedExplicit(propertyName);
        }

        protected void OnPropertyChanged<TProperty>(Expression<Func<TProperty>> projection)
        {
            var memberExpression = (MemberExpression)projection.Body;
            OnPropertyChangedExplicit(memberExpression.Member.Name);
        }

        void OnPropertyChangedExplicit(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }

    }
}
