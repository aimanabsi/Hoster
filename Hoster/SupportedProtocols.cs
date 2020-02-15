using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hoster
{
    class SupportedProtocols
    {
        public const int HL7=1;
        public const int ASTM = 2;

        private static List<Protocol> _protocols;
        public static List<Protocol> GetAll()
        {
            if (_protocols == null)
            {
                _protocols = new List<Protocol>();
                _protocols.Add(new Protocol(HL7, "HL7"));
                _protocols.Add(new Protocol(ASTM, "ASTM"));

            }
          
            return _protocols;
        }
    }

   public class Protocol
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public  Protocol(int id ,string name)
                {
            ID = id;
            Name = name;
        }
    }
}
