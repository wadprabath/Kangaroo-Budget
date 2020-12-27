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
    public partial class FrmCabList : Form
    {
        Taxi t1;
        public FrmCabList()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            t1 = new Taxi();
            t1.AddNewCab(textBox1,textBox2,dateTimePicker1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            t1 = new Taxi();
            t1.withdrawCab(textBox1, textBox2,dateTimePicker1);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            t1 = new Taxi();
            t1.ClearCablist(textBox1, textBox2,dateTimePicker1);

        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
           
            if (e.KeyValue == 13) 
            {
              t1 = new Taxi();
              t1.FindCabList(textBox1, textBox2);
              textBox2.Focus();
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13) 
            {
              t1 = new Taxi();
              t1.FindCabList(textBox1, textBox2);
              textBox2.Focus();
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            FrmCabListPrint frmcabPrint = new FrmCabListPrint();
            frmcabPrint.Show();
        }

      
        
    }
}
