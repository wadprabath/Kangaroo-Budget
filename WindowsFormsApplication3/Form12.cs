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
    public partial class Form12 : Form
    {
        Taxi t1;
        ReportsPrint rp;
        NewReceiptNumber nrec;
        public Form12()
        {
            InitializeComponent();
        }

        private void Form12_Load(object sender, EventArgs e)
        {
            dataGridView1.Rows.Add(20);
        }

       
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            t1 = new Taxi();
            t1.clearFreeDayForm(dataGridView1, textBox1, textBox2, textBox3, toolStripButton1, textBox4);
            dataGridView1.Rows.Add(20);
            nrec = new NewReceiptNumber();
            textBox3.Text = nrec.getPromotionRecNo();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            t1 = new Taxi();
            t1.saveFreeWorkingDays(dataGridView1, textBox1, textBox2, textBox3, toolStripButton1, textBox4,2);             
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            rp = new ReportsPrint();
            rp.printFreePromotion(textBox2,2);
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) 
            {
                t1 = new Taxi();
                t1.getCabNoForSpecialFreeDays(textBox2, textBox1,textBox5);
            }            
        }

        private void dateTimePicker1_CloseUp(object sender, EventArgs e)
        {
            t1 = new Taxi();

            t1.addSpecialFreeDays(dataGridView1, dateTimePicker1);   
        }

        
    }
}
