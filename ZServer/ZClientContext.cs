using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ZServer.Parser;

namespace ZServer
{
   public class ZClientContext : IZClientContext, IDisposable
    {
        private readonly byte[] _buffer;
        private int _bytesLeft;
        private readonly int _bufferSize;
        public event EventHandler<DisconnectedEventArgs> Disconnected;
        public event EventHandler<MessageEventArgs> MessageReceived;
        /// <summary>
		/// Context have been started (a new client have connected)
		/// </summary>
    	public event EventHandler Started = delegate { };

        /// <summary>
		/// This context have been cleaned, which means that it can be reused.
		/// </summary>
    	public event EventHandler Cleaned = delegate { };
        public event EventHandler<ZMessageEventArgs> ZMessageParsed;

        private Stream _stream;
        internal Stream Stream
        {
            get { return _stream; }
            private set { _stream = value; }
            
        }
       public ZClientContext(EndPoint remoteEndPoint,Stream stream, Socket socket,int bufferSize)
        {
            _bufferSize = bufferSize;
            _buffer = new byte[_bufferSize];
            _stream = stream;
            Start();

        }
        public virtual void Start()
        {
            try
            {
                _stream.BeginRead(_buffer, 0, _bufferSize, OnReceive, null);
            }
            catch (IOException err)
            {
                LogWriter.WriteErrorLog(this.ToString()+" : "+err.Message);
            }

            Started(this, EventArgs.Empty);

        }
        private void OnReceive(IAsyncResult ar)
        {
            try
            {
                int bytesRead = Stream.EndRead(ar);
                if (bytesRead == 0)
                {
                    Disconnect(SocketError.ConnectionReset);
                    return;
                }
                _bytesLeft += bytesRead;
                if (_bytesLeft > _buffer.Length) {
                    LogWriter.WriteErrorLog("Too Large Message "+ Encoding.UTF8.GetString(_buffer, 0, bytesRead));
                    return;
                }

                string temp = Encoding.ASCII.GetString(_buffer, 0, _bytesLeft);
                MessageReceived(this, new MessageEventArgs(temp));
                LogWriter.WriteErrorLog("Msg Rcieved : " + temp);
                Console.WriteLine("Msg Recieved : " + temp);
                AzizMsgParser _parser = new AzizMsgParser();
                int offset = _parser.Parse(_buffer, 0, _bytesLeft);

                while (offset != 0 && _bytesLeft - offset > 0) {
                    temp = Encoding.ASCII.GetString(_buffer, offset, _bytesLeft -offset);
                    LogWriter.WriteErrorLog("Processing : " + temp);
                    offset= _parser.Parse(_buffer, 0, _bytesLeft);
                    if (Stream == null)
                        return;
                }

                if (Stream != null && Stream.CanRead)
                    Stream.BeginRead(_buffer, _bytesLeft, _buffer.Length - _bytesLeft, OnReceive, null);
                else
                {
                    LogWriter.WriteErrorLog(this.ToString()+"  : Could not read any more from the socket.");
                    Disconnect(SocketError.Success);
                }

            }
            catch(Exception err)
            {
                LogWriter.WriteErrorLog(this.ToString() + " : " + err.Message);
            }
        }
        public void Close()
        {
            throw new NotImplementedException();
        }
        public virtual void Cleanup()
        {
            if (Stream == null)
                return;

            Stream.Dispose();
            Stream = null;
            _bytesLeft = 0;
            Cleaned(this, EventArgs.Empty);
        }
        public void Disconnect(SocketError err)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Send(byte[] buffer)
        {
            throw new NotImplementedException();
        }

       private void OnMessageReceived(object sender ,MessageEventArgs e)
        {
            Console.WriteLine("OnOnMessageReceived  Got message : " + e.Message);
        }
    }
}
