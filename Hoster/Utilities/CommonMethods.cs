using System.Windows;

namespace Hoster.Utilities
{
    public static class CommonMethods
    {
        public static void SetClipboard(string text)
        {
            Clipboard.SetDataObject(text, true);
        }
    }
}
