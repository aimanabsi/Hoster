using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ZServer
{
  public  class ZListener:ZListenerBase,IListener
    {
        /// <summary>
        /// - THis class will handle comming connections 
        /// </summary>
        public event EventHandler<ClientAcceptedEventArgs> Accepted = delegate { };
        public event EventHandler<MessageEventArgs> MessageReceived = delegate { };
        public event EventHandler<DisconnectedEventArgs> Disconnected = delegate { };
        public event EventHandler<ZMessageEventArgs> ZMessageParsed = delegate { };
        private bool _autoRespond;
        int _protocolType;
        int _responseDelay;
        private TcpListener listener;
        private IPEndPoint endPoint;
        private bool _singleClient = false;
        public ZListener(IPEndPoint endPoint,int protocolType=1,bool autoRespond=true,int responseDelay=0)
        {
            this.endPoint = endPoint;
            _protocolType = protocolType;
            _autoRespond = autoRespond;
            _responseDelay = responseDelay;
            listener = new TcpListener(endPoint);
           
        }

        public void Start(int backlog) {
            listener.Start(backlog);
            listener.BeginAcceptSocket(OnAccept,null);
        }
        private void RetryBeginAccept()
        {
            try
            {
                LogWriter.WriteErrorLog("Trying to accept connection again ..");
                listener.BeginAcceptSocket(OnAccept, null);
                
            }
            catch(Exception err)
            {
                ThroughException(err);
            }
        }
        private void OnAccept(IAsyncResult ar) {
            if (!_singleClient) {
                bool isbeginAcceptCalled = false;
                try
                {
                    Socket socket = listener.EndAcceptSocket(ar);
                    isbeginAcceptCalled = true;
                    _singleClient = true;
                    if (!OnAcceptingSocket(socket))
                    {
                        socket.Disconnect(true);  // Done in finalizer
                        return;
                    }
                    Console.WriteLine("Connection has been Accepted from : " + socket.RemoteEndPoint);
                    LogWriter.WriteErrorLog("Connection has been Accepted from : " + socket.RemoteEndPoint);
                    ZClientContextFactory _contextFactory = new ZClientContextFactory();
                    IZClientContext clientContext = _contextFactory.CreateContext(socket.RemoteEndPoint, new NetworkStream(socket), socket, 8024,_protocolType,_autoRespond,_responseDelay);
                    clientContext.MessageReceived += OnMessageReceived;
                    clientContext.Disconnected += OnClientDisconnected;
                    clientContext.ZMessageParsed += OnZMessageParsed;

                    if (clientContext == null)
                        socket.Disconnect(true);
                }
                catch (Exception err)
                {
                    ThroughException(err);
                    if (!isbeginAcceptCalled)
                    {
                        RetryBeginAccept();
                    }
                }

            }


        }
        protected  bool OnAcceptingSocket(Socket socket)
        {
            
            ClientAcceptedEventArgs args = new ClientAcceptedEventArgs(socket);
            Accepted(this, args);
            return !args.Revoked;
        }

        public void OnMessageReceived(object sender,MessageEventArgs args)
        {
            MessageReceived(this, args);
        }

        public void OnZMessageParsed(object sender, ZMessageEventArgs args)
        {
            ZMessageParsed(this, args);
        }
        public void OnClientDisconnected(object sender, DisconnectedEventArgs args)
        {
            LogWriter.WriteErrorLog("Client disconnected du to : " + args.Error);
            _singleClient = false;
            Disconnected(this, args);
            RetryBeginAccept();
           
        }

        public void Start()
        {
            Start(3);
        }

        public void Disconnect()
        {
            
        }
    }
}
