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
using ZServer.Parser.astm;

namespace ZServer
{
    public class ASTMClientContext : IZClientContext, IDisposable
    {
        private readonly byte[] _buffer;
        private int _bytesLeft;
        private readonly int _bufferSize;
        public event EventHandler<DisconnectedEventArgs> Disconnected;
        public event EventHandler<MessageEventArgs> MessageReceived;
        public event EventHandler<ZMessageEventArgs> ZMessageParsed;
        private int _wait;
        private bool _autoRespond;
        /// <summary>
		/// Context have been started (a new client have connected)
		/// </summary>
    	public event EventHandler Started = delegate { };

        /// <summary>
		/// This context have been cleaned, which means that it can be reused.
		/// </summary>
    	public event EventHandler Cleaned = delegate { };
        private Stream _stream;
        internal Stream Stream
        {
            get { return _stream; }
            private set { _stream = value; }

        }
        public ASTMClientContext(EndPoint remoteEndPoint, Stream stream, Socket socket, int bufferSize, bool autoResond, int responseDelay)
        {
            _wait = responseDelay;
            _autoRespond = autoResond;
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
                string temp = Encoding.UTF8.GetString(_buffer, 0, _bytesLeft);
               
                LogWriter.WriteErrorLog("Msg Rcieved : " + temp);
                Console.WriteLine("Msg Recieved : " + temp);
                ///ASTM has three types of messages 
                ///  1- Setting Status
                ///     << ENQ
                ///     >> ACK
                ///  2- Forwarding Status
                ///  3- End Status
                ///  
               


                //Need to Respond to the SUIT
                if (temp != "")
                {
                    if (temp == ASTMParser.ENQ  )
                    {
                        //   Thread.Sleep(_wait);
                        LogWriter.WriteErrorLog("Setting status <<ENQ \r\n >>ACK");
                        Respond(ASTMParser.ACK);
                       
                    }
                    else if (temp==ASTMParser.EOT)
                    {
                        
                        LogWriter.WriteErrorLog("Ending Status EOF");
                        ASTMParser.ResetBuffer();
                    }
                    else
                    {
                        LogWriter.WriteErrorLog("Forwarding Status");
                        if (ASTMParser.AddToBuffer(temp))
                        {
                            LogWriter.WriteErrorLog("A message frame completed !");
                            LogWriter.WriteErrorLog("Buffer : " + ASTMParser.Buffer);
                            MessageReceived(this, new MessageEventArgs(temp));

                            Message message = new Message(ASTMParser.Buffer);
                            if (1 == 1)
                            {
                                Thread.Sleep(_wait);
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
                                    }
                                    string response = ASTMParser.ACK;
                                    Respond(response);

                                }
                                else
                                {
                                    Respond(ASTMParser.ACK);
                                    LogWriter.WriteErrorLog("Repond with : ACK");
                                }

                            }

                        }
                        else {
                            Respond(ASTMParser.ACK);
                            LogWriter.WriteErrorLog("Repond with : ACK");
                        }
                      
                    }


                }
                else
                {
                    if (_autoRespond)
                    {
                        Thread.Sleep(_wait);
                        Respond(ASTMParser.NACK);
                        LogWriter.WriteErrorLog("Repond with : NACK");
                    }
                }


             
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
                Disconnect(SocketError.Success);
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
          
            byte[] buffer = Encoding.ASCII.GetBytes(response);

            LogWriter.WriteErrorLog("Repond with : " + response);
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
 