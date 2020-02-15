using System;
using System.Collections.Generic;
using System.Windows.Controls;
using MahApps.Metro.IconPacks;
using Hoster.Properties;

namespace Hoster
{
    public static class ApplicationViewManager
    {
        // List of all applications
        public static List<ApplicationViewInfo> GetList()
        {
            var list = new List<ApplicationViewInfo>();

            foreach (Name name in Enum.GetValues(typeof(Name)))
            {
                if (name != Name.None)
                    list.Add(new ApplicationViewInfo(name));
            }

            return list;
        }

        public static string GetTranslatedNameByName(Name name)
        {
            switch (name)
            {
                case Name.Listeners:
                    return "Channels";
                case Name.Dashboard:
                    return "Dashoard";
                default:
                    return "Hoster";
            }
        }

        public static Canvas GetIconByName(Name name)
        {
            var canvas = new Canvas();

            switch (name)
            {
                case Name.Dashboard:
                    canvas.Children.Add(new PackIconOcticons { Kind = PackIconOcticonsKind.Dashboard });
                    break;
                case Name.Listeners:
                    canvas.Children.Add(new PackIconMaterial { Kind = PackIconMaterialKind.Sitemap });
                    break;
               default:
                    canvas.Children.Add(new PackIconModern { Kind = PackIconModernKind.SmileyFrown });
                    break;
            }
            return canvas;

        }

        public enum Name
        {
            Listeners,
            Dashboard,
            None          
        }
    }
}
