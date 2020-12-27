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
    public partial class Form13 : Form
    {
        ReportsPrint rptprint;
        public Form13()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
           
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
            
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void Form13_Load(object sender, EventArgs e)
        {
            panel1.Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            rptprint = new ReportsPrint();
            rptprint.printWithRefVoucherUserWise(dateTimePicker1.Value, dateTimePicker2.Value);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
        }
    }
}
