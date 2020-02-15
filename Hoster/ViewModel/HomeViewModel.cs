using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Net;
using ZServer;
using ZServer.Parser;
using System.Windows;
using System.Windows.Threading;

namespace Hoster.ViewModel
{
   public class HomeViewModel:ViewModelBase,IDisposable
    {
        IPEndPoint end ;
        ZListener lsnr;
        private string _connectionStatus;
        private string _statusColor;
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
        public HomeViewModel()
        {
            end = new IPEndPoint(IPAddress.Parse("192.168.88.14"), 8000);
            lsnr = new ZListener(end);
            _listenerName = "Sysmex XN-550";
            lsnr.Accepted += ConnectionAccepted;
            lsnr.MessageReceived += MessageReceived;
            lsnr.Disconnected += ConnectionDisconnected;
            _connectionStatus = "Disconnected";
            _statusColor = "Red";
            _messages = new ObservableCollection<HosterMessage>();
            StartListener();
        }
        private RelayCommand _startEvent;
        public RelayCommand StartEvent
        {
            get
            {
                return this._startEvent ?? (this._startEvent = new RelayCommand(StartListener));
            }
        }

        public string ListenerName
        {
            get
            {
                return _listenerName;
            }

            set
            {
                _listenerName = value;
            }
        }

        public void StartListener()
        {
            try
            {
                lsnr.Start(3);
            }
            catch(Exception ex)
            {
                LogWriter.WriteErrorLog("Error in StartListener : "+ex.Message);
            }
       }

        private void ConnectionAccepted(object sender, ClientAcceptedEventArgs args)
        {
            _connectionStatus = "Connected to  : " + args.Socket.RemoteEndPoint;
            _statusColor = "Green";

            RaisePropertyChanged("ConnectionStatus");
            RaisePropertyChanged("StatusColor");
        }
        private void ConnectionDisconnected(object sender, DisconnectedEventArgs args)
        {
            _connectionStatus = "Disconnected " ;
            _statusColor = "Red";
            RaisePropertyChanged("ConnectionStatus");
            RaisePropertyChanged("StatusColor");
        }
        private void MessageReceived(object sender, MessageEventArgs args)
        {
          //  _messages.Add(args.Message);

            HosterMessage msg = new HosterMessage();
            msg.ID = "1";
            msg.SampleID = "1";
            msg.TestDesc = args.Message;
            //We should here parse the msgs 

            Application.Current.Dispatcher.BeginInvoke(
              DispatcherPriority.Background,
               new Action(() => {
               AddMessage(msg);
             }));
        }

        private void AddMessage(HosterMessage msg)
        {
            _messages.Add(msg);
            RaisePropertyChanged("Messages");

        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
