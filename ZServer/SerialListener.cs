using SerialPortLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
namespace ZServer
{
   public class SerialListener:IListener
    {
        SerialPortInput serialPort ;
        ISerialContext context;
        public string Port { get; private set; }
        public int BaudRate { get; private set; }
        public event EventHandler<ZMessageEventArgs> ZMessageParsed=delegate { };
        public event EventHandler<MessageEventArgs> MessageReceived = delegate { };

      
        public SerialListener(string portName,int responseDelay,int baudRate,StopBits stopBits,Parity parity,int deviceID)
        {
            Port = portName;
            BaudRate = baudRate;    
            serialPort = new SerialPortInput();
            context =  SerialContextFactory.Create( deviceID, serialPort, responseDelay);
            serialPort.SetPort(Port, BaudRate, stopBits, parity);
            context.MessageReceived += Context_MessageReceived;
            context.ZMessageParsed += Context_ZMessageParsed;
        }

    

        private void Context_ZMessageParsed(object sender, ZMessageEventArgs e)
        {
            LogWriter.WriteErrorLog("======================== Message Parsed ============================");
            LogWriter.WriteErrorLog("SampleID : " + e.Message.SampleID);
            LogWriter.WriteErrorLog("Test : " + e.Message.TESTDESC);
            LogWriter.WriteErrorLog("Result : " + e.Message.TESTRESULT);
            LogWriter.WriteErrorLog("Reference : " + e.Message.TESTREFERENCE);
            LogWriter.WriteErrorLog("Unit : " + e.Message.UNIT);
            ZMessageParsed(this, e);
        }

        private void Context_MessageReceived(object sender, MessageEventArgs e)
        {
            LogWriter.WriteErrorLog("Message aquired : "+e.Message);
            MessageReceived(this, e);
        }

        private void OnMessageReceived(object sender, MessageEventArgs args)
        {
          
            
        }

        public void SetPort(string portName,int responseDelay, int baudRate, StopBits stopBits, Parity parity) {
            serialPort.SetPort(Port, BaudRate, stopBits, parity);
        }

       public void Start()
        {
            LogWriter.WriteErrorLog("Starting .... ");
            serialPort.Connect();

            if (serialPort.IsConnected)
            {
                LogWriter.WriteErrorLog("Serial port is connected ..");

            }
            else {
                LogWriter.WriteErrorLog("Serial port is not  connected !!!");
                throw new Exception("Couldn't connect to serial  Port "+Port);
            }
                }

        public void Disconnect()
        {
           
        }
    }
}
