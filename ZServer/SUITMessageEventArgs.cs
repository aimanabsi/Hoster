using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZServer
{
  public  class SUITMessageEventArgs:EventArgs
    {
        private Dictionary<String, Dictionary<int,String>> _segments;
        public  SUITMessageEventArgs()
        {
            _segments = new Dictionary<string, Dictionary<int, string>>();
        }
        /*
         * H   => Header Segment
         * P   => Patient Segment
         * OBR => Observation Order Segment
         * OBX => Observation Result Segment
         * E   => Error Checking Segment 
         * C   => Comment Segments
         * Q   => Request Results Segment 
         * L   => Message Terminator
         * S   => Scientific Segment
         **/ 
        public static SUITMessageEventArgs Parse(String text)
        {
            SUITMessageEventArgs _msg = new SUITMessageEventArgs();
            int count = 0;
            char[] delimiter = { '|' };
            string temp = text.Trim('|');
            var tokens = temp.Split(delimiter, StringSplitOptions.None);

            foreach (var item in tokens)
            {
               // Field(count, item);
                if (item == "MSH")
                {
                    // Treat the special case "MSH" - the delimiter after the segment name counts as first field
                    ++count;
                }
                ++count;
            }

            return _msg;
        }
    }
}
