using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ZServer;
using static ZServer.DevicesProfiles;

namespace Hoster.ViewModel
{


    public    class ListenersViewModel:ViewModelBase
    {
        private IDataAccess _dataModel;
        const int TCPIP_CONNECTION = 1;
        const int RS232_CONNECTION = 2;
        private ObservableCollection<ListenerViewModel> _listeners;
        public ObservableCollection<ListenerViewModel> Listeners
        {
            get
            {
                return _listeners;
            }

            set
            {

                _listeners = value;
                RaisePropertyChanged();
            }
        }

        public ListenerViewModel CurrentListener
        {
            get
            {
                return _currentListener;
            }

            set
            {
                if (value != _currentListener)
                {
                    _currentListener = value;
                    RaisePropertyChanged();
                }
               
            }
        }

        private ListenerViewModel _currentListener;
        public ListenersViewModel(IDataAccess dataModel)
        {
           
            _dataModel = dataModel;
            _listeners = new ObservableCollection<ListenerViewModel>();
            foreach (DataRow dr in _dataModel.GetListeners().Rows) {

                ListenerSettingsViewModel _lsnrSetting = new ListenerSettingsViewModel(dataModel,Int16.Parse(dr["id"].ToString()));
               // ListenerViewModel _lsnr = new ListenerViewModel(_lsnrSetting.IPAddress, _lsnrSetting.Port, _lsnrSetting.Devices.Find(device => device.ID == _lsnrSetting.DeviceID).Name, _lsnrSetting);
                IListener listener;
                if (_lsnrSetting.ConnType == TCPIP_CONNECTION) {
                    listener = new ZListener(new System.Net.IPEndPoint(IPAddress.Parse(_lsnrSetting.IPAddress),_lsnrSetting.Port));
                }
                else {
                    Parity b;
                    int bauduRate = 19200;
                    StopBits stpBts = StopBits.One;
                    if (_lsnrSetting.Parity == 0)
                    {
                        b = Parity.None;
                    }
                    else if (_lsnrSetting.Parity == 1)
                    {

                        b = Parity.Odd;
                    }
                    else if (_lsnrSetting.Parity == 2)
                    {
                        b = Parity.Even;
                    }
                    else
                        b = Parity.None;

                    if (_lsnrSetting.BaudRate != 0)
                    {
                        bauduRate = _lsnrSetting.BaudRate;
                    }
                   

                    listener = new SerialListener(_lsnrSetting.COMPort,_lsnrSetting.ResponseLatency,bauduRate,_lsnrSetting.StopBits,b,_lsnrSetting.DeviceID);
                }
                
                ListenerViewModel _lsnr = new ListenerViewModel(_lsnrSetting.Devices.Find(device => device.ID == _lsnrSetting.DeviceID).Name,listener, _lsnrSetting);

                Listeners.Add(_lsnr);
            }
          
            if (_listeners.Count > 0) {
                CurrentListener = _listeners.First();

            }
        }


    }
}
