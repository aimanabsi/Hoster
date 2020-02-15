using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ZServer
{
  public  interface IZClientContext
    {

        ///<summary>
        ///Disconnect from Client
        /// </summary>
        void Disconnect(SocketError err);
        /// <summary>
        /// Close the stream
        /// </summary>
        void Close();
        /// <summary>
        /// Send to the stream
        /// </summary>
        /// <param name="buffer"></param>
        void Send(byte[] buffer);
        /// <summary>
        /// the context has been disconnected
        /// </summary>
        event EventHandler<DisconnectedEventArgs> Disconnected;

        event EventHandler<MessageEventArgs> MessageReceived;
        event EventHandler<ZMessageEventArgs> ZMessageParsed;

    }
    public class DisconnectedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets reason to why client disconnected.
        /// </summary>
        public SocketError Error { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DisconnectedEventArgs"/> class.
        /// </summary>
        /// <param name="error">Reason to disconnection.</param>
        public DisconnectedEventArgs(SocketError error)
        {
           // Check.Require(error, "error");

            Error = error;
        }
    }

   
}
