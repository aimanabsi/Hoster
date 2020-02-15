using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Hoster.Views
{
    /// <summary>
    /// Interaction logic for ListenerUI.xaml
    /// </summary>
    public partial class ListenerUI : UserControl
    {
        public ListenerUI()
        {
            InitializeComponent();
        }

        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            ExpenderColumn.Width = new GridLength(350);
        }

        private void Expander_Collapsed(object sender, RoutedEventArgs e)
        {
            ExpenderColumn.Width = new GridLength(40);
        }
    }
}
