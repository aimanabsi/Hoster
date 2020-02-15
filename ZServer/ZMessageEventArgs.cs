using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZServer.Parser;

namespace ZServer
{
    public class ZMessageEventArgs : EventArgs
    {
        public ZMessage Message { get; private set; }
        public ZMessageEventArgs(ZMessage message)
        {
            Message = message;
        }
    }
}
