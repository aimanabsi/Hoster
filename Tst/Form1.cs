using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZServer;

namespace Tst
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void connectionAccepted(object sender ,ClientAcceptedEventArgs args)
        {
            label1.Invoke( new Action(()=> label1.Text= "Connected to " + args.Socket.RemoteEndPoint));

        }

        private void connectionAborted(object sender, DisconnectedEventArgs args)
        {
            label1.Invoke(new Action(() => label1.Text = "Connection aborted due to : " +args.Error));

        }

        private void MessageRecieved(object sender , MessageEventArgs args)
        {
            textBox1.Invoke(new Action(()=>textBox1.Text=textBox1.Text+"\n"+args.Message));
        }
        private void button1_Click(object sender, EventArgs e)
        {
            IPEndPoint end = new IPEndPoint(IPAddress.Parse("192.168.1.81"), 8000);
            ZListener lsnr = new ZListener(end);
            lsnr.Accepted += connectionAccepted;
            lsnr.MessageReceived += MessageRecieved;
            lsnr.Disconnected += connectionAborted;


            lsnr.Start(1);
            button1.Enabled = false;

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
