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
    public partial class FrmRefund : Form
    {
        Taxi t1;
        public FrmRefund()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                t1 = new Taxi();
                t1.viewsimDeposit(textBox1.Text, textBox2.Text, textBox1, textBox2,dateTimePicker1, textBox3, textBox4, textBox5, textBox6);
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                t1 = new Taxi();
                t1.viewsimDeposit(textBox1.Text, textBox2.Text, textBox1, textBox2,dateTimePicker1, textBox3, textBox4, textBox5, textBox6);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            t1 = new Taxi();
            t1.refundSimDeposit(textBox2.Text);
        }
    }
}
