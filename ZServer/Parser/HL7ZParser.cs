using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZServer.Parser.HL7;
namespace ZServer.Parser
{
    public class HL7ZParser : IZParser
    {
        private string _buffer;
        const string HDR_DLM = "0x0B";//Header ddelimiter
        const string TRLR_DLM = "0x1C 0x0d"; //trailer delimiter
        public string AllDelimiters { get; private set; } = @"|^~\&";
        public char FieldDelimiter { get; set; } = '|'; // \F\
        public char ComponentDelimiter { get; set; } = '^'; // \S\
        public char RepeatDelimiter { get; set; } = '~';  // \R\
        public char EscapeCharacter { get; set; } = '\\'; // \E\
        public char SubComponentDelimiter { get; set; } = '&'; // \T\
        public string SegmentDelimiter { get; set; } = "\r";
        public string PresentButNull { get; set; } = "\"\"";
        public string Buffer { get => _buffer; set => _buffer = value; }
        public bool AddToBuffer(string message)
        {
            if (message.StartsWith(HDR_DLM)) {
                ResetBuffer();
            }

            _buffer += message;
            if (_buffer.EndsWith(TRLR_DLM))
                return false;
            return true;
        }

        public ZMessage ParseBuffer()
        {
            ZMessage zmessage = new ZMessage();
           

            return zmessage;
            
        }

     
        private void ResetBuffer()
        {
            _buffer = "";
        }

       public List<ZMessage> Parse()
        {
            List<ZMessage> zmsgs = new List<ZMessage>();
            ////get the message 
            Message hl7Msg = new Message(_buffer);
            if (hl7Msg.ParseMessage())
            {
                LogWriter.WriteErrorLog("The buffer is parsed successfully");
            }
            else
                LogWriter.WriteErrorLog("Didnt parsed successfully");



            return zmsgs;
        }
    }
}
