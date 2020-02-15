using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Hoster
{
    class License
    {
        private string serialKey;
        private string _publicKey;
        private static string _privateKey;
        private static UnicodeEncoding _encoder = new UnicodeEncoding();
        private string channelsCount;
        public License(string _serialKey){
            this.serialKey = _serialKey;
            _publicKey = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory+"\\public.key");
            _privateKey = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "\\private.key");
        }

        public bool ValidateLicense()
        {
            bool validated = false;
            if (serialKey.ToLower().Trim() == HardDrive.GetHDDSerial().ToLower().Trim())
                return true;
            else
                return false;
           

            string hddSerial = HardDrive.GetHDDSerial();
            string decryptedSerial = Decrypt(serialKey);
            string[] serialData = decryptedSerial.Split('-');
            if (serialData.Count() == 0)
                return false;
            string keyHDDSerial = serialData[0];
            if (hddSerial == keyHDDSerial)
            {
                channelsCount = serialData[1];
                return true;
            }
            return validated;

        }
        public static bool IsLicensed(string serialKey)
        {
            bool licensed = false;



            return licensed;
        }

        public string GetPCName()
        {
            string name="";
            return name;

        }

        public string GetMACAddress() {
            string mac = "";

            return mac;
        }

        public string CountLicensedChannels() {
            return "2";
        }


      

        public  string RSA()
        {
            var rsa = new RSACryptoServiceProvider();
            _privateKey = rsa.ToXmlString(true);
            _publicKey = rsa.ToXmlString(false);
            var text = "Test1";
            return _privateKey;
            //var enc = Encrypt(text);
            //MessageBox.Show("public : " + _publicKey);
            //var dec = Decrypt(enc);
            //Console.WriteLine("RSA // Decrypted Text: " + dec);
        }

        public  string Decrypt(string data)
        {
            var rsa = new RSACryptoServiceProvider();
            var dataArray = data.Split(new char[] { ',' });
            byte[] dataByte = new byte[dataArray.Length];
            for (int i = 0; i < dataArray.Length; i++)
            {
                dataByte[i] = Convert.ToByte(dataArray[i]);
            }

            rsa.FromXmlString(_privateKey);
            var decryptedByte = rsa.Decrypt(dataByte, false);
            return _encoder.GetString(decryptedByte);
        }

        public  string Encrypt(string data)
        {
            var rsa = new RSACryptoServiceProvider();
            var dataToEncrypt = _encoder.GetBytes(data);
            var encryptedByteArray = rsa.Encrypt(dataToEncrypt, false).ToArray();
            var length = encryptedByteArray.Count();
            var item = 0;
            var sb = new StringBuilder();
            foreach (var x in encryptedByteArray)
            {
                item++;
                sb.Append(x);

                if (item < length)
                    sb.Append(",");
            }

            return sb.ToString();
        }
    }
}
