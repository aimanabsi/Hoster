using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZPublisher;
using ZServer;

namespace Hoster.ViewModel
{

    public   class SerialListenerViewModel:ListenerViewModelBase
    {
        SerialListener serialListener;
        private ListenerSettingsViewModel _listenerSettingsViewModel;
        IPublisher _publisher;

        public SerialListenerViewModel(ListenerSettingsViewModel lsnrSettings) {
            _listenerSettingsViewModel = lsnrSettings;
            _publisher = PublisherFactory.CreatePublisher(_listenerSettingsViewModel.LisDBType, _listenerSettingsViewModel.LisDBHostname, _listenerSettingsViewModel.LisDBPort, _listenerSettingsViewModel.LisDBName, _listenerSettingsViewModel.LisDBUsername, _listenerSettingsViewModel.LisDBPassword, _listenerSettingsViewModel.LisDBName);
            serialListener = new SerialListener(lsnrSettings.COMPort,lsnrSettings.ResponseLatency,lsnrSettings.BaudRate,System.IO.Ports.StopBits.One,System.IO.Ports.Parity.Even,_listenerSettingsViewModel.DeviceID);
            serialListener.ZMessageParsed += ZMessageParsed;
            StartListener();

        }


        public void StartListener()
        {
            try
            {
                serialListener.Start();
            }
            catch (Exception ex)
            {
                LogWriter.WriteErrorLog("Error in StartListener : " + ex.Message);
            }
        }

        private void ZMessageParsed(object sender, ZMessageEventArgs args) { 

        }

    }
}
