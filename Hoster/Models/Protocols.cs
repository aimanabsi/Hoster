using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hoster.Models
{
public     class Protocols
    {
        public static List<Protocol> GetProtocols()
        {
            List<Protocol> prtcls = new List<Protocol>();
            prtcls.Add(new Protocol(1, "HL7"));
            prtcls.Add(new Protocol(2, "ASTM"));
            return prtcls;
        }
    }

    public class Protocol
    {
        public int ID { get; private set; }
        public string Name { get; private set; }
        public Protocol(int id,string name)
        {
            this.ID = id;
            this.Name = name;

        }
    }
}
