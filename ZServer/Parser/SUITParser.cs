using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZServer.Parser
{
    public class SUITParser
    {

        public void Parse(String text)
        {
            int count = 0;
            char[] delimiter = { '|' };

            string temp = text.Trim('|');
            var tokens = temp.Split(delimiter, StringSplitOptions.None);

            foreach (var item in tokens)
            {
                //Field(count, item);
                if (item == "MSH")
                {
                    // Treat the special case "MSH" - the delimiter after the segment name counts as first field
                    ++count;
                }
                ++count;
            }
        }

        public static bool isSUITMessage(String text){
            bool _suit = false;

            return _suit;
            }
    }
}
