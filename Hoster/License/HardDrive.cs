using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Management;
namespace Hoster
{
    class HardDrive
    {
        private string model = null;
        private string type = null;
        private string serialNo = null;
        public string Model
        {
            get { return model; }
            set { model = value; }
        }
        public string Type
        {
            get { return type; }
            set { type = value; }
        }
        public string SerialNo
        {
            get { return serialNo; }
            set { serialNo = value; }
        }
        public static  string GetHDDSerial()
        {
            string hddSerial = "";
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia");

            int i = 0;
            foreach (ManagementObject wmi_HD in searcher.Get())
            {
                // get the hard drive from collection
                // using index

                // get the hardware serial no.
                if (wmi_HD["SerialNumber"] != null)
                    hddSerial = wmi_HD["SerialNumber"].ToString();

                ++i;
            }
            return hddSerial;
        }
    }

}
