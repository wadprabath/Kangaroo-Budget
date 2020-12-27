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
    public partial class FrmSpReports : Form
    {
         ReportsPrint rp;

        public FrmSpReports()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            rp=new ReportsPrint();
            rp.notWorkingNotPaid(dateTimePicker1.Value,dateTimePicker3.Value,textBox1);
            //rp.notWorkingNotPaid(DateTime from,DateTime ldate, int backDays);
        }

        private void button28_Click(object sender, EventArgs e)
        {
            rp = new ReportsPrint();
            rp.lastPaymentDates();
        }

        private void button29_Click(object sender, EventArgs e)
        {
            rp = new ReportsPrint();
            rp.lastPaymentDatesGivenRange(dateTimePicker1.Value);
        }

        private void button2_Click(object sender, EventArgs e)
        {
             rp = new ReportsPrint();
             rp.cabAnalyser(dateTimePicker1.Value, dataGridView1, dataGridView2,dataGridView3,dataGridView4,label5,label6,label7,1); 
        }

        private void button3_Click(object sender, EventArgs e)
        {
            rp = new ReportsPrint();
            rp.cabAnalyser(dateTimePicker1.Value, dataGridView1, dataGridView2, dataGridView3, dataGridView4, label5, label6, label7,2); 
        }
    }
}
