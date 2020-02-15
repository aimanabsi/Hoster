using MahApps.Metro.IconPacks;
using System.Collections.Generic;

namespace Hoster
{
    public static class SettingsViewManager
    {
        // List of all applications
        public static List<SettingsViewInfo> List => new List<SettingsViewInfo>
        {
            // General
            
            new SettingsViewInfo(Name.General, new PackIconModern{ Kind = PackIconModernKind.Box }, Group.General),
            new SettingsViewInfo(Name.Window, new PackIconMaterial { Kind = PackIconMaterialKind.Application }, Group.General),
            new SettingsViewInfo(Name.Appearance, new PackIconMaterial { Kind = PackIconMaterialKind.AutoFix }, Group.General),
            new SettingsViewInfo(Name.Language, new PackIconMaterial { Kind = PackIconMaterialKind.Flag }, Group.General),
            new SettingsViewInfo(Name.HotKeys, new PackIconOcticons { Kind = PackIconOcticonsKind.Keyboard }, Group.General),
            new SettingsViewInfo(Name.Autostart, new PackIconMaterial { Kind = PackIconMaterialKind.Power }, Group.General),
            new SettingsViewInfo(Name.Update, new PackIconMaterial { Kind = PackIconMaterialKind.Download }, Group.General),
            new SettingsViewInfo(Name.ImportExport, new PackIconMaterial { Kind = PackIconMaterialKind.Import }, Group.General),
            new SettingsViewInfo(Name.Settings, new PackIconMaterialLight { Kind = PackIconMaterialLightKind.Cog }, Group.General),

            // Applications
            new SettingsViewInfo(Name.Dashboard, ApplicationViewManager.GetIconByName(ApplicationViewManager.Name.Dashboard), Group.Applications),
            new SettingsViewInfo(Name.Listeners, ApplicationViewManager.GetIconByName(ApplicationViewManager.Name.Listeners), Group.Applications),
         
           
        };

        public static string TranslateName(Name name, Group group)
        {
            switch (name)
            {
                case Name.Dashboard:
                    return"Dashboard";
                case Name.Listeners:
                    return "Channels";
                default:
                    return "Translation of name not found";
            }
        }

        public static string TranslateGroup(Group group)
        {
            switch (group)
            {
               
                default:
                    return "Translation of group not found!";
            }
        }

        public enum Name
        {
            General,
            Dashboard,
            Listeners,
            Window,
            Appearance,
            Language,
            HotKeys,
            Autostart,
            Update,
            ImportExport,
            Settings,
            IPScanner,
            PortScanner,
            Ping,
            Traceroute,
            DNSLookup,
            RemoteDesktop,
            PuTTY,
            TightVNC,
            SNMP,
            WakeOnLAN,
            HTTPHeaders,
            Whois
        }

        public enum Group
        {
            General,
            Applications
        }
    }
}
