using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZServer
{
 public   interface ISerialContext
    {
         event EventHandler<ZMessageEventArgs> ZMessageParsed;
         event EventHandler<MessageEventArgs> MessageReceived;
    }
}
