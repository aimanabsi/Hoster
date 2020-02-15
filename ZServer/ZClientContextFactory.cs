using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ZServer
{
    public class ZClientContextFactory
    {
       public const int HL7 = 1;
       public  const int ASTM = 2;
        public ZClientContextFactory()
        {

        }
        public IZClientContext CreateContext(EndPoint remoteEndPoint, Stream stream, Socket socket, int bufferSize,int protocolType,bool respond,int responseDelay)
        {
            IZClientContext _context;
            switch (protocolType)
            {
                case HL7:
                    _context = new HL7ClientContext(remoteEndPoint,stream,socket,bufferSize, respond, responseDelay);
                    break;

                case ASTM:
                    _context=new ASTMClientContext(remoteEndPoint, stream, socket, bufferSize,respond,responseDelay);
                    break;
                default:
                    _context = null;
                    break;
            }

            return _context;

        }

        public IZClientContext CreateContext(string deviceName,EndPoint remoteEndPoint, Stream stream, Socket socket,int bufferSize,bool respond,int responseDelay) {
            IZClientContext _context;

            switch (deviceName.Trim())
            {
                case "Nihon Kohden CelltacG":
                    _context = new NihonKohdenCelltacGContext(remoteEndPoint, stream, socket, bufferSize, respond, responseDelay);
                    break;
                default:
                    throw new Exception("Unknown device !");
            }
          
            return _context;
        }
    }
}
