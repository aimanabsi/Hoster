using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZServer.Parser.astm
{
  public   class ASTMParser
    {
        public  static string SOH = char.ConvertFromUtf32(1);
        public static string STX = char.ConvertFromUtf32(2);
        public static string ETX = char.ConvertFromUtf32(3);
        public static string EOT = char.ConvertFromUtf32(4);
        public static string ENQ = char.ConvertFromUtf32(5);
        public static string ACK = char.ConvertFromUtf32(6);
        public static string NACK = char.ConvertFromUtf32(21);
        public static string ETB = char.ConvertFromUtf32(23);
        public static string LF = char.ConvertFromUtf32(10);
        public static string CR = char.ConvertFromUtf32(13);
        public static string Buffer { get; set; }
        public ASTMParser()
        {

        }


        public static void ResetBuffer()
        {
            Buffer = "";
        }

        public static bool AddToBuffer(string msg)
        {
            bool finished = false;
            //if it is : message terminator record
            if (msg.StartsWith("L"))
                return true;
            Buffer += msg;
            return finished;

        }
    }
}
