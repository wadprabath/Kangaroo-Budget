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
    public partial class Form18 : Form
    {
        ReportsPrint rprint = new ReportsPrint();
        public Form18()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            rprint.SecettedOtherRecipPrint(dataGridView1);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            rprint.findOtherReceipt(textBox1, dataGridView1);
        }
    }
}
