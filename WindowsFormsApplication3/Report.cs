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
    public partial class Report : Form
    {
        ReportsPrint rp;

        public Report()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            rp = new ReportsPrint();
            rp.PayementForGivenDate(dateTimePicker1.Value);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            rp = new ReportsPrint();
            rp.MobitelBillForGivenMonth(comboBox1.Text,comboBox2.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            rp = new ReportsPrint();
            rp.BrandedMonthlyIncomeTaxi(dateTimePicker1.Value, dateTimePicker2.Value);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            rp = new ReportsPrint();
            rp.NormalPerDayTaxiIncome(dateTimePicker1.Value, dateTimePicker2.Value);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            rp = new ReportsPrint();
            rp.DailyPaymentWithPhoneBill(dateTimePicker1.Value, dateTimePicker2.Value);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            rp = new ReportsPrint();
            rp.DateWisePaymentWithPhoneBill(dateTimePicker1.Value, dateTimePicker2.Value);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            rp = new ReportsPrint();
            rp.printFreeDayCabWise(dateTimePicker1.Value, dateTimePicker2.Value);
        }

        private void button8_Click(object sender, EventArgs e)
        {
              rp = new ReportsPrint();
              rp.printFreeDayDateWise(dateTimePicker1.Value, dateTimePicker2.Value);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            rp = new ReportsPrint();
            rp.printOtherPayment(dateTimePicker1.Value,dateTimePicker2.Value);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            rp = new ReportsPrint();
            rp.printOtherPaymentUserWise(dateTimePicker1.Value, dateTimePicker2.Value);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            rp = new ReportsPrint();
            rp.DailyPaymentWithPhoneBillUserWise(dateTimePicker1.Value, dateTimePicker2.Value);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            rp = new ReportsPrint();
            rp.printBrandedFreeDays(dateTimePicker1.Value, dateTimePicker2.Value);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            rp = new ReportsPrint();
            rp.printSimDepoRefund(dateTimePicker1.Value, dateTimePicker2.Value);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            rp = new ReportsPrint();
            rp.printBrandedCars(dateTimePicker1.Value, dateTimePicker2.Value);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            rp = new ReportsPrint();
            rp.NormalMonthlyIncomeTaxi(dateTimePicker1.Value, dateTimePicker2.Value);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            rp = new ReportsPrint();
            rp.BrandedMonthlyIncomeTaxi(dateTimePicker1.Value, dateTimePicker2.Value);
        }

        private void button17_Click(object sender, EventArgs e)
        {
             rp = new ReportsPrint();
             rp.BrandedPaymentUserWise(dateTimePicker1.Value, dateTimePicker2.Value);
        }

        private void button18_Click(object sender, EventArgs e)
        {
            rp = new ReportsPrint();
            rp.BrandedPaymentAllUsers(dateTimePicker1.Value, dateTimePicker2.Value);
        }

        private void button19_Click(object sender, EventArgs e)
        {
            rp = new ReportsPrint();
            rp.NightCars(dateTimePicker1.Value, dateTimePicker2.Value);
          
        }

        private void button20_Click(object sender, EventArgs e)
        {
            rp = new ReportsPrint();
            rp.printBankDepositedPayment(dateTimePicker1.Value, dateTimePicker2.Value);
        }

        private void button21_Click(object sender, EventArgs e)
        {
            rp = new ReportsPrint();
            rp.printCancellationInfo(dateTimePicker1.Value, dateTimePicker2.Value);
        }

        private void button22_Click(object sender, EventArgs e)
        {
            rp = new ReportsPrint();
            rp.printCancellationInfoUSerWise(dateTimePicker1.Value, dateTimePicker2.Value);
        }

        private void button23_Click(object sender, EventArgs e)
        {
            rp = new ReportsPrint();
            rp.MobitelBillForGivenMonthYard(comboBox1.Text, comboBox2.Text);
        }

        private void button24_Click(object sender, EventArgs e)
        {
            rp = new ReportsPrint();
            rp.MobitelBillForGivenMonthHeadOffice(comboBox1.Text, comboBox2.Text);
        }

        private void button26_Click(object sender, EventArgs e)
        {
            rp = new ReportsPrint();
            rp.FineUserWise(dateTimePicker1.Value, dateTimePicker2.Value);
        }

        private void button25_Click(object sender, EventArgs e)
        {
            rp = new ReportsPrint();
            rp.FineAllUser(dateTimePicker1.Value, dateTimePicker2.Value);
        }

        private void button27_Click(object sender, EventArgs e)
        {
            rp = new ReportsPrint();
            rp.printOtherPaymentCancellaion(dateTimePicker1.Value, dateTimePicker2.Value);
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

        private void button30_Click(object sender, EventArgs e)
        {
            rp = new ReportsPrint();
            rp.BrandedCarMobitelBillForGivenMonth(comboBox1.Text, comboBox2.Text);           
         }

        private void button31_Click(object sender, EventArgs e)
        {
            rp = new ReportsPrint();
            rp.printVoucherPhoneBillRecoverySummary(dateTimePicker1.Value, dateTimePicker2.Value);
        }

        private void button32_Click(object sender, EventArgs e)
        {
            rp = new ReportsPrint();
            rp.printPendingPhoneBill();
        }

        private void button33_Click(object sender, EventArgs e)
        {
            rp = new ReportsPrint();
            rp.DailyMagnetCarPaymentWithPhoneBillUserWise(dateTimePicker1.Value, dateTimePicker2.Value);
        }

        private void button34_Click(object sender, EventArgs e)
        {
            rp = new ReportsPrint();
            rp.MagnetlMonthlyIncomeTaxi(dateTimePicker1.Value, dateTimePicker2.Value);
        }

        private void button35_Click(object sender, EventArgs e)
        {
            rp = new ReportsPrint();
            rp.MagnetPerDayTaxiIncome(dateTimePicker1.Value, dateTimePicker2.Value);
        }

        private void button36_Click(object sender, EventArgs e)
        {
            rp = new ReportsPrint();
            rp.printSpecialFreeDays(dateTimePicker1.Value, dateTimePicker2.Value);
        }

        private void button38_Click(object sender, EventArgs e)
        {
            rp = new ReportsPrint();
            rp.AppRentalAllUser(dateTimePicker1.Value, dateTimePicker2.Value);
        }

        private void button39_Click(object sender, EventArgs e)
        {
            rp = new ReportsPrint();
            rp.AppRentlForGivenMonth(comboBox1.Text, comboBox2.Text);
        }

        private void button37_Click(object sender, EventArgs e)
        {
             rp = new ReportsPrint();
             rp. AppRentalUserWise(dateTimePicker1.Value, dateTimePicker2.Value);
           
        }

        }
}
