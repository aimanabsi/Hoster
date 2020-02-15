using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hoster.Models.Settings;
namespace Hoster.ViewModel
{
    class SettingsViewModel:ViewModelBase
    {
        private bool _isLoading;
        private bool _startWithWindows;
        public bool StartWithWindows
        {
            get { return _startWithWindows; }
            set
            {
                if (value == _startWithWindows)
                    return;

                if (!_isLoading)
                    EnableDisableAutostart(value);

                _startWithWindows = value;
                RaisePropertyChanged();
            }
        }

        private bool _startMinimizedInTray;
        public bool StartMinimizedInTray
        {
            get { return _startMinimizedInTray; }
            set
            {
                if (value == _startMinimizedInTray)
                    return;

                if (!_isLoading)
                    SettingsManager.Current.Autostart_StartMinimizedInTray = value;

                _startMinimizedInTray = value;
                RaisePropertyChanged();
            }
        }
        public bool ConfiguringAutostart { get; set; }
        private async void EnableDisableAutostart(bool enable)
        {
            ConfiguringAutostart = true;

            try
            {
                if (enable)
                    await AutostartManager.EnableAsync();
                else
                    await AutostartManager.DisableAsync();

                // Show the user some awesome animation to indicate we are working on it :)
                await Task.Delay(2000);
            }
            catch (Exception ex)
            {
                
            }

            ConfiguringAutostart = false;
        }

        public SettingsViewModel()
        {
            _isLoading = true;
            LoadSettings();
            _isLoading = false;
        }
        private void LoadSettings()
        {
            StartWithWindows = AutostartManager.IsEnabled;
            StartMinimizedInTray = SettingsManager.Current.Autostart_StartMinimizedInTray;
        }
    }
}
