using System;
using Dragablz;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;

namespace Hoster.Controls
{
    public partial class DragablzTabHostWindow : INotifyPropertyChanged
    {
        #region PropertyChangedEventHandler
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Variables
        public IInterTabClient InterTabClient { get; }
        private readonly ApplicationViewManager.Name _applicationName;

        private string _applicationTitle;
        public string ApplicationTitle
        {
            get { return _applicationTitle; }
            set
            {
                if (value == _applicationTitle)
                    return;

                _applicationTitle = value;
                OnPropertyChanged();
            }
        }

        private bool _isPuTTYControl;
        public bool IsPuTTYControl
        {
            get { return _isPuTTYControl; }
            set
            {
                if(value == _isPuTTYControl)
                    return;

                _isPuTTYControl = value;
                OnPropertyChanged();
            }
        }

        public bool ShowCurrentApplicationTitle =true;
        #endregion

        #region Constructor
        public DragablzTabHostWindow(ApplicationViewManager.Name applicationName)
        {
            InitializeComponent();
            DataContext = this;

            // Transparency
            //if (SettingsManager.Current.Appearance_EnableTransparency)
            //{
            //    AllowsTransparency = true;
            //    Opacity = SettingsManager.Current.Appearance_Opacity;
            //}

            _applicationName = applicationName;

            InterTabClient = new DragablzInterTabClient(applicationName);

            InterTabController.Partition = applicationName.ToString();

            ApplicationTitle = ApplicationViewManager.GetTranslatedNameByName(applicationName);
            

           // SettingsManager.Current.PropertyChanged += SettingsManager_PropertyChanged;
        }
        #endregion

        #region ICommand & Actions
        public ItemActionCallback CloseItemCommand => CloseItemAction;

        private void CloseItemAction(ItemActionCallbackArgs<TabablzControl> args)
        {
            // Switch between application identifiert...
            switch (_applicationName)
            {
                case ApplicationViewManager.Name.None:
                    break;
               
                case ApplicationViewManager.Name.Listeners:
                    break;
               
                default:
                    break;

            }
        }

        #region PuTTY Commands
       // public ICommand RestartPuTTYSessionCommand => new RelayCommand(RestartPuTTYSessionAction);

        private void RestartPuTTYSessionAction(object view)
        {
            //if (view is PuTTYControl puttyControl)
            //    puttyControl.RestartPuTTYSession();
        }
        #endregion
        #endregion

        #region Events
        private void SettingsManager_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //if (e.PropertyName == nameof(SettingsInfo.Window_ShowCurrentApplicationTitle))
            //    OnPropertyChanged(nameof(ShowCurrentApplicationTitle));
        }
        #endregion

        #region Window helper
        // Move the window when the user hold the title...
        private void HeaderBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }
        #endregion 
    }
}