using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZServer.Parser;

namespace ZServer
{
    public class SUITClientContext : IZClientContext, IDisposable
    {
        private readonly byte[] _buffer;
        private int _bytesLeft;
        private readonly int _bufferSize;
        public event EventHandler<DisconnectedEventArgs> Disconnected;
        public event EventHandler<MessageEventArgs> MessageReceived;
        private int _wait = 3000;
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
        public SUITClientContext(EndPoint remoteEndPoint, Stream stream, Socket socket, int bufferSize)
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
                LogWriter.WriteErrorLog(this.ToString() + " : " + err.Message);
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
                if (_bytesLeft > _buffer.Length)
                {
                    LogWriter.WriteErrorLog("Too Large Message " + Encoding.UTF8.GetString(_buffer, 0, bytesRead));
                    return;
                }
                string temp = Encoding.ASCII.GetString(_buffer, 0, _bytesLeft);
                MessageReceived(this, new MessageEventArgs(temp));
                LogWriter.WriteErrorLog("Msg Rcieved : " + temp);
                Console.WriteLine("Msg Recieved : " + temp);
                //Need to Respond to the SUIT
                if (temp != "")
                {
                    Thread.Sleep(_wait);
                    Respond("[ACK]");
                }
                else {
                    Thread.Sleep(_wait);
                    Respond("[NACK]");
                }

                   
                SUITParser _parser = new SUITParser();
                _bytesLeft -= _bytesLeft;
                if (Stream != null && Stream.CanRead)
                    Stream.BeginRead(_buffer, _bytesLeft, _buffer.Length - _bytesLeft, OnReceive, null);
                else
                {
                    LogWriter.WriteErrorLog(this.ToString() + "  : Could not read any more from the socket.");
                    Disconnect(SocketError.Success);
                }

            }
            catch (Exception err)
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
        /// <summary>
        /// Disconnect from client
        /// </summary>
        /// <param name="error">error to report in the <see cref="Disconnected"/> event.</param>
        public void Disconnect(SocketError error)
        {
            // disconnect may not throw any exceptions
            try
            {
                //_sock.Disconnect(true);
                if (error == SocketError.Success)
                {
                  //  if (Stream is ReusableSocketNetworkStream)
                        Stream.Flush();
                }
                //Stream.Close();
                Disconnected(this, new DisconnectedEventArgs(error));
            }
            catch (Exception err)
            {
                LogWriter.WriteErrorLog("Disconnect threw an exception: " + err);
            }
        }

      
        public void Send(byte[] buffer)
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer");
            Send(buffer, 0, buffer.Length);
        }

        public void Send(byte[] buffer, int offset, int size)
        {

            if (offset + size > buffer.Length)
                throw new ArgumentOutOfRangeException("offset", offset, "offset + size is beyond end of buffer.");

            if (Stream != null && Stream.CanWrite)
            {
                try
                {
                    Stream.Write(buffer, offset, size);
                }
                catch (IOException)
                {

                }
            }

        }
        public void Respond(string response)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(char.ConvertFromUtf32(6));
        
            Send(buffer);
                     
        }
        private void OnMessageReceived(object sender, MessageEventArgs e)
        {
            Console.WriteLine("OnOnMessageReceived  Got message : " + e.Message);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool unmanaged)
        {
            if (unmanaged)
            {
                if (Stream != null)
                {
                    try
                    {
                        if (Stream.CanWrite)
                            Stream.Flush();

                        Cleanup();
                    }
                    catch (IOException)
                    {

                    }
                }

            }

        }

    }
}
