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
    public partial class FrmLocationVoucherRpt : Form
    {

        Taxi t1;
        ReportsPrint repots;

        public FrmLocationVoucherRpt()
        {
            InitializeComponent();
        }

        private void FrmLocationVoucherRpt_Load(object sender, EventArgs e)
        {
            t1 = new Taxi();
            t1.fillLocationComboBox(comboBox1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            repots = new ReportsPrint();
            string[] split = (comboBox1.Text).Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            string location = split[0];
            repots.printWithRefVoucherUserWiseMonthGroupLocationWise(dateTimePicker1.Value, dateTimePicker2.Value, location, comboBox1.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            repots = new ReportsPrint();
            string[] split = (comboBox1.Text).Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            string location = split[0];
            repots.printWithoutRefVoucherLocationWise(dateTimePicker1.Value, dateTimePicker2.Value, location, comboBox1.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            repots = new ReportsPrint();
            string[] split = (comboBox1.Text).Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            string location = split[0];
            repots.printVoucherDeductRefundLocationWise(dateTimePicker1.Value, dateTimePicker2.Value, location, comboBox1.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            repots = new ReportsPrint();
            repots.printMobileLoanRecoverySummary(dateTimePicker1.Value, dateTimePicker2.Value);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            repots = new ReportsPrint();
            repots.printNewDeductionREfundAllCabMonthly(dateTimePicker1.Value, dateTimePicker2.Value, comboBox2, comboBox3);
        }
    }
}
