using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZServer.Parser
{
   public class ZMessage
    {
        private string _pID;
        private string _sampleID;
        public string SampleID { get => _sampleID; set => _sampleID = value; }
        private string _tESTDESC;
        private string _tESTRESULT;
        private string _tESTREFERENCE;
         private string _dATE;
        public string PID { get => _pID; set => _pID = value; }
        public string TESTDESC { get => _tESTDESC; set => _tESTDESC = value; }
        public string TESTRESULT { get => _tESTRESULT; set => _tESTRESULT = value; }
        public string TESTREFERENCE { get => _tESTREFERENCE; set => _tESTREFERENCE = value; }
        public string DATE { get => _dATE; set => _dATE = value; }
        public string UNIT { get; set; }
        public ZMessage()
        {

        }
    }
}
