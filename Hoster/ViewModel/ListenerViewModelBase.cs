using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hoster.ViewModel
{
   public abstract class ListenerViewModelBase:ViewModelBase
    {
        #region properties 
        private string _statusColor;
        private string _connectionStatus;
    
        public string ConnectionStatus
        {
            get { return _connectionStatus; }
            set { _connectionStatus = value; }
        }
        public string StatusColor
        {
            get { return _statusColor; }
            set { _statusColor = value; }
        }

        private string _listenerName;
        private ObservableCollection<HosterMessage> _messages;
        public ObservableCollection<HosterMessage> Messages
        {
            get { return _messages; }
            set { _messages = value; }
        }
        public string ListenerName
        {
            get
            {
                return _listenerName;
            }

            set
            {
                if (value != _listenerName) { _listenerName = value; RaisePropertyChanged(); }

            }
        }

    
        #endregion

        public ListenerViewModelBase()
        {
            _messages = new ObservableCollection<HosterMessage>();

        }
    }
}
