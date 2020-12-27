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
    public partial class FrmLocationRpt : Form
    {
        Taxi t1;
        ReportsPrint repots;
        ReportsPrint rptprint;
        public FrmLocationRpt()
        {
            InitializeComponent();
        }

        private void FrmLocationRpt_Load(object sender, EventArgs e)
        {
            t1 = new Taxi();
            t1.fillLocationComboBox(comboBox1);
        }

        private void button11_Click(object sender, EventArgs e)
        {
             repots = new ReportsPrint();           
             string[] split =  (comboBox1.Text).Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
             string location = split[0];             
             repots.DailyPaymentWithPhoneBillLocationWise(dateTimePicker1.Value, dateTimePicker2.Value,location, comboBox1.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        { 
            repots = new ReportsPrint();
            string[] split = (comboBox1.Text).Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            string location = split[0];
            repots.printOtherPaymentLocationWise(dateTimePicker1.Value, dateTimePicker2.Value, location, comboBox1.Text);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            repots = new ReportsPrint();
            string[] split = (comboBox1.Text).Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            string location = split[0];
            repots.MobitelBillLocationWise(dateTimePicker1.Value, dateTimePicker2.Value, location, comboBox1.Text,comboBox3.Text,comboBox2.Text);

        }

        private void button3_Click(object sender, EventArgs e)
        {
            repots = new ReportsPrint();
            string[] split = (comboBox1.Text).Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            string location = split[0];
            repots.MobitelBillLocationWiseMonthWise(dateTimePicker1.Value, dateTimePicker2.Value, location, comboBox1.Text, comboBox3.Text, comboBox2.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            repots = new ReportsPrint();
            string[] split = (comboBox1.Text).Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            string location = split[0];
            repots.FineAllUserLocationWise(dateTimePicker1.Value, dateTimePicker2.Value, location, comboBox1.Text);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            repots = new ReportsPrint();
            string[] split = (comboBox1.Text).Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            string location = split[0];
            repots.BrandedMobitelBillLocationWise(dateTimePicker1.Value, dateTimePicker2.Value, location, comboBox1.Text, comboBox3.Text, comboBox2.Text);

        }

        private void button6_Click(object sender, EventArgs e)
        {

            repots = new ReportsPrint();
            string[] split = (comboBox1.Text).Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            string location = split[0];
            repots.BrandedMobitelBillLocationWiseMonthWise(dateTimePicker1.Value, dateTimePicker2.Value, location, comboBox1.Text, comboBox3.Text, comboBox2.Text);
        }

       

       

       
    }
}
