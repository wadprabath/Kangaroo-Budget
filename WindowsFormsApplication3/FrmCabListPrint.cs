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
    public partial class FrmCabListPrint : Form
    {
        ReportsPrint rptprnt;
        public FrmCabListPrint()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            rptprnt =new ReportsPrint();
            rptprnt.printActiveCabList(dateTimePicker1, dateTimePicker2);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            rptprnt = new ReportsPrint();
            rptprnt.printWithdrawnCabList(dateTimePicker1, dateTimePicker2);
        }
    }
}
