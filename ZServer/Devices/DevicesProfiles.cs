using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZServer
{
    /// <summary>
    /// this class contain invariable profile names 
    ///  company_device_model_protocol
    /// </summary>
 public   class DevicesProfiles
    {
        public const string NIHONKOHDEN_CELLTAC_ES_ASTM = "NihonKohden_Celltac_G_ASTM";
        public const string ROCHE_COBAS_411E_ASTM = "ROCHE_COBAS_411E_ASTM";

       
    }
    public enum SupportedDevices
    {
        NihonKhoden_Celltac_g=1,
        Roche_Cobas_411e,
    }
}
