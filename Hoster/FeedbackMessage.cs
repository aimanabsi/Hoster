using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hoster
{
    public class FeedbackMessage
    {
        private string _message;
        private int _type;
        public const int NONE = 0;
        public const int SUCCESS = 1;
        public const int ALERT = 2;
        public const int ERROR = 3;
        public string Message { get { return _message; } set { _message = value; } }
        public int Type { get { return _type; } set { _type = value; } }

        public FeedbackMessage(string message, int type)
        {
            this._message = message;
            this._type = type;

        }
        public static FeedbackMessage CreatSuccessMessage(string message)
        {
            return new FeedbackMessage(message, SUCCESS);
        }

        public static FeedbackMessage CreatErrorMessage(string message)
        {
            return new FeedbackMessage(message, ERROR);
        }
        /// <summary>
        /// - create an alert feedback message
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static FeedbackMessage CreatAlertMessage(string message)
        {
            return new FeedbackMessage(message, ALERT);
        }

        public static FeedbackMessage CreatEmptyMessage()
        {
            return new FeedbackMessage("", NONE);
        }
    }
}
