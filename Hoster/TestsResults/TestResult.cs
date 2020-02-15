using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hoster.TestsResults
{
   public class TestResult
    {
        #region properties 
        private int _id;
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }
        private  string _test;
        public string Test {
            get { return _test; }
            set { _test = value; }
        }
        private string _testCode;
        public string TestCode
        {
            get { return _testCode; }
            set { _testCode = value; }
        }

        public string Result
        {
            get
            {
                return _result;
            }

            set
            {
                _result = value;
            }
        }

        public string Unit
        {
            get
            {
                return _unit;
            }

            set
            {
                _unit = value;
            }
        }

        public string Reference
        {
            get
            {
                return _reference;
            }

            set
            {
                _reference = value;
            }
        }

        public int ResultState
        {
            get
            {
                return _resultState;
            }

            set
            {
                _resultState = value;
            }
        }

        public bool Formated
        {
            get
            {
                return _formated;
            }

            set
            {
                _formated = value;
            }
        }

        public DateTime Date
        {
            get
            {
                return _date;
            }

            set
            {
                _date = value;
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

        public string SampleID
        {
            get
            {
                return _sampleID;
            }

            set
            {
                _sampleID = value;
            }
        }

        private string _result;
        private string _unit;

        private string _reference;
        private int _resultState;
        private bool _formated;
        private DateTime _date;
        private int _deviceID;
        private int _listenerID;
        private string _sampleID;
      
        #endregion

        public TestResult(string sampleID,string test,string testResult,string unit,string reference,int listenerID,int deviceID,DateTime date)
        {
            this._sampleID = sampleID;
            this._test = test;
            this._result = testResult;
            this._unit = unit;
            this._reference = reference;
            this._listenerID = listenerID;
            this._deviceID = deviceID;
            this._date = date;
        }
    }
}
