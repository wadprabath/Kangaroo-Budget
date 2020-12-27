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
    public partial class Form11 : Form
    {
        public ReceiptNew rcn;
        Taxi t1;
        ReportsPrint rp;
        NewReceiptNumber nrec;
        public Form11()
        {
            InitializeComponent();
        }

        private void Form11_Load(object sender, EventArgs e)
        {
            //ReceiptNew rcnew = new ReceiptNew();
            //textBox2.Text= rcnew.textBox1.Text;
            textBox2.Text = ((ReceiptNew)rcn).textBox1.Text;
            textBox2.Focus();
            textBox2.Select();
            

            nrec = new NewReceiptNumber();
            textBox3.Text=nrec.getPromotionRecNo();
           

        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) 
            {
                t1 = new Taxi();
                t1.getLastDayForFreeDays(textBox2,textBox1,dataGridView1);
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
             t1 = new Taxi();
             t1.saveFreeWorkingDays(dataGridView1, textBox1, textBox2, textBox3,toolStripButton1,textBox4,1);             
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
              rp=new ReportsPrint();
              rp.printFreePromotion(textBox2,1);
           

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            textBox1.Text = "";
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
             t1 = new Taxi();             
             t1.clearFreeDayForm(dataGridView1,textBox1,textBox2,textBox3,toolStripButton1,textBox4);
             nrec = new NewReceiptNumber();
             textBox3.Text = nrec.getPromotionRecNo();

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {

        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        
    }
}
