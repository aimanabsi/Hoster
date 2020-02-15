using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tst
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.Text = Parse(textBox1.Text);
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }


        private string Parse(string msg)
        {
            string _parsed = "";
            char[] _recordDelimitor = "<CR>".ToArray();
            // - parsed the records 
            string[] _records = msg.Split(_recordDelimitor);
            // - for Each record parse its fields 
            // 
            int count = 1;
            foreach(string _record in _records)
            {
                _parsed += "Record " + count + " :  " + _record+"\n";
                count++;
            }
            return _parsed;
        }
    }
}
