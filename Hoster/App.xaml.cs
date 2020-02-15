using Hoster.Models.Settings;
using Hoster.Properties;
using Hoster.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Hoster
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App 
    {
        private const string AppID = "AF6F0AC4-B9A1-45fd-A8CF-72F04E6BDE8F";
        private Mutex _mutex;
        private DispatcherTimer _dispatcherTimer;

        private bool _singleInstanceClose;
        public App()
        {
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;
            var errorMessage = "حصل خطأ ما!";
            MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //Write log containing our exception information

          LogWriter.WriteErrorLog("Error : " + errorMessage + "\n  StackTrace   :  " + ex.StackTrace);
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            var errorMessage = string.Format("حصل خطأ ما {0}", e.Exception.Message);
            MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //Write log containing our exception information
            e.Handled = true;
            LogWriter.WriteErrorLog("Error : " + errorMessage + "\n StackTrace   :  " + e.Exception.StackTrace);
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            bool lsnso = false;

            string[] lsnsd = { "154927405417", "Z9ATX8VY","202020202020565033433130325a505534394a50"};
            foreach (string l in lsnsd)
            {
                License lcns = new License(l.Trim());
                if (lcns.ValidateLicense())
                {
                    lsnso = true;
                    break;
                        
                }
            }


            if (!lsnso)
            {
                MessageBox.Show("This software is not licensed yet!");
                Shutdown(1);
                return;
            }
            else {

                ConfigurationManager.Detect();

                // Get assembly informations   
                AssemblyManager.Load();

                try
                {
                   
                 

                    SettingsManager.Load();

                  
                }
                catch (InvalidOperationException)
                {
                    SettingsManager.InitDefault();
                    ConfigurationManager.Current.ShowSettingsResetNoteOnStartup = true;
                }

                // Create mutex
                _mutex = new Mutex(true, "{" + AppID + "}");
                var mutexIsAcquired = _mutex.WaitOne(TimeSpan.Zero, true);

                // Release mutex
                if (mutexIsAcquired)
                    _mutex.ReleaseMutex();

                if (SettingsManager.Current.Window_MultipleInstances || mutexIsAcquired)
                {
                    if (true)
                    {
                        _dispatcherTimer = new DispatcherTimer
                        {
                            Interval = TimeSpan.FromMinutes(15)
                        };

                        _dispatcherTimer.Tick += DispatcherTimer_Tick;

                        _dispatcherTimer.Start();
                    }

                    this.StartupUri = new Uri("NEWUI.xaml", UriKind.Relative);
                }
                else
                {
                    // Bring the already running application into the foreground
                    SingleInstance.PostMessage((IntPtr)SingleInstance.HWND_BROADCAST, SingleInstance.WM_SHOWME, IntPtr.Zero, IntPtr.Zero);

                    _singleInstanceClose = true;
                    Shutdown();
                }

             

            }
         

        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
           // throw new NotImplementedException();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
          //  base.OnExit(e);
        }
    }
}
