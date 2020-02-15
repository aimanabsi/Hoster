using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZServer.Parser
{
    interface IZParser
    {
        /// <summary>
        /// add the message to the buffer message stack so protocol can parse it as a whole
        /// </summary>
        /// <param name="message">the message segment </param>
        /// <returns> return true if the message buffer structure completed  else return false </returns>
        bool AddToBuffer(string message);

        /// <summary>
        /// Parse the message stack buffer
        /// </summary>
        /// <returns>the parsed field as zmessage object </returns>
        List<ZMessage> Parse();
    }
}
