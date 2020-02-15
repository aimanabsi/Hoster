using SerialPortLib;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZServer.Serial
{
  public  class SerialPortSettings
    {
        public string PortName { get; set; }
        public int BaudRate { get; set; }
        public StopBits StopBits { get; set; }
        public Parity Parity { get; set; }
        public DataBits DataBits { get; set; }
        public SerialPortSettings(string portName, int baudRate, StopBits stopBits, Parity parity,DataBits dataBits=DataBits.Eight )
        {
            this.PortName = portName;
            this.BaudRate = baudRate;
            this.StopBits = stopBits;
            this.Parity = parity;
            this.DataBits = dataBits;
        }
    }
}
