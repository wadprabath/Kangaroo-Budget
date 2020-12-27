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
    public partial class FrmRportTime : Form
    {
        User us;
        ReportsPrint rp;
        public FrmRportTime()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            us = new User();
            us.logoutTime(DateTime.Now, textBox1.Text, dateTimePicker2,dateTimePicker1, button11);
        }

        private void FrmRportTime_Load(object sender, EventArgs e)
        {
            us = new User();
            textBox1.Text = us.getCurrentUser();
            us.getLoginTime(textBox1.Text, dateTimePicker1);
            button11.Enabled = false;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            rp = new ReportsPrint();
            rp.DailyPaymentWithPhoneBillUserWiseTimeRange(dateTimePicker1.Value, dateTimePicker2.Value);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button11.Enabled = true;
        }
    }
}
