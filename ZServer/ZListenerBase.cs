using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZServer
{
 public abstract   class ZListenerBase
    {



        protected void ThroughException(Exception err)
        {
            ExceptionHandler handler = ExceptionThrown;

            if (handler != null)
            {
                handler(this, err);
            }
            else
            {
                LogWriter.WriteErrorLog(this.ToString()+" : "+err.Message);
            }
        }

        /// <summary>
        /// Catch exceptions not handled by the listener.
        /// </summary>
        /// <remarks>
        /// Exceptions will be thrown during debug mode if this event is not used,
        /// exceptions will be printed to console and suppressed during release mode.
        /// </remarks>
        public event ExceptionHandler ExceptionThrown;

    }

}
