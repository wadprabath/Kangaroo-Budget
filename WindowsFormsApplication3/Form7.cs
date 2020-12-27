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
    public partial class Form7 : Form
    {
        Taxi t = new Taxi();
         
        public Form7()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            t.findReceipt(textBox1, dataGridView1);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
           textBox2.Text= t.SecettedRecipPrint(dataGridView1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            t.CancelRecipt(textBox2.Text);
        }
    }
}
