using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZServer
{
  public  interface IListener
    {
         event EventHandler<ZMessageEventArgs> ZMessageParsed ;
         event EventHandler<MessageEventArgs> MessageReceived;
         void Start();
        void Disconnect();
    }
}
