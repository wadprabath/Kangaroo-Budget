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
    
    public partial class Form16 : Form
    {

        ReportsPrint rptprint;
        User us;
        Taxi t1;

        public Form16()
        {
            InitializeComponent();
        }        

        private void Form16_Load(object sender, EventArgs e)
        {
            us = new User();
            t1 = new Taxi();
            textBox1.Text = us.getCurrentUser();
            comboBox1.Text = t1.EnteredLocation(); us = new User();
            //t1 = new Taxi();
            //textBox1.Text = us.getCurrentUser();
            //comboBox1.Text = t1.VoucherEnteredLocation();
            t1.getLastPrintedTime(textBox1,dateTimePicker3,dateTimePicker1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            rptprint =new ReportsPrint();
            rptprint.printWithRefVoucherUserWiseTimeRange(dateTimePicker1.Value,dateTimePicker2.Value);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            rptprint =new ReportsPrint();
            rptprint.printWithRefVoucherUserWiseTimeRangeMonthGroup(dateTimePicker1.Value, dateTimePicker2.Value);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //rptprint = new ReportsPrint();
            //rptprint.printWithRefVoucherUserWiseTimeRangeMonthGroupCreditCard(dateTimePicker1.Value, dateTimePicker2.Value);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            rptprint = new ReportsPrint();
            rptprint.printVoucherCancellationInfoUSerWise(dateTimePicker1.Value, dateTimePicker2.Value);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            rptprint = new ReportsPrint();
            rptprint.printVoucherCancellationInfo(dateTimePicker1.Value, dateTimePicker2.Value);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            rptprint = new ReportsPrint();
            rptprint.printVoucherDeductRefundUserVice(dateTimePicker1.Value, dateTimePicker2.Value);
        }

        private void dateTimePicker3_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            rptprint = new ReportsPrint();
            rptprint.printWithoutRefVoucherUserWiseTimeRangeMonthGroup(dateTimePicker1.Value, dateTimePicker2.Value);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            rptprint = new ReportsPrint();
            rptprint.printVoucherPhoneBillRecoveryDaily(dateTimePicker1.Value, dateTimePicker2.Value);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            rptprint = new ReportsPrint();
            rptprint.printMobileLoanRecoveryDaily(dateTimePicker1.Value, dateTimePicker2.Value);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            rptprint = new ReportsPrint();
            rptprint.printAppPhoneFineChargesDaily(dateTimePicker1.Value, dateTimePicker2.Value);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            rptprint = new ReportsPrint();
            rptprint.printAppPhoneFineRefundDaily(dateTimePicker1.Value, dateTimePicker2.Value);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            rptprint = new ReportsPrint();
            rptprint.printNewVoucherDeductRefundUserVice(dateTimePicker1.Value, dateTimePicker2.Value);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            rptprint = new ReportsPrint();
            rptprint.printWithRefVoucherUserWiseTimeRangeMonthGroupWallet(dateTimePicker1.Value, dateTimePicker2.Value);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            rptprint = new ReportsPrint();
            rptprint.printWithRefVoucherUserWiseTimeRangeMonthGroupCreditCard(dateTimePicker1.Value, dateTimePicker2.Value);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            rptprint = new ReportsPrint();
            rptprint.printWithRefVoucherUserWiseTimeRangeMonthGroupCorperate(dateTimePicker1.Value, dateTimePicker2.Value);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            rptprint = new ReportsPrint();
            rptprint.printWithRefVoucherUserWiseTimeRangeMonthGroupTouch(dateTimePicker1.Value, dateTimePicker2.Value);

        }

        private void button16_Click_1(object sender, EventArgs e)
        {
            rptprint = new ReportsPrint();
            rptprint.printWithRefVoucherUserWiseTimeRangeMonthGroupTouch(dateTimePicker1.Value, dateTimePicker2.Value);
        }

        private void button15_Click_1(object sender, EventArgs e)
        {
            rptprint = new ReportsPrint();
            rptprint.printWithRefVoucherUserWiseTimeRangeMonthGroupCorperate(dateTimePicker1.Value, dateTimePicker2.Value);
        }

        private void button14_Click_1(object sender, EventArgs e)
        {
            rptprint = new ReportsPrint();
            rptprint.printWithRefVoucherUserWiseTimeRangeMonthGroupCreditCard(dateTimePicker1.Value, dateTimePicker2.Value);
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            rptprint = new ReportsPrint();
            rptprint.printWithRefVoucherUserWiseTimeRangeMonthGroup(dateTimePicker1.Value, dateTimePicker2.Value);
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            rptprint = new ReportsPrint();
            rptprint.printWithRefVoucherUserWiseTimeRange(dateTimePicker1.Value, dateTimePicker2.Value);
        }

        private void button17_Click(object sender, EventArgs e)
        {
            rptprint = new ReportsPrint();
            rptprint.printWithRefVoucherUserWiseTimeRangeMonthGroupSLT(dateTimePicker1.Value, dateTimePicker2.Value);
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void button18_Click(object sender, EventArgs e)
        {
            rptprint = new ReportsPrint();
            rptprint.printWithRefVoucherUserWiseTimeRangeMonthGroupCallUP(dateTimePicker1.Value, dateTimePicker2.Value);
        }

        private void button19_Click(object sender, EventArgs e)
        {
            rptprint = new ReportsPrint();
            rptprint.printWithRefVoucherUserWiseTimeRangeMonthGroupToken(dateTimePicker1.Value, dateTimePicker2.Value);
        }

        private void button20_Click(object sender, EventArgs e)
        {
            rptprint = new ReportsPrint();
            rptprint.printWithRefVoucherUserWiseTimeRangeMonthGroupAddCom(dateTimePicker1.Value, dateTimePicker2.Value);
        }

       
    }
}
