using System.Reflection;
using System.Security.Principal;
using System.IO;
using System;

namespace Hoster.Models.Settings
{
    public static class ConfigurationManager
    {
       

        public static void Detect()
        {
            Console.WriteLine("Inside THE ConfigurationManager.Detect()");
            var applicationLocation = Assembly.GetExecutingAssembly().Location;

            Current = new ConfigurationInfo
            {
                IsAdmin = new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator),
                ExecutionPath = Path.GetDirectoryName(applicationLocation),
                ApplicationFullName = applicationLocation,
                ApplicationName = Path.GetFileNameWithoutExtension(Path.GetFileName(applicationLocation))
            };
        }
        public static ConfigurationInfo Current { get; set; }
    }
}
