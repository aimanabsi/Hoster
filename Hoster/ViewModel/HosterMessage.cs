using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hoster.ViewModel
{
 public  class HosterMessage
    {
        private string _id;
        private string _sampleID;
        private string _testDesc;
        private string _result;
        private string _unit;
        private string _normalRange;
        private string _other1;
        private string _other2;
        private string _time;

        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }
        public string SampleID
        {
            get { return _sampleID; }
            set { _sampleID = value; }
        }
        public string TestDesc
        {
            get { return _testDesc; }
            set { _testDesc = value; }
        }
        public string Other1
        {
            get
            {
                return _other1;
            }

            set
            {
                _other1 = value;
            }
        }
        public string Other2
        {
            get
            {
                return _other2;
            }

            set
            {
                _other2 = value;
            }
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

        public string NormalRange
        {
            get
            {
                return _normalRange;
            }

            set
            {
                _normalRange = value;
            }
        }

        public string Time
        {
            get
            {
                return _time;
            }

            set
            {
                _time = value;
            }
        }

        public HosterMessage()
        {

        }
        public HosterMessage(string sampleID,string test,string result,string unit,string normalRange)
        {

        }

    }
}
