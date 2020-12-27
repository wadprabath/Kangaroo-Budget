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
    public partial class FrmAdvfind : Form
    {
        Voucher vr;
        public FrmAdvfind()
        {
            InitializeComponent();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                vr = new Voucher();
                vr.advancedFindHiresEndRef(dataGridView1, dateTimePicker1, textBox1.Text,radioButton1,radioButton2,radioButton3);
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                vr = new Voucher();
                vr.advancedFindHiresFullRef(dataGridView1, dateTimePicker1, textBox2.Text, radioButton1, radioButton2,radioButton3);
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text.Length == 0)
                dataGridView1.DataSource = null;
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                vr = new Voucher();
                vr.advancedFindHiresCab(dataGridView1, dateTimePicker1, textBox3.Text, radioButton1, radioButton2,radioButton3);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0)
                dataGridView1.DataSource = null;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (textBox3.Text.Length == 0)
                dataGridView1.DataSource = null;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            dateTimePicker1.Value = DateTime.Now;
            dataGridView1.DataSource = null;
            dataGridView2.DataSource = null;
            dataGridView2.Columns.Add("", "");           
            dataGridView2.Columns[0].Width = 672;
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void FrmAdvfind_Load(object sender, EventArgs e)
        {
            radioButton1.Checked = true;
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
                MessageBox.Show("Please Enter Cab No Without 'K', Eg-: If Cab No is K901, You Should Type only 901. No need to type 'K'");
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
           
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
                MessageBox.Show("Please Enter Refernce No Without English Letters, Eg-: If Refernce No is AB1234, You Should Type only 1234. No need to type 'AB'");
            }
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                vr = new Voucher();
                vr.advancedFindInMessageTable(textBox5.Text, dateTimePicker2, dataGridView2,textBox4);
            }
           
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            if (textBox5.Text.Length == 0)            
                dataGridView2.DataSource = null;  
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (textBox4.Text.Length == 0)            
                dataGridView2.DataSource = null;
                
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                vr = new Voucher();
                vr.advancedFindInMessageTable(textBox5.Text, dateTimePicker2, dataGridView2, textBox4);
            }
        }
    }
}
