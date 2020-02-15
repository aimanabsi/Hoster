using SerialPortLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZServer;

namespace Tst
{
    public partial class RS232 : Form
    {
        SerialPortInput serialPort = new SerialPortInput();
        const int cihazId = 2;
        private string _buffer = "";
        static string soh = char.ConvertFromUtf32(1);
        static string stx = char.ConvertFromUtf32(2);
        static string etx = char.ConvertFromUtf32(3);
        static string eot = char.ConvertFromUtf32(4);
        static string enq = char.ConvertFromUtf32(5);
        static string ack = char.ConvertFromUtf32(6);
        static string nack = char.ConvertFromUtf32(21);
        static string etb = char.ConvertFromUtf32(23);
        static string lf = char.ConvertFromUtf32(10);
        static string cr = char.ConvertFromUtf32(13);
        const int wait = 3000;
        static EventWaitHandle wh;
        public RS232()
        {
            InitializeComponent();
            serialPort.ConnectionStatusChanged += delegate (object sender, ConnectionStatusChangedEventArgs args)
            {
                Console.WriteLine("Connected = {0}", args.Connected);   
             };

            serialPort.MessageReceived += delegate (object sender, MessageReceivedEventArgs args)
            {
                wh = new AutoResetEvent(false);
                Console.WriteLine("Received message: {0}", BitConverter.ToString(args.Data));
                textBox1.Text = "MsgRcvd : " + Encoding.ASCII.GetString(args.Data);
                LogWriter.WriteErrorLog("Msg Recieved : " + Encoding.ASCII.GetString(args.Data));
                string msg = "[ACK]";
                byte[] bytes = Encoding.UTF8.GetBytes(msg);
                try
                {
                    // serialPort.SendMessage(bytes);
                    Eval2(Encoding.ASCII.GetString(args.Data));
                }
                catch (Exception ex)
                {
                    LogWriter.WriteErrorLog("Error : " + ex.Message);
                }
                
            };
        }

        private void button1old_Click(object sender, EventArgs e)
        {
            String portName = comboBox1.Text;

            if (portName != "")
            {
                button1.Enabled = false;
                comboBox1.Enabled = false;
                serialPort.SetPort(portName, 19200);
               // serialPort.
               try
                {
                    serialPort.Connect();
                }
                catch(Exception ex)
                {
                    LogWriter.WriteErrorLog(" Error : " + ex.Message);
                }
                // Connect the serial port
          
                string msg = "Hey";
                byte[] bytes = Encoding.Unicode.GetBytes(msg);
          //      serialPort.SendMessage(bytes);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String portName = comboBox1.Text;
            SerialListener listener;
            if (portName != "")
            {
                button1.Enabled = false;
                comboBox1.Enabled = false;
              
                // serialPort.
                try
                {
                    listener = new SerialListener(portName,3000, 19200, System.IO.Ports.StopBits.One, System.IO.Ports.Parity.None,2);
                    listener.Start();
                }
                catch (Exception ex)
                {
                    LogWriter.WriteErrorLog(" Error : " + ex.Message);
                }
                // Connect the serial port

                string msg = "Hey";
                byte[] bytes = Encoding.Unicode.GetBytes(msg);
                //      serialPort.SendMessage(bytes);
            }
        }


        private void RS232_Load(object sender, EventArgs e)
        {

            foreach (string s in System.IO.Ports.SerialPort.GetPortNames())
            {
                comboBox1.Items.Add(s);
            }
        }

        private void RS232_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
            System.Environment.Exit(1);
        }

        private void RS232_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
            System.Environment.Exit(1);
        }

        private  void ACK()
        {
            Thread.Sleep(wait);
            byte[] _bts= Encoding.ASCII.GetBytes(ack);
            serialPort.SendMessage(_bts);
        }

        private  void Eval(string str)
        {
            string received;

          //  Thread thread;

            received = null;

            received = str;
        

            if (received == ack)
            {
                wh.Set();
                LogWriter.WriteErrorLog("=> :  ACK ");

            }
            else if (received == enq)
            {
                ACK();
                LogWriter.WriteErrorLog("=> :  ENQ ");
            }

            else if (received == eot)
            {
                LogWriter.WriteErrorLog("=> :  EOT ");
            }
            else
            {
                ACK();
                if (received.Length >3) {
                    LogWriter.WriteErrorLog("record type : " + received[2].ToString());
                }
                LogWriter.WriteErrorLog("Inside the else and the recieved string is : " + received);
              //  string recordType = received[2].ToStrin
                //    case "H":g();

                //switch (recordType)
                //{
                //        ACK();
                //        break;
                //    case "Q":
                //        string[] fields = received.Split('|');
                //        string[] comps = fields[2].Split('^');
                //        string barcode = comps[1];

                //        //    _barcode = Convert.ToInt32(barcode);

                //        //     orderFlag = true;
                //        ACK();
                //        break;
                //    case "R":
                //        string test = "";
                //        string result = "";
                //        string resultUnit = "";
                //        test = received.Split('|').GetValue(2).ToString();
                //        result = received.Split('|').GetValue(3).ToString();
                //        resultUnit = received.Split('|').GetValue(4).ToString();
                //        LogWriter.WriteErrorLog("test : " + test + "   result : " + result + "  resultunit  : " + resultUnit);
                //        //    Update(_barcode, test, result, resultUnit);
                //        ACK();
                //        break;
                //    case "O":
                //        //   _barcode = Convert.ToInt32(received.Split('|').GetValue(2).ToString());
                //        ACK();
                //        break;
                //    case "L":
                //        ACK();
                //        break;
                //    default:
                //        ACK();
                //        break;
//                }

            }

        }
        private void Eval2(string str)
        {
            string received;

            //  Thread thread;

            received = null;

            received = str;


            if (received == ack)
            {
                wh.Set();

            }
            else if (received == enq)
            {
                ACK();
             
            }

            else if (received == eot)
            {
                LogWriter.WriteErrorLog("Whole message is :  " + _buffer);

                _buffer = "";
                ACK();
            }
            else
            {
                ACK();
                if (received.Length > 3)
                {
                  //  LogWriter.WriteErrorLog("record type : " + received[2].ToString());
                }
             //   LogWriter.WriteErrorLog("Inside the else and the recieved string is : " + received);
                 string recordType = received[2].ToString();
                //   
                switch (recordType)
                {
                    case "H":
                        ACK();
                        _buffer = "";
                break;
                    case "Q":
                        string[] fields = received.Split('|');
                string[] comps = fields[2].Split('^');
                string barcode = comps[1];

                //    _barcode = Convert.ToInt32(barcode);

                //     orderFlag = true;
                ACK();
                break;
                    case "R":
                        string test = "";
                string result = "";
                string resultUnit = "";
                test = received.Split('|').GetValue(2).ToString();
                result = received.Split('|').GetValue(3).ToString();
                resultUnit = received.Split('|').GetValue(4).ToString();
               // LogWriter.WriteErrorLog("test : " + test + "   result : " + result + "  resultunit  : " + resultUnit);
                //    Update(_barcode, test, result, resultUnit);
                ACK();
                break;
                    case "O":
                        //   _barcode = Convert.ToInt32(received.Split('|').GetValue(2).ToString());
                        ACK();
                break;
                    case "L":
                        ACK();
                break;
                default:
                        ACK();
                break;
            }
                _buffer = _buffer + received;

            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string pcName = System.Environment.MachineName;

            MessageBox.Show("Device name is : " + pcName);
            //string str = textBox1.Text;
            //ZServer.Parser.ASTM1394.Message msg=new ZServer.Parser.ASTM1394.Message(str);
            //textBox2.Text = "msg has " + msg.Records.Count + " which are : \n";
            //if (msg.Records.Count>0)
            //{
            //    foreach (string s in msg.Records.Keys) {
            //        textBox2.Text = textBox2.Text + "" + s + "\n";
            //    }
              
            //}

        }
    }
}
