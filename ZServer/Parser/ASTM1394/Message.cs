using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZServer.Parser.ASTM1394
{
 public   class Message
    {
        static string SOH = char.ConvertFromUtf32(1);
        static string STX = char.ConvertFromUtf32(2);
        static string ETX = char.ConvertFromUtf32(3);
        static string EOT = char.ConvertFromUtf32(4);
        static string ENQ = char.ConvertFromUtf32(5);
        static string ACK = char.ConvertFromUtf32(6);
        static string NACK = char.ConvertFromUtf32(21);
        static string ETB = char.ConvertFromUtf32(23);
        static string LF = char.ConvertFromUtf32(10);
        static string CR = char.ConvertFromUtf32(13);
     public   Dictionary<string ,Dictionary<int,Record>> Records { get; set; }
      public  string RowMessage { get; set; }
        public Message(string msg)
        {
            this.RowMessage = msg;
            GenerateRecords();
        }

        public bool ValidateMessage()
        {
            bool isValid = false;
            if (RowMessage == "")
                return false;
            var expr = "\x02(.*?)\x1C\x0D";
            

            return isValid;
        }


        private void GenerateRecords()
        {
            // - omitt STX ETX 
            string msg = RowMessage.Replace(STX, "");
            msg= RowMessage.Replace(ETX, "");
            //  - omit new lines 
//msg = msg.Replace("\n", "");
            // - Split by CR 
            string [] rowRecords=    msg.Split(CR[0]);
            Records = new Dictionary<string, Dictionary<int, Record>>();
            foreach(string rcs in rowRecords)
            {
                string rc = rcs.Trim();
                if (Records.ContainsKey(rc[0].ToString())) {
                    Dictionary<int, Record> d;
                    Records.TryGetValue(rc[0].ToString(), out d);
                    if (rc[0] == 'H')
                    {
                        d.Add(1, new Record(rc.Trim()));
                    }
                    else {
                        d.Add(Int32.Parse(rc[2].ToString()), new Record(rc.Trim()));
                    }
                   
                }
                else
                {
                    Dictionary<int, Record> c = new Dictionary<int, Record>();
                    if (rc[0] == 'H')
                    {
                        c.Add(1, new Record(rc));
                    }
                    else
                    {
                        c.Add(Int32.Parse(rc[2].ToString()), new Record(rc));
                    }


                    Records.Add(rc[0].ToString(),c);
                }
            }

        }

    }
}
