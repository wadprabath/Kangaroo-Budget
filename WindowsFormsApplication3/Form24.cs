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
    public partial class Form24 : Form
    {
        ReportsPrint rprint;

        public Form24()
        {
            InitializeComponent();
        }
          
        private void button1_Click(object sender, EventArgs e)
        {
            rprint = new ReportsPrint();
            rprint.selectPrintOption(radioButton1, radioButton2, radioButton3, dateTimePicker1, dateTimePicker2, textBox1, textBox2);
        }
    }
}
