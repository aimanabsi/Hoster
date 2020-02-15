using SerialPortLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZServer
{
   public class SerialContextFactory
    {

        public static ISerialContext Create(int deviceID,SerialPortInput stream,int responseDelay)
        {
            ISerialContext c;
            switch ( deviceID)
            {
                case (int)SupportedDevices.NihonKhoden_Celltac_g:
                    c = new CelltacESSerialASTMContext(stream, responseDelay);
                    break;
                case (int)SupportedDevices.Roche_Cobas_411e:
                    c = new RocheCobas411ASTMContext(stream, responseDelay);
                    break;
                default:
                    c =new NullSerialContext();
                    break;
            }

            return c;

        }

    }
}
