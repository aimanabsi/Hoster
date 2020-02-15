using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Shapes;

namespace Hoster
{
    /// <summary>
    /// Interaction logic for Tst.xaml
    /// </summary>
    public partial class Tst : Window
    {
        public Tst()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(Guid.NewGuid().ToString("N"));


            //IDataAccess _db = new MSAccessDataAccess(GlobalStaticConfiguration.GetDBConnectionString());
            //DataTable dt = _db.GetListeners();
            //foreach(DataRow _dr in dt.Rows)
            //{
            //    textBlock.Text = "\n" + _dr["device_id"].ToString();
            //}
        }
    }
}
