using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZServer.Parser
{
    class HL7Parserold
    {
        public int Parse(byte[] buffer, int offset, int count)
        {
            int handledBytes = 0;
            int startPos = -1;

            int endOfBufferPos = offset + count;



            for (int currentPos = offset; currentPos < endOfBufferPos; currentPos++)
            {
                var ch = (char)buffer[currentPos];
                if (ch == (char)0x1c)
                    return handledBytes;
                handledBytes = currentPos + 1;

            }

            return handledBytes;
        }
    }
}
