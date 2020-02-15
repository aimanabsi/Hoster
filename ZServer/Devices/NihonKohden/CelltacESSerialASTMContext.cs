using SerialPortLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZServer.Parser;
using ZServer.Parser.astm;

namespace ZServer
{
  public  class CelltacESSerialASTMContext:ISerialContext
    {
        /// <summary>
        /// a context for comunication with nihon khoden celltac es mek-7222 analyser in serial port
        /// using ASTM protocol
        /// </summary>
        /// 
        static string STX = char.ConvertFromUtf32(2);
        static string ETX = char.ConvertFromUtf32(3);
        static string ack = char.ConvertFromUtf32(6);
        static string soh = char.ConvertFromUtf32(1);
        static string stx = char.ConvertFromUtf32(2);
        static string etx = char.ConvertFromUtf32(3);
        static string eot = char.ConvertFromUtf32(4);
        static string ENQ = char.ConvertFromUtf32(5);
        static string nack = char.ConvertFromUtf32(21);
        static string etb = char.ConvertFromUtf32(23);
        static string lf = char.ConvertFromUtf32(10);
        static string CR = char.ConvertFromUtf32(13);
        public event EventHandler<ZMessageEventArgs> ZMessageParsed;
        public event EventHandler<MessageEventArgs> MessageReceived;
        int wait;
        private string _buffer = "";
        private ZMessage tempMessage;
        SerialPortInput _stream;
        bool gotSTX = false;
        bool gotETX = false;
        public CelltacESSerialASTMContext(SerialPortInput stream,int responseDelay)
        {
            this.wait = responseDelay;
            this._stream = stream;
            stream.MessageReceived += Stream_MessageReceived;

        }

        private void Stream_MessageReceived(object sender, MessageReceivedEventArgs args)
        {
            string message = Encoding.ASCII.GetString(args.Data);
            Process(message);
            MessageReceived(this, new MessageEventArgs(message));
        }

        private void ACK()
        {
            Thread.Sleep(wait);
            byte[] _bts = Encoding.ASCII.GetBytes(ack);
            _stream.SendMessage(_bts);
        }

        private void Process(string str)
        {
            string received;

            //  Thread thread;

            received = null;

            received = str;


            if (received == ack)
            {
                //    wh.Set();

            }
            else if (received == ENQ)
            {
                ACK();

            }

            else if (received == eot)
            {
                LogWriter.WriteErrorLog("message Aquired :  " + _buffer);
                Parse(_buffer);
                _buffer = "";
                tempMessage = null;

            }
            else if (received.Length <3) {
                ACK();
            }
            else
            {
                // ACK();
                if (received.Length > 3)
                {
                    //  LogWriter.WriteErrorLog("record type : " + received[2].ToString());
                }
                //   LogWriter.WriteErrorLog("Inside the else and the recieved string is : " + received);
                string recordType = received[2].ToString();
                //   
                if (tempMessage == null) {
                    tempMessage = new ZMessage();
                }

                switch (recordType)
                {

                    case "H":
                        _buffer = "";
                        tempMessage = null;
                        tempMessage = new ZMessage();
                        ACK();

                        break;
                    case "Q":
                        string[] fields = received.Split('|');
                        string[] comps = fields[2].Split('^');
                        string barcode = comps[1];

                        //    _barcode = Convert.ToInt32(barcode);

                        //     orderFlag = true;
                        ACK();
                        break;
                    case "P":
                        //sample data
                        tempMessage.PID = received.Split('|').GetValue(2).ToString().Trim();
                        ACK();
                        break;
                    case "R":

                        string[] resultsRows = received.Split(STX.ToCharArray());
                        if (resultsRows.Count() > 1)
                        {
                            LogWriter.WriteErrorLog("There are many results in this block");
                            foreach (string row in resultsRows)
                            {

                                if (row.Trim() != "") {
                                    string test = "";
                                    string result = "";
                                    string resultUnit = "";
                                    string reference = "";
                                    string testComponent = row.Split('|').GetValue(2).ToString();
                                    test = testComponent.Split('^').GetValue(3).ToString();
                                    result = row.Split('|').GetValue(3).ToString();
                                    string unitComponent = row.Split('|').GetValue(4).ToString();
                                    if (unitComponent.Contains('^'))
                                    {
                                        resultUnit = unitComponent.Split('^').GetValue(0).ToString();

                                    }
                                    else
                                        resultUnit = unitComponent;

                                    reference = row.Split('|').GetValue(5).ToString();

                                    tempMessage.TESTDESC = test;
                                    tempMessage.TESTRESULT = result;
                                    tempMessage.UNIT = resultUnit;
                                    tempMessage.TESTREFERENCE = reference;
                                    ZMessageParsed(this, new ZMessageEventArgs(tempMessage));

                                }

                            }


                        }
                        else {
                            string test = "";
                            string result = "";
                            string resultUnit = "";
                            string reference = "";
                            string testComponent = received.Split('|').GetValue(2).ToString();
                            test = testComponent.Split('^').GetValue(3).ToString();
                            result = received.Split('|').GetValue(3).ToString();
                            string unitComponent = received.Split('|').GetValue(4).ToString();
                            if (unitComponent.Contains('^'))
                            {
                                resultUnit = unitComponent.Split('^').GetValue(0).ToString();

                            }
                            else
                                resultUnit = unitComponent;

                            reference = received.Split('|').GetValue(5).ToString();
                            tempMessage.TESTDESC = test;
                            tempMessage.TESTRESULT = result;
                            tempMessage.UNIT = resultUnit;
                            tempMessage.TESTREFERENCE = reference;
                            ZMessageParsed(this, new ZMessageEventArgs(tempMessage));

                        }
                        ACK();

                        break;
                    case "O":
                        string orderComponent = received.Split('|').GetValue(2).ToString().Trim();
                        if (orderComponent.Contains('^'))
                        {
                            tempMessage.SampleID = orderComponent.Split('^').GetValue(0).ToString().Trim();
                        }
                        else {
                            tempMessage.SampleID = orderComponent;
                        }


                        ACK();
                        break;
                    case "L":
                        LogWriter.WriteErrorLog("Termination Line ...");
                        ACK();
                        break;
                    default:
                        ACK();
                        break;
                }
                _buffer = _buffer + received.Replace(received[1].ToString(), "");

            }


        }

        private void Parse(string msg)
        {
            LogWriter.WriteErrorLog("Start parsing ..." );

            msg = msg.Replace(STX, "");
            msg = msg.Replace(ETX, "");
            LogWriter.WriteErrorLog("msg after cleaning : " + msg);
                Message message = new Message(msg);
                if (message.ParseMessage())
                {


                    ///Check if the message is ORU : look for OBX segment
                    if (message.SegmentList.ContainsKey("R"))
                    {
                        Segment PIDSegment = message.Segments("P").FirstOrDefault();
                        //There are results here 
                        foreach (Segment oru in message.Segments("R"))
                        {
                            ZMessage zmsg = new ZMessage();
                            zmsg.PID = PIDSegment.Fields(3).Value;
                            if (oru.Fields(3).IsComponentized)
                            {
                                LogWriter.WriteErrorLog("Components are :");
                                foreach (Component c in oru.Fields(3).ComponentList)
                                {
                                    //check if subcomp
                                    LogWriter.WriteErrorLog(" " + c.Value);
                                }
                            }
                            else
                            {
                                LogWriter.WriteErrorLog("No  it is not Componentized");
                            }
                            zmsg.TESTDESC = oru.Fields(3).Value;
                            zmsg.TESTRESULT = oru.Fields(5).Value;
                            zmsg.UNIT = oru.Fields(6).Value;
                            zmsg.TESTREFERENCE = oru.Fields(8).Value;
                            ZMessageParsed(this, new ZMessageEventArgs(zmsg));
                        }

                    }
                    else

                    {

                        LogWriter.WriteErrorLog("Message can't be parsed! ");
                    }

                


            }


        }


    }
}
