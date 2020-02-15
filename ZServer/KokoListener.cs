using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZServer
{
    public class KokoListener : IListener
    {
        public event EventHandler<ZMessageEventArgs> ZMessageParsed;
        public event EventHandler<MessageEventArgs> MessageReceived;

        public void Disconnect()
        {
            throw new NotImplementedException();
        }

        public void Start()
        {
            throw new NotImplementedException();
        }
    }
}
