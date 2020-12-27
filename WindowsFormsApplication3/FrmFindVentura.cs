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
    public partial class FrmFindVentura : Form
    {
        Taxi t1;
        public FrmFindVentura()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            t1 = new Taxi();
            t1.findVenturaReceipt(textBox1,dataGridView1);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            t1 = new Taxi();
            string recno = t1.SecettedVenturaRecipPrint(dataGridView1);
        }
    }
}
