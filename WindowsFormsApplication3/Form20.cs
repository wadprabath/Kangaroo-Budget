using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication3
{
    public partial class Form20 : Form
    {
        Taxi t1;
        public Form20()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            t1 = new Taxi();
            t1.SavePaymentFroOneDayFromVouchers(textBox3,textBox1,textBox2,dateTimePicker1);
        }
    }
}
