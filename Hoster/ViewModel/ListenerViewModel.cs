using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Hoster.TestsResults;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using ZPublisher;
using ZServer;
using ZServer.Parser;

namespace Hoster.ViewModel
{
   public class ListenerViewModel : ViewModelBase, IDisposable
    {



        #region properties
        private bool _isUp;
        int msgMaxID=1;
        public bool IsUp
        {
            get { return _isUp; }
            set { if (_isUp != value) {
                    _isUp = value;
                    RaisePropertyChanged();
                } }
        }
        private ObservableCollection<TestResult> _results;
        public ObservableCollection<TestResult> Results
        {
            get { return _results; }
            set { _results = value; }
        }

        IPEndPoint end;
        IListener lsnr;
        public ListenerSettingsViewModel ListenerSettings { get; set; }
        private string _connectionStatus;
        private string _statusColor;
        private IPublisher _publisher;
        private ListenerSettingsViewModel _listenerSettingsViewModel;

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

        private FeedbackMessage _feedback;
        public FeedbackMessage Feedback
        {
            get
            {
                if (_feedback == null)
                    _feedback = FeedbackMessage.CreatEmptyMessage();
                return _feedback;
            }
            set { if (value != _feedback) { _feedback = value; RaisePropertyChanged(); } }
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
        #region commands
        private RelayCommand _startEvent;
        public RelayCommand StartEvent
        {
            get
            {
                return this._startEvent ?? (this._startEvent = new RelayCommand(StartListener));
            }
        }

        #endregion

        #region start circuit breaker settings
        int waitInterval = 45000;
        int maxRetryTimes = 100;
        int retryTimes = 0;
        #endregion


        #region functions
        public Task  StartListenerAsync()
        {
            return Task.Run(()=> {
                StartListener();
            });

        }


        public void StartListener()
        {

            string pcName = System.Environment.MachineName;
            if (pcName.Trim().ToLower() == ListenerSettings.ListenerHost.Trim().ToLower())
            {
                try
                {
                    lsnr.Start();
                    IsUp = true;
                }
                catch (Exception ex)
                {
                    Feedback = FeedbackMessage.CreatErrorMessage(ex.Message);
                    LogWriter.WriteErrorLog("Error in StartListener : " + ex.Message);
                    IsUp = false;
                    if (retryTimes<=maxRetryTimes) {
                        Task.Run(() => {
                            RetryListenerStarting();
                        });
                    }
                   
                }
            }
            else
            {
                Feedback = FeedbackMessage.CreatAlertMessage("This channel work on a differnect PC");
            }
        }


        private void RetryListenerStarting()
        {
            retryTimes++;
            Thread.Sleep(2000);
            Feedback = FeedbackMessage.CreatAlertMessage("Retrying to connect again ... ");
            Thread.Sleep(waitInterval);
            StartListener();

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
            _connectionStatus = "Disconnected ";
            _statusColor = "Red";
            RaisePropertyChanged("ConnectionStatus");
            RaisePropertyChanged("StatusColor");
        }
        private void MessageReceived(object sender, MessageEventArgs args)
        {
            //  _messages.Add(args.Message);

            //HosterMessage msg = new HosterMessage();
            //msg.ID = "1";
            //msg.SampleID = "1";
            //msg.TestDesc = args.Message;
            ////We should here parse the msgs 

            //Application.Current.Dispatcher.BeginInvoke(
            //  DispatcherPriority.Background,
            //   new Action(() =>
            //   {
            //       AddMessage(msg);
            //   }));

            //HL7ZParser parser = new HL7ZParser();


            //if (!parser.AddToBuffer(args.Message))
            //{
            //    // Parse the message 
            //  List<ZMessage> zmsgs=  parser.Parse();


            //}
            //else
            //{
            //    LogWriter.WriteErrorLog("not expected");
            //}
            ////Check if auto_publish is enabled
            //if (_listenerSettingsViewModel.AutoPublish)
            //{
            //    //Publish to the LISDB 
            //    //parse the message and convert it to coloumn ,value pair.

            //  //  _publisher.Publish(_listenerSettingsViewModel.LisTableName,)
            //}


        }

        private void ZMessageParsed(object sender, ZMessageEventArgs args)
        {
            HosterMessage msg = new HosterMessage();
            msg.ID = msgMaxID.ToString();
            msg.SampleID = args.Message.SampleID;
            if (_listenerSettingsViewModel.GetTestCode(args.Message.TESTDESC.Trim()) != "")
            {
                msg.TestDesc = _listenerSettingsViewModel.GetTestCode(args.Message.TESTDESC.Trim());
            }
            else {
                msg.TestDesc = args.Message.TESTDESC;
            }
            msg.NormalRange = args.Message.TESTREFERENCE;
            msg.Result = args.Message.TESTRESULT;
            msg.Unit = args.Message.UNIT;
            TestResult result = new TestResult(args.Message.SampleID,
                args.Message.TESTDESC, args.Message.TESTRESULT,
                 args.Message.UNIT, args.Message.TESTREFERENCE,
                 ListenerSettings.ListenerID, ListenerSettings.DeviceID,DateTime.Now);
            if (IsResultFoundInDB(result)) {
                Dictionary<string, string> conditions = new Dictionary<string, string>();

                conditions.Add("sample_id", result.SampleID);
                conditions.Add("test_desc", result.Test);
                ListenerSettings.DataAccess.Delete(DBTablesNames.RESULTS_TABLE, conditions);
            }

            int inserted = AddToDB(result);
            if (inserted != 0)
            {
                result.ID = inserted;
            }
            else
            {
                throw new Exception("Error : Couldn't insert the result into the Database!");
            }
            if (_listenerSettingsViewModel.AutoPublish) {
                bool _published = Publish(result);
                if (!_published)
                {
                    LogWriter.WriteErrorLog("Error : couldn't publish result to LIS . ");
                    Feedback = FeedbackMessage.CreatErrorMessage("Failed Publishing resutl into LIS .");
                }
            }
           
            Application.Current.Dispatcher.BeginInvoke(
              DispatcherPriority.Background,
               new Action(() =>
               {
                   AddResult(result);
               }));
            msgMaxID = msgMaxID + 1;
        }

        private int AddToDB(TestResult result )
        {
            int i = 0;

            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("sample_id", result.SampleID);
            data.Add("test_desc", result.Test);
            data.Add("test_result", result.Result);
            data.Add("test_unit", result.Unit);
            data.Add("test_range", result.Reference);
            data.Add("device_id", result.DeviceID.ToString());
            i = ListenerSettings.DataAccess.Insert(DBTablesNames.RESULTS_TABLE,data);
            return i;
        }

        private bool Publish(TestResult result)
        {
            bool published = false;
            Dictionary<String, string> data = new Dictionary<string, string>();
            data.Add(ListenerSettings.LisPIDCol, result.SampleID);
            data.Add(ListenerSettings.LisTestDescCol, result.Test);
            data.Add(ListenerSettings.LisResultCol, result.Result);
            data.Add(ListenerSettings.LisUnitCol, result.Unit);
            data.Add(ListenerSettings.LisReferenceCol, result.Reference);
            published= _publisher.Publish(ListenerSettings.LisTableName,data);
            return published;
        }

       
        private bool IsResultFoundInDB(TestResult result)
        {
            bool found = false;


            return found;

        }
        private void AddMessage(HosterMessage msg)
        {
            _messages.Add(msg);
            RaisePropertyChanged("Messages");

        }
        private void AddResult(TestResult result)
        {
            Results.Add(result);
            RaisePropertyChanged("Results");
        }

        private ObservableCollection<TestResult> GetStoredResults()
        {
            ObservableCollection<TestResult> results = new ObservableCollection<TestResult>();
            DataTable dt = ListenerSettings.DataAccess.GetStoredResults(ListenerSettings.ListenerID);
            foreach(DataRow dr in dt.Rows)
            {
                results.Add(new TestResult(

                    dr["sample_id"].ToString(),
                    dr["test_desc"].ToString(),
                    dr["test_result"].ToString(),
                    dr["test_unit"].ToString(),
                    dr["test_range"].ToString(),
                  ListenerSettings.ListenerID,
                  ListenerSettings.DeviceID,
                    DateTime.Now)
                    );

            }

            return results;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion

        public ListenerViewModel(string ipAddress,int port,string listenerName,ListenerSettingsViewModel listnerSettingsViewModel)
        {

            this._listenerSettingsViewModel = listnerSettingsViewModel;
            ListenerSettings = listnerSettingsViewModel;
           
            _publisher = PublisherFactory.CreatePublisher(_listenerSettingsViewModel.LisDBType,_listenerSettingsViewModel.LisDBHostname,_listenerSettingsViewModel.LisDBPort,_listenerSettingsViewModel.LisDBName,_listenerSettingsViewModel.LisDBUsername,_listenerSettingsViewModel.LisDBPassword,_listenerSettingsViewModel.LisDBName);
            end = new IPEndPoint(IPAddress.Parse(ipAddress), port);
            lsnr = new ZListener(end,_listenerSettingsViewModel.ProtocolID,true,_listenerSettingsViewModel.ResponseLatency);
            _listenerName = listenerName;
        //    lsnr.Accepted += ConnectionAccepted;
            lsnr.MessageReceived += MessageReceived;
            lsnr.ZMessageParsed += ZMessageParsed;
          //  lsnr.Disconnected += ConnectionDisconnected;
            _connectionStatus = "Disconnected";
            _statusColor = "Red";
            _messages = new ObservableCollection<HosterMessage>();
            StartListener();
            RaisePropertyChanged("ListenerSettings");
        }

        public ListenerViewModel(string listenerName,IListener listener, ListenerSettingsViewModel listnerSettingsViewModel) {
            this._listenerSettingsViewModel = listnerSettingsViewModel;
            ListenerSettings = listnerSettingsViewModel;
            this. _publisher = PublisherFactory.CreatePublisher(_listenerSettingsViewModel.LisDBType, _listenerSettingsViewModel.LisDBHostname, _listenerSettingsViewModel.LisDBPort, _listenerSettingsViewModel.LisDBName, _listenerSettingsViewModel.LisDBUsername, _listenerSettingsViewModel.LisDBPassword, _listenerSettingsViewModel.LisDBName);
            this.lsnr = listener;
            _listenerName = listenerName;
          ///  lsnr.Accepted += ConnectionAccepted;
            lsnr.MessageReceived += MessageReceived;
            lsnr.ZMessageParsed += ZMessageParsed;
          //  lsnr.Disconnected += ConnectionDisconnected;
            _connectionStatus = "Disconnected";
            _statusColor = "Red";
            _messages = new ObservableCollection<HosterMessage>();
            StartListener();
          
            RaisePropertyChanged("ListenerSettings");
        }
    }
}
