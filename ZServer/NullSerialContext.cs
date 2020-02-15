using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZServer
{
    public class NullSerialContext : ISerialContext
    {
        public event EventHandler<ZMessageEventArgs> ZMessageParsed;
        public event EventHandler<MessageEventArgs> MessageReceived;
    }
}
