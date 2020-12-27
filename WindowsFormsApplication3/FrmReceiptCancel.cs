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
    public partial class FrmReceiptCancel : Form
    {
        public FrmReceiptCancel()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) 
            {
                Taxi t1 = new Taxi();
                t1.findReceiptForCancell(textBox1.Text,textBox2,textBox3,textBox4,textBox5,radioButton1,radioButton2,radioButton4,radioButton3,textBox7);
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            textBox1.Clear(); textBox2.Clear(); textBox3.Clear(); textBox4.Clear(); textBox5.Clear(); textBox6.Clear();
            radioButton1.Checked = false; radioButton2.Checked = false; radioButton4.Checked = false; radioButton3.Checked = false;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Taxi t1 = new Taxi();
            t1.cancellAllReceipt(textBox2, textBox3, textBox4, textBox5, textBox6,radioButton1, radioButton2, radioButton4,radioButton3,comboBox1,textBox7);
            //t1.saveCancellationInfo(textBox2, textBox3, textBox4, textBox5, textBox6, radioButton1, radioButton2, radioButton4,radioButton3);
        }
    }
}
