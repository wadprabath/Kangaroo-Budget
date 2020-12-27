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
    public partial class Form15 : Form
    {
        ReportsPrint rptprint;
        User us;
        Taxi t1;
        public Form15()
        {
            InitializeComponent();
        }

        private void crystalReportViewer1_Click(object sender, EventArgs e)
        {
            
        }
        protected override bool ProcessCmdKey(ref Message Message, Keys KeyData)
        {
            //check on condition that which key pressed
            if (KeyData == Keys.P)
            {
                MessageBox.Show("test");
                //put your code what you want to do on P button press
            }

            // call the base class to handle other key events
            return base.ProcessCmdKey(ref Message, KeyData);
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            rptprint = new ReportsPrint();
            rptprint.printWithRefVoucherUserWise(dateTimePicker1.Value, dateTimePicker2.Value);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            rptprint = new ReportsPrint();
            rptprint.printWithoutRefVoucherUserWise(dateTimePicker1.Value, dateTimePicker2.Value);
        }

        private void Form14_Load(object sender, EventArgs e)
        {
            us = new User();
            t1 = new Taxi();
            textBox1.Text = us.getCurrentUser();
            comboBox1.Text = t1.EnteredLocation();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            rptprint = new ReportsPrint();
            rptprint.printWithRefVoucherLocationWise(dateTimePicker1.Value, dateTimePicker2.Value, comboBox1.Text);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            rptprint = new ReportsPrint();
            rptprint.printWithoutRefVoucherLocationWise(dateTimePicker1.Value, dateTimePicker2.Value, comboBox1.Text);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            rptprint = new ReportsPrint();
            rptprint.printWithRefVoucherYardAndOffice(dateTimePicker1.Value, dateTimePicker2.Value);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            rptprint = new ReportsPrint();
            rptprint.printWithoutRefVoucherYardAndOffice(dateTimePicker1.Value, dateTimePicker2.Value);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            rptprint = new ReportsPrint();
            rptprint.printVoucherPaymentProof(dateTimePicker1.Value, dateTimePicker2.Value);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            rptprint = new ReportsPrint();
            rptprint.printVoucherHire(dateTimePicker1.Value, dateTimePicker2.Value);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            rptprint = new ReportsPrint();
            rptprint.printVoucherHireCabWise(dateTimePicker1.Value, dateTimePicker2.Value, textBox2.Text);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            rptprint = new ReportsPrint();
            rptprint.VoucherPaymentSummary(dateTimePicker1.Value, dateTimePicker2.Value);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            rptprint = new ReportsPrint();
            rptprint.printVoucherPaymentSummaryWithref(dateTimePicker1.Value, dateTimePicker2.Value);            
        }

        private void button14_Click(object sender, EventArgs e)
        {
            rptprint = new ReportsPrint();
            rptprint.selectVoucherReportPrint(textBox1.Text, textBox2.Text, comboBox1.Text, dateTimePicker1, dateTimePicker2, radioButton1, radioButton2, radioButton3, radioButton4, radioButton5, radioButton6, radioButton7, radioButton8, radioButton9, radioButton10, radioButton11, radioButton12, radioButton13, radioButton14, radioButton15, radioButton16, radioButton17, radioButton18, radioButton19, radioButton20, radioButton21, radioButton22, radioButton23, radioButton24, radioButton25,radioButton26,radioButton27, dataGridView1, comboBox2, comboBox3,radioButton28,radioButton29,radioButton30);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            rptprint = new ReportsPrint();
            rptprint.printVoucherHireFillGrid(dateTimePicker1.Value, dateTimePicker2.Value,dataGridView1);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            //rptprint = new ReportsPrint();
            //rptprint.testReport();


        }

        private void radioButton19_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void radioButton27_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
