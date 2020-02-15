using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZServer.Parser.ASTM1394
{
 public   class Record
    {
        public string RowRecord { get; set; }
        public string RecordHead { get; set; }
        public Record(string rowRecord)
        {
            this.RowRecord = rowRecord;
            if(RowRecord!="")
            {
                RecordHead = rowRecord[0].ToString() ;
            }
        }
        public bool IsTerminationRecord()
        {
            if (RowRecord[0] == 'L')
                return true;
            return false;
        }
        

    }
}
