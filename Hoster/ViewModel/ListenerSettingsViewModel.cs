using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO.Ports;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;


namespace Hoster.ViewModel
{
    public class ListenerSettingsViewModel : ViewModelBase
    {
        #region properties

        private bool _inEditMode;
        private string _listenerHost;
        public string ListenerHost
        {
            get { return _listenerHost; }
            set { _listenerHost = value;RaisePropertyChanged(); }
        }
        public bool InEditMode
        {
            get { return _inEditMode; }
            set {
                if (_inEditMode != value) {
                    _inEditMode = value;
                    RaisePropertyChanged();
                }
            }

        }
        private int _listenerID;
        private bool _isComPort;
        private bool _isTCPIP;
        public bool IsTCPIP
        {
            get { return _isTCPIP; }
            set { _isTCPIP = value;
                RaisePropertyChanged();
            }
        }
        public bool IsComPort
        {
            get { return _isComPort; }
            set { _isComPort = value;
                RaisePropertyChanged();
            }
        }
        public int ListenerID
        {
            get
            {
                return _listenerID;
            }

            set
            {
                _listenerID = value;
            }
        }

        public int DeviceID
        {
            get
            {
                return _deviceID;
            }

            set
            {
                _deviceID = value;
                RaisePropertyChanged();
            }
        }

        public int ConnType
        {
            get
            {
                return _connType;
            }

            set
            {
                _connType = value;
                if (_connType == 1)
                {
                    IsComPort = false;
                    IsTCPIP = true;
                }
                else if (_connType == 2)
                {
                    IsComPort = true;
                    IsTCPIP = false;

                }
                RaisePropertyChanged();
               
            }
        }

        public int ProtocolID
        {
            get
            {
                return _protocolID;
            }

            set
            {
                _protocolID = value;
                RaisePropertyChanged();
            }
        }

        public int ResponseLatency
        {
            get
            {
                return _responseLatency;
            }

            set
            {
                _responseLatency = value;
                RaisePropertyChanged();
            }
        }

        public string IPAddress
        {
            get
            {
                return _ipAddress;
            }

            set
            {
                _ipAddress = value;
                RaisePropertyChanged();
            }
        }

        public int Port
        {
            get
            {
                return _port;
            }

            set
            {
                _port = value;
                RaisePropertyChanged();
            }
        }

        private string _comPort;
        public string COMPort { get { return _comPort; }  set { _comPort = value;RaisePropertyChanged(); } }

        private int _baudRate;
        public int BaudRate { get { return _baudRate; }  set { _baudRate = value;RaisePropertyChanged(); } }

        private StopBits _stopBits;
        public StopBits StopBits {
            get { return _stopBits; }
            set { _stopBits = value;RaisePropertyChanged(); }
            }
        private int _parity;
       
        public int Parity
        {
            get { return _parity; }
            set {
                _parity = value;
                RaisePropertyChanged();
            }
        }
             
        public string ProtocolVersion
        {
            get
            {
                return _protocolVersion;
            }

            set
            {
                _protocolVersion = value;
                RaisePropertyChanged();
            }
        }

        public int LisDBType
        {
            get
            {
                return _lisDBType;
            }

            set
            {
                _lisDBType = value;
                RaisePropertyChanged();
            }
        }

        public string LisDBHostname
        {
            get
            {
                return _lisDBHostname;
            }

            set
            {
                _lisDBHostname = value;
                RaisePropertyChanged();
            }
        }

        public string LisDBUsername
        {
            get
            {
                return _lisDBUsername;
            }

            set
            {
                _lisDBUsername = value;
            }
        }

        public string LisDBPassword
        {
            get
            {
                return _lisDBPassword;
            }

            set
            {
                _lisDBPassword = value;
                RaisePropertyChanged();
            }
        }

        private string _networkAdapter;
        public string LisTableName
        {
            get
            {
                return _lisTableName;
            }

            set
            {
                _lisTableName = value;
                RaisePropertyChanged();
            }
        }

        public string LisPIDCol
        {
            get
            {
                return _lisPIDCol;
            }

            set
            {
                _lisPIDCol = value;
                RaisePropertyChanged();
            }
        }

        public string LisTestDescCol
        {
            get
            {
                return _lisTestDescCol;
            }

            set
            {
                _lisTestDescCol = value;
                RaisePropertyChanged();
            }
        }

        public string LisResultCol
        {
            get
            {
                return _lisResultCol;
            }

            set
            {
                _lisResultCol = value;
                RaisePropertyChanged();
            }
        }

        public string LisUnitCol
        {
            get
            {
                return _lisUnitCol;
            }

            set
            {
                _lisUnitCol = value;
                RaisePropertyChanged();
            }
        }

        public string LisReferenceCol
        {
            get
            {
                return _lisReferenceCol;
            }

            set
            {
                _lisReferenceCol = value;
                RaisePropertyChanged();
            }
        }

        public int LisDBPort
        {
            get
            {
                return _lisDBPort;
            }

            set
            {
                _lisDBPort = value;
                RaisePropertyChanged();
            }
        }

        public string LisDBName
        {
            get
            {
                return _lisDBName;
            }

            set
            {
                _lisDBName = value;
                RaisePropertyChanged();
            }
        }

        public List<Device> Devices
        {
            get
            {
                return _devices;
            }

            set
            {
                _devices = value;
            }
        }

        public List<string> SerialPorts
        {
            get
            {
                return _serialPorts;
            }

            set
            {
                _serialPorts = value;
            }
        }

        private List<int> _baudRatesList;
        public List<int> BaudRatesList
        {
            get
            {
                if (_baudRatesList == null)
                {
                    _baudRatesList = new List<int>();
                    _baudRatesList.Add(9600);
                    _baudRatesList.Add(14400);
                    _baudRatesList.Add(19200);
                }

                return _baudRatesList;
            }
        }
        private List<NetworkAdapter> _networkAdapters;

        public List<ConnectionType> ConnTypes
        {
            get
            {
                return _connTypes;
            }

            set
            {
                _connTypes = value;
            }
        }

        private List<DBType> _dBTypes;
        public List<Protocol> Protocols
        {
            get
            {
                return _protocols;
            }

            set
            {
                _protocols = value;
            }
        }

        public List<NetworkAdapter> NetworkAdapters
        {
            get
            {
                return _networkAdapters;
            }

            set
            {
                _networkAdapters = value;
            }
        }

        public List<DBType> DBTypes
        {
            get
            {
                return _dBTypes;
            }

            set
            {
                _dBTypes = value;
            }
        }

        private List<ConnectionType> _connTypes;
        private int _deviceID;

        private int _connType;
        private int _protocolID;
        private int _responseLatency;
        private string _ipAddress;
        private int _port;
        private List<string> _serialPorts;
        private List<Protocol> _protocols;
        private string _protocolVersion;
        private int _lisDBType;
        private string _lisDBHostname;
        private int _lisDBPort;
        private string _lisDBName;
        private string _lisDBUsername;
        private string _lisDBPassword;
        private string _lisTableName;
        private string _lisPIDCol;
        private string _lisTestDescCol;
        private string _lisResultCol;
        private string _lisUnitCol;
        private string _lisReferenceCol;
        private List<Device> _devices;
        private bool _autoPublish;
        private List<StopBits> _stopBitsList;

        public List<StopBits> StopBitsList
        {
            get {
                if (_stopBitsList == null) {
                    _stopBitsList = new List<StopBits>();
                    _stopBitsList.Add(StopBits.None);
                    _stopBitsList.Add(StopBits.One);
                    _stopBitsList.Add(StopBits.Two);
                }
                return _stopBitsList;
            }
        }
        private List<string > _parityList;
        public List<string> ParityList
        {
            get
            {
                if (_parityList == null)
                {
                    _parityList = new List<string>();
                    _parityList.Add("None");
                    _parityList.Add("Odd");
                    _parityList.Add("Even");
                }
                return _parityList;
            }
        }
        private FeedbackMessage _feedback;
        public FeedbackMessage Feedback
        {
            get {
                if (_feedback == null)
                    _feedback = FeedbackMessage.CreatEmptyMessage();
                return _feedback;
            }
            set
            {
                if (value != _feedback) {
                    _feedback = value;
                    RaisePropertyChanged();
                }
            }
        }

        Dictionary<string,string> TestsCodes { get; set; }
        #endregion
        IDataAccess _model;

        public IDataAccess DataAccess{
            get { return _model; }
            }
        public ListenerSettingsViewModel(IDataAccess model) {
            _model = model;
            TestsCodes = new Dictionary<string, string>();

            _devices = new List<Device> ();
            _connTypes = new List<ConnectionType>();
            _protocols = new List<Protocol>();
            _networkAdapters = new List<NetworkAdapter>();
            _dBTypes = new List<DBType>();
            DataTable _devicesData = _model.GetDevices();
            foreach (DataRow dr in _devicesData.Rows)
            {
                _devices.Add(new Device(Int32.Parse(dr["id"].ToString()),dr["device_name"].ToString()));
            }

       
          
                _connTypes.Add(new ConnectionType(1, "TCP/IP"));
                _connTypes.Add(new ConnectionType(2, "RS232"));
            
                _protocols.Add(new Protocol(1, "HL7"));
                _protocols.Add(new Protocol(2, "ASTM"));

            //
            FillNetworkAdapters();
            FillDBTypes();
            Init(1);
        }



        public ListenerSettingsViewModel(IDataAccess model ,int listenerID)
        {
            _model = model;
            TestsCodes = new Dictionary<string, string>();

            _devices = new List<Device>();
            _connTypes = new List<ConnectionType>();
            _protocols = new List<Protocol>();
            _networkAdapters = new List<NetworkAdapter>();
            _dBTypes = new List<DBType>();
            DataTable _devicesData = _model.GetDevices();
            foreach (DataRow dr in _devicesData.Rows)
            {
                _devices.Add(new Device(Int32.Parse(dr["id"].ToString()), dr["device_name"].ToString()));
            }

            _connTypes.Add(new ConnectionType(1, "TCP/IP"));
            _connTypes.Add(new ConnectionType(2, "RS232"));

            _protocols.Add(new Protocol(1, "HL7"));
            _protocols.Add(new Protocol(2, "ASTM"));

            //
            FillNetworkAdapters();
            FillDBTypes();
            Init(listenerID);

        }
        #region functions

        public void Edit()
        {
            InEditMode = true;

        }

        public void Cancel()
        {
            InEditMode = false;
            Init(_listenerID);
        }
        public void FillNetworkAdapters()
        {

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                _networkAdapters.Add(new NetworkAdapter(nic.Id,nic.Name,nic.Description));
            }
          
        }

        public void FillDBTypes()
        {
            //  DataTable 
            _dBTypes.Add(new DBType(1, "Oracle"));
            _dBTypes.Add(new DBType(2, "MS SqlServer"));

        }

        public void UpdateModel()
        {
            Dictionary<string,string> lsnrData = new Dictionary<string, string>();
            lsnrData.Add("id", _listenerID.ToString());
            lsnrData.Add("device_id", _deviceID.ToString());
            lsnrData.Add("protocol_id", _protocolID.ToString());
            lsnrData.Add("connection_type", _connType.ToString());
            lsnrData.Add("response_latency", _responseLatency.ToString());
            lsnrData.Add("ip", _ipAddress.ToString());
            lsnrData.Add("lis_db_hostname", _lisDBHostname.ToString());
            lsnrData.Add("lis_db_port", _lisDBPort.ToString());
            lsnrData.Add("lis_db_username", _lisDBUsername.ToString());
            lsnrData.Add("lis_db_password", _lisDBPassword.ToString());
            lsnrData.Add("lis_tablename", _lisTableName.ToString());
            lsnrData.Add("lis_dbname", _lisDBName.ToString());
            lsnrData.Add("lis_pid_col", _lisPIDCol.ToString());
            lsnrData.Add("lis_testdesc_col", _lisTestDescCol.ToString());
            lsnrData.Add("lis_result_col", _lisResultCol.ToString());
            lsnrData.Add("lis_unit_col", _lisUnitCol.ToString());
            lsnrData.Add("lis_reference_col", _lisReferenceCol.ToString());
            lsnrData.Add("port", _port.ToString());
            lsnrData.Add("network_adapter_id", _networkAdapter.ToString());
            lsnrData.Add("lis_db_type", _lisDBType.ToString());
            if (_autoPublish)
            {
                lsnrData.Add("auto_publish", "1");
            }
            else
                lsnrData.Add("auto_publish", "0");

         //  lsnrData.Add("bit_rate", _baudRate.ToString());
        //    lsnrData.A
            //lsnrData.Add("port", _port.ToString());
            //lsnrData.Add("port", _port.ToString());
            //lsnrData.Add("port", _port.ToString());
            //lsnrData.Add("port", _port.ToString());
            //lsnrData.Add("port", _port.ToString());
            if (_model.Update("listeners", "id", lsnrData)==1) {
                Feedback = FeedbackMessage.CreatSuccessMessage("تم التعديل بنجاح");
                InEditMode = false;
            }
            else
            {
                Feedback = FeedbackMessage.CreatErrorMessage("فشل حفظ التعديلات");
            }

        }

        public void Init(int listenerID)
        {
            DataTable _lsnrData = _model.GetListenerByID(listenerID);
            foreach (DataRow dr in _lsnrData.Rows)
            {
                ListenerID = Int32.Parse(dr["id"].ToString());
                DeviceID = Int32.Parse(dr["device_id"].ToString());
                ProtocolID = Int32.Parse(dr["protocol_id"].ToString());
                ProtocolVersion = dr["protocol_version"].ToString();
                ConnType = Int32.Parse(dr["connection_type"].ToString());
                ResponseLatency = Int32.Parse(dr["response_latency"].ToString());
                IPAddress = dr["ip"].ToString();
                if (dr["port"].ToString().Trim() != "")
                {
                    Int32.TryParse(dr["port"].ToString(),out _port);
                }
                 LisDBHostname  = dr["lis_db_hostname"].ToString();
                if (dr["lis_db_port"].ToString().Trim() != "")
                {
                     Int32.TryParse(dr["lis_db_port"].ToString(),out _lisDBPort);
                }

                COMPort = dr["com_port"].ToString();
                Int32.TryParse(dr["bit_rate"].ToString(),out _baudRate);
                Int32.TryParse(dr["parity_bits"].ToString(),out _parity);
                LisDBUsername = dr["lis_db_username"].ToString();
                LisDBPassword = dr["lis_db_password"].ToString();
                LisTableName = dr["lis_tablename"].ToString();
                LisDBName = dr["lis_dbname"].ToString();
                LisPIDCol = dr["lis_pid_col"].ToString();
                LisTestDescCol = dr["lis_testdesc_col"].ToString();
                LisResultCol = dr["lis_result_col"].ToString();
                LisUnitCol = dr["lis_unit_col"].ToString();
                ListenerHost = dr["listener_host"].ToString();
                NetworkAdapter= dr["network_adapter_id"].ToString();
                LisReferenceCol = dr["lis_reference_col"].ToString();
                if (dr["lis_db_type"].ToString().Trim()!="") {
                    LisDBType = Int32.Parse(dr["lis_db_type"].ToString());
                }
              
                if (_connType==1)
                {
                    IsComPort = false;
                    IsTCPIP = true;
                }
                else if (_connType == 2)
                {
                    IsComPort = true;
                    IsTCPIP = false;
                }
                if (dr["auto_publish"].ToString().Trim()!="") {
                    int _auto = Int16.Parse(dr["auto_publish"].ToString());
                    if (_auto == 1)
                        AutoPublish = true;
                    else
                        AutoPublish = false;
                }
           
               
            }
            DataTable _deviceTestsCodes = DataAccess.GetDeviceTestsCodes(DeviceID);
            foreach (DataRow dr in _deviceTestsCodes.Rows)
            {
                TestsCodes.Add(dr["device_test_code"].ToString(), dr["test_code"].ToString());
            }


        }
        public string GetTestCode(string deviceTestCode)
        {
            string code = "";

            if(TestsCodes.TryGetValue(deviceTestCode,out code))
            {
                return code;
            }

            return code;
        }
        
        #endregion

        #region commands
        private RelayCommand _saveChanges;
        public RelayCommand SaveChanges
        {
            get
            {
                return this._saveChanges ?? (this._saveChanges = new RelayCommand(UpdateModel));
            }
        }

        private RelayCommand _editEvent;
        public RelayCommand EditEvent
        {
            get
            {
                return this._editEvent ?? (this._editEvent = new RelayCommand(Edit));
            }
        }
        private RelayCommand _cancelEvent;
        public RelayCommand CancelEvent
        {
            get
            {
                return this._cancelEvent ?? (this._cancelEvent = new RelayCommand(Cancel));
            }
        }


        public string NetworkAdapter
        {
            get
            {
                return _networkAdapter;
            }

            set
            {
                _networkAdapter = value;
            }
        }

        public bool AutoPublish
        {
            get
            {
                return _autoPublish;
            }

            set
            {
                if (_autoPublish != value)
                {
                    _autoPublish = value;
                    RaisePropertyChanged();
                }
            }
        }
        #endregion
    }

    #region subclasses

    public class Device
    {
        private int _id;

        public int ID
        {
            get
            {
                return _id;
            }

            set
            {
                _id = value;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
            }
        }

        private string _name;
        public Device()
        {

        }
        public Device(int id, string name)
        {
            _id = id;
            _name = name;
        }
    }

    public class ConnectionType
    {
        #region props
        private int _id;

        public int ID
        {
            get
            {
                return _id;
            }

            set
            {
                _id = value;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
            }
        }

        private string _name;
        #endregion

        public ConnectionType(int id, string name)
        {
            _id = id;
            _name = name;
        }
    }

    public class Protocol
    {
        private int _id;
        private string _name;

        public int ID
        {
            get
            {
                return _id;
            }

            set
            {
                _id = value;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
            }
        }
        public Protocol(int id, string name)
        {
            _id = id;
            _name = name;

        }
    }

    public class DBType
    {
        private int _id;
        private string _name;

        public int ID
        {
            get
            {
                return _id;
            }

            set
            {
                _id = value;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
            }
        }
        public DBType(int id, string name)
        {
            _id = id;
            _name = name;

        }
    }

    public class NetworkAdapter
    {
        private string _id;
        private string _name;
        private string _description;
        public string ID
        {
            get
            {
                return _id;
            }

            set
            {
                _id = value;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
            }
        }

        public string Description
        {
            get
            {
                return _description;
            }

            set
            {
                _description = value;
            }
        }

        public NetworkAdapter(string id, string name, string description)
        {
            _id = id;
            _name = name;
            _description = description;

        }


    }
    #endregion

}
