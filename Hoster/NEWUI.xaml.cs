using GalaSoft.MvvmLight.Command;
using Hoster.Models.Settings;
using Hoster.Utilities;
using Hoster.Views;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Hoster
{
    /// <summary>
    /// Interaction logic for NEWUI.xaml
    /// </summary>
    /// 

    public partial class NEWUI : INotifyPropertyChanged
    {
        #region PropertyChangedEventHandler
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        private SettingsUI _settingsView;
        private HomeUI _homeUIView;
        private Object _mainUI;
        private DashboardView _dashboardView;
        private NotifyIcon _notifyIcon;
        private readonly bool _isLoading;
        private ApplicationViewManager.Name _filterLastViewName;
        private int? _filterLastCount;
        private string _search = string.Empty;
        public string Search
        {
            get { return _search; }
            set
            {
                if (value == _search)
                    return;

                _search = value;

                if (SelectedApplication != null)
                    _filterLastViewName = SelectedApplication.Name;

                Applications.Refresh();

                var sourceCollection = Applications.SourceCollection.Cast<ApplicationViewInfo>();
                var filteredCollection = Applications.Cast<ApplicationViewInfo>();

                var sourceInfos = sourceCollection as ApplicationViewInfo[] ?? sourceCollection.ToArray();
                var filteredInfos = filteredCollection as ApplicationViewInfo[] ?? filteredCollection.ToArray();

                if (_filterLastCount == null)
                    _filterLastCount = sourceInfos.Length;

                SelectedApplication = _filterLastCount > filteredInfos.Length ? filteredInfos.FirstOrDefault() : sourceInfos.FirstOrDefault(x => x.Name == _filterLastViewName);

                _filterLastCount = filteredInfos.Length;

                // Show note when there was nothing found
                SearchNothingFound = filteredInfos.Length == 0;

                OnPropertyChanged();
            }
            }

        private bool _searchNothingFound;
        public bool SearchNothingFound
        {
            get { return _searchNothingFound; }
            set
            {
                if (value == _searchNothingFound)
                    return;

                _searchNothingFound = value;
                OnPropertyChanged();
            }
            }
        public bool ShowCurrentApplicationTitle => SettingsManager.Current.Window_ShowCurrentApplicationTitle;

        private bool _isInTray;
        private bool _closeApplication;
        private bool _expandApplicationView;
        public bool ExpandApplicationView
        {
            get { return _expandApplicationView; }
            set
            {
                if (value == _expandApplicationView)
                    return;

                if (!_isLoading)
                    SettingsManager.Current.ExpandApplicationView = value;

                if (!value)
                    ClearSearchOnApplicationListMinimize();

                _expandApplicationView = value;
                OnPropertyChanged();
            }
            }

        private bool _isTextBoxSearchFocused;
        public bool IsTextBoxSearchFocused
        {
            get { return _isTextBoxSearchFocused; }
            set
            {
                if (value == _isTextBoxSearchFocused)
                    return;

                if (!value)
                    ClearSearchOnApplicationListMinimize();

                _isTextBoxSearchFocused = value;
                OnPropertyChanged();
            }
            }

       
        private bool _isApplicationListOpen;

        public bool IsApplicationListOpen
        {
            get { return _isApplicationListOpen; }
            set
            {
                if (value == _isApplicationListOpen)
                    return;

                if (!value)
                    ClearSearchOnApplicationListMinimize();

                _isApplicationListOpen = value;
                OnPropertyChanged();
            }
            }
        private bool _updateAvailable;
        public bool UpdateAvailable
        {
            get { return _updateAvailable; }
            set
            {
                if (value == _updateAvailable)
                    return;

                _updateAvailable = value;
                OnPropertyChanged();
            }
            }

        private string _updateText;
        public string UpdateText
        {
            get { return _updateText; }
            set
            {
                if (value == _updateText)
                    return;

                _updateText = value;
                OnPropertyChanged();
            }
            }
        public ICollectionView Applications { get; private set; }
        private bool _isMouseOverApplicationList;
        public bool IsMouseOverApplicationList
        {
            get { return _isMouseOverApplicationList; }
            set
            {
                if (value == _isMouseOverApplicationList)
                    return;

                if (!value)
                    ClearSearchOnApplicationListMinimize();

                _isMouseOverApplicationList = value;
                OnPropertyChanged();
            }
            }

        private ApplicationViewInfo _selectedApplication;
        public ApplicationViewInfo SelectedApplication
        {
            get { return _selectedApplication; }
            set
            {
                if (value == _selectedApplication)
                    return;

                if (value != null)
                    ChangeApplicationView(value.Name);

                _selectedApplication = value;
                OnPropertyChanged();
            }
            }
        public NEWUI()
        {
            _isLoading = true;
            InitializeComponent();
            DataContext = this;
            ConfigurationManager.Detect();
            SettingsManager.Load();
            AppearanceManager.Load();

            SettingsManager.Current.PropertyChanged += SettingsManager_PropertyChanged;
            EventSystem.RedirectToApplicationEvent += EventSystem_RedirectToApplicationEvent;
            EventSystem.RedirectToSettingsEvent += EventSystem_RedirectToSettingsEvent;

            LoadApplicationList();
            _isLoading = false;
        }


        protected override async void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            if (_mainUI == null)
                _mainUI = new MainContentUI();
           
        }
        private void SettingsManager_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //if (e.PropertyName == nameof(SettingsInfo.Window_ShowCurrentApplicationTitle))
            //    OnPropertyChanged(nameof(true));
        }

        private void LoadApplicationList()
        {
            //// Need to add items here... if in SettingsInfo/Constructor --> same item will appear multiple times...
            if (SettingsManager.Current.General_ApplicationList.Count == 0)
                SettingsManager.Current.General_ApplicationList = new ObservableCollection<ApplicationViewInfo>(ApplicationViewManager.GetList());

            Applications = new CollectionViewSource { Source = SettingsManager.Current.General_ApplicationList }.View;

            Applications.SortDescriptions.Add(new SortDescription(nameof(ApplicationViewInfo.Name), ListSortDirection.Ascending)); // Always have the same order, even if it is translated...
            //Applications.Filter = o =>
            //{
            //    //if (!(o is ApplicationViewInfo info))
            //    //    return false;

            //    if (string.IsNullOrEmpty(Search))
            //        return info.IsVisible;

            //    var regex = new Regex(@" |-");

            //    var search = regex.Replace(Search, "");

            //    // Search by TranslatedName and Name
            //    return info.IsVisible && (regex.Replace(ApplicationViewManager.GetTranslatedNameByName(info.Name), "").IndexOf(search, StringComparison.OrdinalIgnoreCase) > -1 || regex.Replace(info.Name.ToString(), "").IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0);
            //};

            SettingsManager.Current.General_ApplicationList.CollectionChanged += (sender, args) => Applications.Refresh();

            // Get application from settings
            SelectedApplication = Applications.SourceCollection.Cast<ApplicationViewInfo>().FirstOrDefault(x => x.Name == SettingsManager.Current.General_DefaultApplicationViewName);

            // Scroll into view
            if (SelectedApplication != null)
                ListViewApplication.ScrollIntoView(SelectedApplication);
        }

        private async void MetroWindowMain_Closing(object sender, CancelEventArgs e)
        {
            // Force restart (if user has reset the settings or import them)
            if (SettingsManager.ForceRestart )
            {
                RestartApplication(false);

                _closeApplication = true;
            }

            // Hide the application to tray
            if (!_closeApplication && (SettingsManager.Current.Window_MinimizeInsteadOfTerminating && WindowState != WindowState.Minimized))
            {
                e.Cancel = true;

                WindowState = WindowState.Minimized;

                return;
            }

            // Confirm close
            if (!_closeApplication && SettingsManager.Current.Window_ConfirmClose)
            {
                e.Cancel = true;

                // If the window is minimized, bring it to front
                if (WindowState == WindowState.Minimized)
                    BringWindowToFront();

                var settings = AppearanceManager.MetroDialog;

              

                // Fix airspace issues
                ConfigurationManager.Current.IsDialogOpen = true;

                var result = await this.ShowMessageAsync("Confirm", "Are you sure you do want to exit ?", MessageDialogStyle.AffirmativeAndNegative, settings);

                ConfigurationManager.Current.IsDialogOpen = false;

                if (result != MessageDialogResult.Affirmative)
                    return;

                _closeApplication = true;
                Close();

                return;
            }

            // Unregister HotKeys
            //if (_registeredHotKeys.Count > 0)
            //    UnregisterHotKeys();

            // Dispose the notify icon to prevent errors
            _notifyIcon?.Dispose();
        }

      
     

        private ApplicationViewManager.Name? _currentApplicationViewName;

        private void ChangeApplicationView(ApplicationViewManager.Name name)
        {
            if (_currentApplicationViewName == name)
                return;

            Console.WriteLine("The _currentApplicationViewName is  : "+name);
            switch (name)
            {
                case ApplicationViewManager.Name.Listeners:
                    if (_mainUI == null)
                        _mainUI = new MainContentUI();
                    else
                        RefreshApplicationView(name);

                    //  ContentControlApplication.Content = _homeUIView;
                    ContentControlApplication.Content = _mainUI;
                    break;
                case ApplicationViewManager.Name.None:
                    break;
                case ApplicationViewManager.Name.Dashboard:
                    if (_dashboardView == null)
                        _dashboardView = new DashboardView();
                    ContentControlApplication.Content = _dashboardView ;
                    break;
                default:
                    break;
                  //  throw new ArgumentOutOfRangeException(nameof(name), name, null);
            }

            _currentApplicationViewName = name;
        }

        private void RefreshApplicationView(ApplicationViewManager.Name name)
        {
            switch (name)
            {

                case ApplicationViewManager.Name.Listeners:
                 //   _homeUIView.Refresh();
                    break;
              
              
                case ApplicationViewManager.Name.None:
                    break;
                default:
                    break;
            }
        }

        private void ClearSearchOnApplicationListMinimize()
        {
           
        }

        private void EventSystem_RedirectToApplicationEvent(object sender, EventArgs e)
        {
            EventSystemRedirectApplicationArgs args = (EventSystemRedirectApplicationArgs)e;
            if (!(e is EventSystemRedirectApplicationArgs ))
                return;

            // Change view
            SelectedApplication = Applications.SourceCollection.Cast<ApplicationViewInfo>().FirstOrDefault(x => x.Name == args.Application);

            // Crate a new tab / perform action
            switch (args.Application)
            {
               
                case ApplicationViewManager.Name.Listeners:
                    break;
                case ApplicationViewManager.Name.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #region Settings
     

        private void EventSystem_RedirectToSettingsEvent(object sender, EventArgs e)
        {
            OpenSettings();
        }

        private async void CloseSettings()
        {
            ShowSettingsView = false;

            // Enable/disable tray icon
            if (!_isInTray)
            {
                if (true && _notifyIcon == null)
                    InitNotifyIcon();

                if (_notifyIcon != null)
                    _notifyIcon.Visible = true;

                MetroWindowMain.HideOverlay();
            }



            // Ask the user to restart (if he has changed the language)
           

            // Change the transparency
            //if (AllowsTransparency != SettingsManager.Current.Appearance_EnableTransparency || (Opacity != SettingsManager.Current.Appearance_Opacity))
            //{
            //    if (!AllowsTransparency || !SettingsManager.Current.Appearance_EnableTransparency)
            //        Opacity = 1;
            //    else
            //        Opacity = SettingsManager.Current.Appearance_Opacity;
            //}

            // Change HotKeys
            if (SettingsManager.HotKeysChanged)
            {
           //     UnregisterHotKeys();
           //     RegisterHotKeys();

                SettingsManager.HotKeysChanged = false;
            }

            // Save the settings
            //if (SettingsManager.Current.SettingsChanged)
            //    SettingsManager.Save();

            // Refresh the view
        //    RefreshApplicationView(SelectedApplication.Name);
        }
        private bool _showSettingsView;
        public bool ShowSettingsView
        {
            get { return _showSettingsView; }
            set
            {


                if (value == _showSettingsView)
                    return;

                _showSettingsView = value;
                OnPropertyChanged();
            }
            }

        private void OpenSettings()
        {

            // Init settings view
            if (_settingsView == null)
            {
                _settingsView = new SettingsUI();
                ContentControlSettings.Content = _settingsView;
            }
            else // Change view
            {
              
            //   _settingsView.Refresh();
            }

            // Show the view (this will hide other content)
            ShowSettingsView = true;

            // Bring window to front
            ShowWindowAction();
        }



        private void CloseApplicationAction()
        {
           System.Windows. MessageBox.Show("CLosing the window : ");
            _closeApplication = true;
            Close();
        }

        private void RestartApplication(bool closeApplication = true)
        {
            new Process
            {
                StartInfo =
                {
                    FileName = ConfigurationManager.Current.ApplicationFullName,
                    Arguments = $"--restart-pid:{Process.GetCurrentProcess().Id}"
                }
            }.Start();

            if (!closeApplication)
                return;

            _closeApplication = true;
            Close();
        }

        public ICommand ApplicationListMouseEnterCommand
        {
            get { return new RelayCommand(ApplicationListMouseEnterAction); }
        }

        private void ApplicationListMouseEnterAction()
        {
            IsMouseOverApplicationList = true;
        }

        public ICommand ApplicationListMouseLeaveCommand
        {
            get { return new RelayCommand(ApplicationListMouseLeaveAction); }
        }

        private void ApplicationListMouseLeaveAction()
        {
            // Don't minmize the list, if the user has accidently mouved the mouse while searching
            if (!IsTextBoxSearchFocused)
                IsApplicationListOpen = false;

            IsMouseOverApplicationList = false;
        }

        public ICommand TextBoxSearchGotKeyboardFocusCommand
        {
            get { return new RelayCommand( TextBoxSearchGotKeyboardFocusAction); }
        }

        private void TextBoxSearchGotKeyboardFocusAction()
        {
            IsTextBoxSearchFocused = true;
        }

        public ICommand TextBoxSearchLostKeyboardFocusCommand
        {
            get { return new RelayCommand(TextBoxSearchLostKeyboardFocusAction); }
        }

        private void TextBoxSearchLostKeyboardFocusAction()
        {
            if (!IsMouseOverApplicationList)
                IsApplicationListOpen = false;

            IsTextBoxSearchFocused = false;
        }

        public ICommand ClearSearchCommand
        {
            get { return new RelayCommand( ClearSearchAction); }
        }

        private void ClearSearchAction()
        {
            Search = string.Empty;
        }
        private void ShowWindowAction()
        {
            if (_isInTray)
                ShowWindowFromTray();

            if (!IsActive)
                BringWindowToFront();
        }
        private void ShowWindowFromTray()
        {
            _isInTray = false;

            Show();

            _notifyIcon.Visible = SettingsManager.Current.TrayIcon_AlwaysShowIcon;
        }
        private void InitNotifyIcon()
        {
            _notifyIcon = new NotifyIcon();

            // Get the application icon for the tray
            using (var iconStream = System.Windows.Application.GetResourceStream(new Uri("pack://application:,,,/Resources/Images/Hoster.ico"))?.Stream)
            {
                if (iconStream != null)
                    _notifyIcon.Icon = new Icon(iconStream);
            }

            _notifyIcon.Text = Title;
            _notifyIcon.DoubleClick += NotifyIcon_DoubleClick;
            _notifyIcon.MouseDown += NotifyIcon_MouseDown;
           _notifyIcon.Visible = SettingsManager.Current.TrayIcon_AlwaysShowIcon;
        }
        private void NotifyIcon_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            var trayMenu = (System.Windows.Controls.ContextMenu)FindResource("ContextMenuNotifyIcon");
            trayMenu.IsOpen = true;
        }

        private void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            ShowWindowAction();
        }

        private void BringWindowToFront()
        {
            if (WindowState == WindowState.Minimized)
                WindowState = WindowState.Normal;

            Activate();
        }
        

        private void MetroWindowMain_StateChanged(object sender, EventArgs e)
        {

        }

        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {

        }

        private void ScrollViewer_ManipulationBoundaryFeedback(object sender, ManipulationBoundaryFeedbackEventArgs e)
        {

        }

        private void HeaderBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        #region Commands
        public ICommand OpenApplicationListCommand
        {
            get { return new RelayCommand(OpenApplicationListAction); }
        }

        private void OpenApplicationListAction()
        {
            IsApplicationListOpen = true;
        }

        public ICommand OpenSettingsCommand
        {
            get { return new RelayCommand( OpenSettingsAction); }
        }

        private void OpenSettingsAction()
        {
            OpenSettings();
        }

        public ICommand CloseSettingsCommand
        {
            get { return new RelayCommand(CloseSettingsAction); }
        }

        private void CloseSettingsAction()
        {
            CloseSettings();
        }

        public ICommand ShowWindowCommand
        {
            get { return new RelayCommand( ShowWindowAction); }
        }

      

        public ICommand CloseApplicationCommand
        {
            get { return new RelayCommand(CloseApplicationAction); }
        }
        #endregion

        #endregion

        #region NotifyIcon
       
     
        #endregion
    }

}
