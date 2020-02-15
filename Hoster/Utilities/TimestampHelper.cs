using System;

namespace Hoster.Utilities
{
    public static class TimestampHelper
    {
        public static string GetTimestamp()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmss");
        }
    }
}
