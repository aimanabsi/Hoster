using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;


namespace Hoster.Models.Settings
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class SettingsInfo : INotifyPropertyChanged
    {
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Variables
        [XmlIgnore] public bool SettingsChanged { get; set; }

        private string _settingsVersion = "0.0.0.0";
        public string SettingsVersion
        {
            get { return _settingsVersion; }
            set
            {
                if (value == _settingsVersion)
                    return;

                _settingsVersion = value;
                SettingsChanged = true;
            }
        }
        #endregion
        #region General 
        // General        
        private ApplicationViewManager.Name _general_DefaultApplicationViewName = ApplicationViewManager.Name.Listeners;
        public ApplicationViewManager.Name General_DefaultApplicationViewName
        {
            get { return _general_DefaultApplicationViewName; }
            set
            {
                if (value == _general_DefaultApplicationViewName)
                    return;

                _general_DefaultApplicationViewName = value;
                SettingsChanged = true;
            }
        }

        private int _general_HistoryListEntries = 5;
        public int General_HistoryListEntries
        {
            get {return _general_HistoryListEntries; }
            set
            {
                if (value == _general_HistoryListEntries)
                    return;

                _general_HistoryListEntries = value;
                SettingsChanged = true;
            }
        }

        private ObservableCollection<ApplicationViewInfo> _general_ApplicationList = new ObservableCollection<ApplicationViewInfo>();
        public ObservableCollection<ApplicationViewInfo> General_ApplicationList
        {
            get {return _general_ApplicationList; }
            set
            {
                if (value == _general_ApplicationList)
                    return;

                _general_ApplicationList = value;

                OnPropertyChanged();

                SettingsChanged = true;
            }
        }

        // Window
        private bool _window_ConfirmClose=true;
        public bool Window_ConfirmClose
        {
            get {return _window_ConfirmClose; }
            set
            {
                if (value == _window_ConfirmClose)
                    return;

                _window_ConfirmClose = value;
                SettingsChanged = true;
            }
        }

        private bool _window_MinimizeInsteadOfTerminating=true;
        public bool Window_MinimizeInsteadOfTerminating
        {
            get {return _window_MinimizeInsteadOfTerminating; }
            set
            {
                if (value == _window_MinimizeInsteadOfTerminating)
                    return;

                _window_MinimizeInsteadOfTerminating = value;
                SettingsChanged = true;
            }
        }

        private bool _window_MultipleInstances=false;
        public bool Window_MultipleInstances
        {
            get {return _window_MultipleInstances; }
            set
            {
                if (value == _window_MultipleInstances)
                    return;

                _window_MultipleInstances = value;
                SettingsChanged = true;
            }
        }

        private bool _window_MinimizeToTrayInsteadOfTaskbar=true;
        public bool Window_MinimizeToTrayInsteadOfTaskbar
        {
            get {return _window_MinimizeToTrayInsteadOfTaskbar; }
            set
            {
                if (value == _window_MinimizeToTrayInsteadOfTaskbar)
                    return;

                _window_MinimizeToTrayInsteadOfTaskbar = value;
                SettingsChanged = true;
            }
        }

        private bool _window_ShowCurrentApplicationTitle;
        public bool Window_ShowCurrentApplicationTitle
        {
            get {return _window_ShowCurrentApplicationTitle; }
            set
            {
                if (value == _window_ShowCurrentApplicationTitle)
                    return;

                _window_ShowCurrentApplicationTitle = value;

                OnPropertyChanged();

                SettingsChanged = true;
            }
        }

        // TrayIcon
        private bool _trayIcon_AlwaysShowIcon;
        public bool TrayIcon_AlwaysShowIcon
        {
            get {return _trayIcon_AlwaysShowIcon; }
            set
            {
                if (value == _trayIcon_AlwaysShowIcon)
                    return;

                _trayIcon_AlwaysShowIcon = value;
                SettingsChanged = true;
            }
        }

        // Appearance
        private string _appearance_AppTheme;
        public string Appearance_AppTheme
        {
            get {return _appearance_AppTheme; }
            set
            {
                if (value == _appearance_AppTheme)
                    return;

                _appearance_AppTheme = value;
                SettingsChanged = true;
            }
        }

        private string _appearance_Accent;
        public string Appearance_Accent
        {
            get {return _appearance_Accent; }
            set
            {
                if (value == _appearance_Accent)
                    return;

                _appearance_Accent = value;
                SettingsChanged = true;
            }
        }

        private bool _appearance_EnableTransparency;
        public bool Appearance_EnableTransparency
        {
            get {return _appearance_EnableTransparency; }
            set
            {
                if (value == _appearance_EnableTransparency)
                    return;

                _appearance_EnableTransparency = value;
                SettingsChanged = true;
            }
        }

        private double _appearance_Opacity = 0.85;
        public double Appearance_Opacity
        {
            get {return _appearance_Opacity; }
            set
            {
                if (value == _appearance_Opacity)
                    return;

                _appearance_Opacity = value;
                SettingsChanged = true;
            }
        }

        // Localization
        private string _localization_CultureCode;
        public string Localization_CultureCode
        {
            get {return _localization_CultureCode; }
            set
            {
                if (value == _localization_CultureCode)
                    return;

                _localization_CultureCode = value;
                SettingsChanged = true;
            }
        }

        // Autostart
        private bool _autostart_StartMinimizedInTray=true;
        public bool Autostart_StartMinimizedInTray
        {
            get {return _autostart_StartMinimizedInTray; }
            set
            {
                if (value == _autostart_StartMinimizedInTray)
                    return;

                _autostart_StartMinimizedInTray = value;
                SettingsChanged = true;
            }
        }

        // HotKey
        private bool _hotKey_ShowWindowEnabled;
        public bool HotKey_ShowWindowEnabled
        {
            get {return _hotKey_ShowWindowEnabled; }
            set
            {
                if (value == _hotKey_ShowWindowEnabled)
                    return;

                _hotKey_ShowWindowEnabled = value;
                SettingsChanged = true;
            }
        }

        private int _hotKey_ShowWindowKey = 79;
        public int HotKey_ShowWindowKey
        {
            get {return _hotKey_ShowWindowKey; }
            set
            {
                if (value == _hotKey_ShowWindowKey)
                    return;

                _hotKey_ShowWindowKey = value;
                SettingsChanged = true;
            }
        }

        private int _hotKey_ShowWindowModifier = 3;
        public int HotKey_ShowWindowModifier
        {
            get {return _hotKey_ShowWindowModifier; }
            set
            {
                if (value == _hotKey_ShowWindowModifier)
                    return;

                _hotKey_ShowWindowModifier = value;
                SettingsChanged = true;
            }
        }

        // Update
        private bool _update_CheckForUpdatesAtStartup = true;
        public bool Update_CheckForUpdatesAtStartup
        {
            get {return _update_CheckForUpdatesAtStartup; }
            set
            {
                if (value == _update_CheckForUpdatesAtStartup)
                    return;

                _update_CheckForUpdatesAtStartup = value;
                SettingsChanged = true;
            }
        }
        #endregion

        #region Others
        // Application view       
        private bool _expandApplicationView;
        public bool ExpandApplicationView
        {
            get {return _expandApplicationView; }
            set
            {
                if (value == _expandApplicationView)
                    return;

                _expandApplicationView = value;
                SettingsChanged = true;
            }
        }
        #endregion

 
    }
}